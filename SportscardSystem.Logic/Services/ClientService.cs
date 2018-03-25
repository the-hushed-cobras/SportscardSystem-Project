﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bytes2you.Validation;
using SportscardSystem.Data.Contracts;
using SportscardSystem.DTO;
using SportscardSystem.DTO.Contracts;
using SportscardSystem.Logic.Services.Contracts;
using SportscardSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportscardSystem.Logic.Services
{
    public class ClientService : IClientService
    {
        private readonly ISportscardSystemDbContext dbContext;
        private readonly IMapper mapper;

        public ClientService(ISportscardSystemDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException("Context can't be null.");
            this.mapper = mapper ?? throw new ArgumentNullException("Mapper can't be null.");
        }

        public void AddClient(IClientDto clientDto)
        {
            Guard.WhenArgument(clientDto, "ClientDto can not be null").IsNull().Throw();

            var clientToAdd = this.mapper.Map<Client>(clientDto);

            this.dbContext.Clients.Add(clientToAdd);
            this.dbContext.SaveChanges();
        }

        public void DeleteClient(string firstName, string lastName, int? age)
        {
            var client = this.dbContext.Clients.FirstOrDefault(x => !x.IsDeleted && x.Age == age && x.FirstName == firstName && x.LastName == lastName);
            Guard.WhenArgument(client, "There is no client with this params").IsNull().Throw();

            client.IsDeleted = true;
            client.DeletedOn = DateTime.Now;

            foreach (var sportscard in client.Sportscards)
            {
                sportscard.IsDeleted = true;
                sportscard.DeletedOn = DateTime.Now;
            }

            foreach (var visit in client.Visits)
            {
                visit.IsDeleted = true;
                visit.DeletedOn = DateTime.Now;
            }

             this.dbContext.SaveChanges();
        }

        public IEnumerable<IClientDto> GetAllClients()
        {
            var allClients = this.dbContext.Clients.Where(c => !c.IsDeleted).ProjectTo<ClientDto>().ToList();
            Guard.WhenArgument(allClients, "AllClients can not be null").IsNull().Throw();

            Console.WriteLine("test");
            return allClients;
        }

        public IClientDto GetMostActiveClient()
        {
            var mostActiveClient = this.dbContext.Clients.Where(c => !c.IsDeleted).OrderByDescending(c => c.Visits.Count()).ThenBy(c => c.FirstName).FirstOrDefault();
            Guard.WhenArgument(mostActiveClient, "Most active client can not be null!").IsNull().Throw();

            var mostActiveClientDto = this.mapper.Map<ClientDto>(mostActiveClient);

            return mostActiveClientDto;
        }

        public Guid GetCompanyGuidByName(string companyName)
        {
            Guid result;

            try
            {
                result = this.dbContext.Companies.FirstOrDefault(x => x.Name == companyName).Id;
            }
            catch (Exception)
            {

                throw new ArgumentException("No such company exists!");
            }

            return result;
        }
    }
}
