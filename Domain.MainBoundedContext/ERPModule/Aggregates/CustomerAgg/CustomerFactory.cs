//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================


namespace Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg
{
    using System;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.Resources;

    /// <summary>
    /// This is the factory for Customer creation, which means that the main purpose
    /// is to encapsulate the creation knowledge.
    /// What is created is a transient entity instance, with nothing being said about persistence as yet
    /// </summary>
    public static class CustomerFactory
    {
        /// <summary>
        /// Create a new transient customer
        /// </summary>
        /// <param name="firstName">The customer firstName</param>
        /// <param name="lastName">The customer lastName</param>
        /// <param name="country">The associated country to this customer</param>
        /// <returns>A valid customer</returns>
        public static Customer CreateCustomer(string firstName, string lastName,Country country,Address address)
        {
            //create the customer instance
            var customer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName
            };

            //set customer address
            customer.Address = address;

            //set default picture relation with no data
            customer.Picture = new Picture();
            

            //TODO: By default this is the limit for customer credit, you can set this 
            //parameter customizable via configuration or other system
            customer.CreditLimit = 1000M;

            //Associate country
            customer.SetCountry(country);

            //by default this customer is enabled
            customer.Enable();

            return customer;
        }

        /// <summary>
        /// Create a new transient customer
        /// </summary>
        /// <param name="firstName">The customer firstName</param>
        /// <param name="lastName">The customer lastName</param>
        /// <param name="country">The associated country to this customer</param>
        /// <returns>A valid customer</returns>
        public static Customer CreateCustomer(string firstName, string lastName, Guid countryId,Address address)
        {
            //create the customer instance
            var customer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName
            };

            //set address
            customer.Address = address;

            //TODO: By default this is the limit for customer credit, you can set this 
            //parameter customizable via configuration or other system
            customer.CreditLimit = 1000M;

            //set country identifier
            customer.CountryId = countryId;

            customer.Enable();

            return customer;
        }

        /// <summary>
        /// Create a new transient customer
        /// </summary>
        /// <param name="firstName">The customer firstName</param>
        /// <param name="lastName">The customer lastName</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="creditLimit">The customer credit Limit</param>
        /// <returns>A validCustomer</returns>
        public static Customer CreateCustomer(string firstName, string lastName, Guid countryId, decimal creditLimit)
        {
            //create the customer instance
            var customer = new Customer()
            {
                FirstName = firstName,
                LastName = lastName
            };

            //set credit limit
            customer.CreditLimit = creditLimit;

            //set country identifier
            customer.CountryId = countryId;

            //by default all customer  enabled
            customer.Enable();

            return customer;
        }
    }
}
