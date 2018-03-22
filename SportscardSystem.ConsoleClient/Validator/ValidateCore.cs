﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportscardSystem.ConsoleClient.Core.Providers.Contracts;

namespace SportscardSystem.ConsoleClient.Validator
{
    public class ValidateCore : IValidateCore
    {
        private readonly IReader reader;
        private readonly IWriter writer;

        public ValidateCore(IWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException();
        }
        //Validations
        public int IntFromString(string commandParameter, string parameterName)
        {
            int parsedValue = int.TryParse(commandParameter, out parsedValue)
                ? parsedValue
                : throw new ArgumentException($"Please provide a valid integer value for {parameterName}!");

            return parsedValue;
        }

        public void ClientAgeValidation(int? age, string parameterName)
        {
            if (age < 18 || age > 100)
            {
                throw new ArgumentException("Client age should be from 18 to 100 years old.");
            }
        }

        
    }
}