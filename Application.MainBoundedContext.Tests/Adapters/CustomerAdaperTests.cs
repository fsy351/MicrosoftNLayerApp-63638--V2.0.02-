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


namespace Application.MainBoundedContext.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CustomerAdaperTests
    {
        [TestMethod]
        public void CustomerToCustomerDTOAdapt()
        {
            //Arrange

            Guid idCustomer = IdentityGenerator.NewSequentialGuid();
            Guid idCountry = IdentityGenerator.NewSequentialGuid();
            Customer customer = new Customer()
            {
                Id = idCustomer,
                FirstName = "Jhon",
                LastName = "El rojo",
                CreditLimit = 1000M,
                Telephone = "617404929",
                Company = "Spirtis",
                Address = new Address("Monforte", "27400", "AddressLine1", "AddressLine2"),
                Picture = new Picture() { Id = idCustomer, RawPhoto = new byte[0]{} }
            };

            customer.SetCountry(new Country() { Id = idCountry, CountryName = "Spain", CountryISOCode = "es-ES" });

            //Act

            ITypeAdapter adapter = PrepareTypeAdapter();
            var dto = adapter.Adapt<Customer, CustomerDTO>(customer);

            //Assert

            Assert.AreEqual(idCustomer, dto.Id);
            Assert.AreEqual(customer.FirstName, dto.FirstName);
            Assert.AreEqual(customer.LastName, dto.LastName);
            Assert.AreEqual(customer.Company, dto.Company);
            Assert.AreEqual(customer.Telephone, dto.Telephone);
            Assert.AreEqual(customer.CreditLimit, dto.CreditLimit);

            Assert.AreEqual(customer.Country.CountryName, dto.CountryCountryName);
            Assert.AreEqual(idCountry, dto.CountryId);

            Assert.AreEqual(customer.Picture.RawPhoto, dto.PictureRawPhoto);

            Assert.AreEqual(customer.Address.City, dto.AddressCity);
            Assert.AreEqual(customer.Address.ZipCode, dto.AddressZipCode);
            Assert.AreEqual(customer.Address.AddressLine1, dto.AddressAddressLine1);
            Assert.AreEqual(customer.Address.AddressLine2, dto.AddressAddressLine2);
        }

        [TestMethod]
        public void CustomerEnumerableToCustomerListDTOListAdapt()
        {
            //Arrange
            Guid idCustomer = IdentityGenerator.NewSequentialGuid();
            Guid idCountry = IdentityGenerator.NewSequentialGuid();
            Customer customer = new Customer()
            {
                Id = idCustomer,
                FirstName = "Unai",
                LastName = "Zorrilla",
                CreditLimit = 1000M,
                Telephone = "617404929",
                Company = "Spirtis",
                Address = new Address("Monforte", "27400", "AddressLine1", "AddressLine2"),
                Picture = new Picture() { Id = idCustomer, RawPhoto = new byte[0] { } }
            };

            customer.SetCountry(new Country() { Id = idCountry, CountryName = "Spain", CountryISOCode = "es-ES" });

            IEnumerable<Customer> customers = new List<Customer>() { customer };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();

            var dtos = adapter.Adapt<IEnumerable<Customer>, List<CustomerListDTO>>(customers);

            //Assert

            Assert.IsNotNull(dtos);
            Assert.IsTrue(dtos.Any());
            Assert.IsTrue(dtos.Count == 1);

            CustomerListDTO dto = dtos[0];

            Assert.AreEqual(idCustomer, dto.Id);
            Assert.AreEqual(customer.FirstName, dto.FirstName);
            Assert.AreEqual(customer.LastName, dto.LastName);
            Assert.AreEqual(customer.Company, dto.Company);
            Assert.AreEqual(customer.Telephone, dto.Telephone);
            Assert.AreEqual(customer.CreditLimit, dto.CreditLimit);
            Assert.AreEqual(customer.Address.City, dto.AddressCity);
            Assert.AreEqual(customer.Address.ZipCode, dto.AddressZipCode);
            Assert.AreEqual(customer.Address.AddressLine1, dto.AddressAddressLine1);
            Assert.AreEqual(customer.Address.AddressLine2, dto.AddressAddressLine2);
        

        }

        ITypeAdapter PrepareTypeAdapter()
        {
            TypeAdapter adapter = new TypeAdapter(new RegisterTypesMap[]{new ERPModuleRegisterTypesMap()});

            return adapter;
        }
    }
}
