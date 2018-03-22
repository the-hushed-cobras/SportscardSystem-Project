﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Bytes2you.Validation;
using SportscardSystem.Data;
using SportscardSystem.Data.Contracts;
using SportscardSystem.DTO;
using SportscardSystem.DTO.Contracts;
using SportscardSystem.Logic.Services.Contracts;
using SportscardSystem.Models;
using System;
using System.Linq;

namespace SportscardSystem.Logic.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ISportscardSystemDbContext dbContext;
        private readonly IMapper mapper;

        public CompanyService(ISportscardSystemDbContext dbContext, IMapper mapper)
        {
            Guard.WhenArgument(dbContext, "DbContext can not be null").IsNull().Throw();
            Guard.WhenArgument(mapper, "Mapper can not be null").IsNull().Throw();

            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public void AddCompany(ICompanyDto companyDto)
        {
            Guard.WhenArgument(companyDto, "CompanyDto can not be null").IsNull().Throw();

            var companyToAdd = this.mapper.Map<Company>(companyDto);

            if (!this.dbContext.Companies.Any(c => c.Name == companyDto.Name))
            {
                this.dbContext.Companies.Add(companyToAdd);
                this.dbContext.SaveChanges();
            }
            else
            {
                throw new ArgumentException("A company with the same name already exists!");
            }
        }

        //To be implemented
        public void DeleteCompany(ICompanyDto companyDto)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ICompanyDto> GetAllCompanies()
        {
            try
            {
                var allCompanies = dbContext.Companies.ProjectTo<CompanyDto>();
                return allCompanies;
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Companies can not be null");
            }
        }

        
    }
}
