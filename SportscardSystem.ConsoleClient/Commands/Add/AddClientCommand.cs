﻿using Bytes2you.Validation;
using SportscardSystem.ConsoleClient.Commands.Abstract;
using SportscardSystem.ConsoleClient.Commands.Contracts;
using SportscardSystem.ConsoleClient.Core.Factories.Contracts;
using SportscardSystem.ConsoleClient.Validator;
using SportscardSystem.Logic.Services.Contracts;
using System;
using System.Collections.Generic;

namespace SportscardSystem.ConsoleClient.Commands.Add
{
    public class AddClientCommand : Command, ICommand
    {
        private IClientService clientService;
        private readonly IValidateCore coreValidator;

        public AddClientCommand(ISportscardFactory sportscardFactory, IClientService clientService, IValidateCore coreValidator)
            : base(sportscardFactory)
        {
            Guard.WhenArgument(sportscardFactory, "Sportscard factory can not be null!").IsNull().Throw();
            Guard.WhenArgument(clientService, "Client service can not be null!").IsNull().Throw();
            Guard.WhenArgument(coreValidator, "Validator can not be null!").IsNull().Throw();

            this.clientService = clientService;
            this.coreValidator = coreValidator;
        }

        public string Execute(IList<string> parameters)
        {
            string clientFirstName;
            string clientLastName;
            int? clientAge;
            string companyName;

            try
            {
                clientFirstName = parameters[0];
                clientLastName = parameters[1];
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to parse AddClient command parameters.");
            }
            Guid companyId;

            Guard.WhenArgument(parameters.Count, "Parameters count.").IsGreaterThan(4).Throw();
            Guard.WhenArgument(clientFirstName, "Client first name.").IsNullOrEmpty().Throw();
            Guard.WhenArgument(clientLastName, "Client last name.").IsNullOrEmpty().Throw();

            //1 Validation command lenght 
            if (parameters.Count == 3)
            {
                companyName = parameters[2];
                clientAge = null;
            }
            else
            {
                clientAge = this.coreValidator.IntFromString(parameters[2], "Client's age.");
                Guard.WhenArgument(clientAge, "Clients Age.").IsNull().Throw();

                this.coreValidator.ClientAgeValidation(clientAge, "Client's age");
                companyName = parameters[3];
            }

            Guard.WhenArgument(companyName, "Company Name").IsNullOrEmpty().Throw();
            companyId = this.clientService.GetCompanyGuidByName(companyName);

            var client = this.SportscardFactory.CreateClientDto(clientFirstName, clientLastName, clientAge, companyId);
            Guard.WhenArgument(client, "Client DTO").IsNull().Throw();
            this.clientService.AddClient(client);

            return $"{clientFirstName} {clientLastName} client was added to database.";
        }
    }
}
