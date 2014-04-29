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
    using Microsoft.Samples.NLayerApp.Application.Seedwork;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork.Moles;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg.Moles;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg.Moles;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.NetFramework.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Validator;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class CustomerManagemenetServiceTests
    {
        [TestMethod()]
        public void AddNewCustomerReturnNullIfCustomerDtoIsNull()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);


            //act
            var result = customerManagementService.AddNewCustomer(null);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void AddNewCustomerReturnNullIfCustomerCountryInformationIsEmpty()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            var customerDTO = new CustomerDTO()
            {
                CountryId = Guid.Empty
            };

            //act
            var result = customerManagementService.AddNewCustomer(customerDTO);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void AddNewCustomerReturnAdaptedDTO()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.AddCustomer = (customer) => { };
            customerRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            var customerDTO = new CustomerDTO()
            {
                CountryId = Guid.NewGuid(),
                FirstName = "Jhon",
                LastName = "El rojo"
            };

            //act
            var result = customerManagementService.AddNewCustomer(customerDTO);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id != Guid.Empty);
            Assert.AreEqual(result.FirstName, customerDTO.FirstName);
            Assert.AreEqual(result.LastName, customerDTO.LastName);
        }


        [TestMethod()]
        [ExpectedException(typeof(ApplicationValidationErrorsException))]
        public void AddNewCustomerThrowApplicationErrorsWhenEntityIsNotValid()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.AddCustomer = (customer) => { };
            customerRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            var customerDTO = new CustomerDTO() //missing lastname
            {
                CountryId = Guid.NewGuid(),
                FirstName = "Jhon"
            };

            //act
            var result = customerManagementService.AddNewCustomer(customerDTO);
        }
        [TestMethod()]
        public void RemoveCustomerSetCustomerAsDisabled()
        {
            //Arrange
            Guid countryGuid = Guid.NewGuid();
            Guid customerId = Guid.NewGuid();
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();

            customerRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };

            var customer = CustomerFactory.CreateCustomer("Jhon","El rojo",countryGuid,new Address("city", "zipCode", "address line", "address line"));
            customer.Id = customerId;


            customerRepository.GetGuid = (guid) =>
            {
                return customer;
            };

            //Act
            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);
            customerManagementService.RemoveCustomer(customerId);

            //Assert
            Assert.IsFalse(customer.IsEnabled);
        }

        [TestMethod()]
        public void UpdateCustomerMergePersistentAndCurrent()
        {
            //Arrange
            Guid countryGuid = Guid.NewGuid();
            Guid customerId = Guid.NewGuid();
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();

            customerRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };

            customerRepository.GetGuid = (guid) =>
            {
                var customer = CustomerFactory.CreateCustomer("Jhon",
                                                               "El rojo",
                                                               countryGuid,
                                                               new Address("city", "zipCode", "address line", "address line"));
                customer.Id = customerId;

                return customer;
            };

            customerRepository.MergeCustomerCustomer = (persistent, current) =>
            {
                Assert.AreEqual(persistent, current);
                Assert.IsTrue(persistent != null);
                Assert.IsTrue(current != null);
            };

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            var customerDTO = new CustomerDTO() //missing lastname
            {
                Id = customerId,
                CountryId = countryGuid,
                FirstName = "Jhon",
                LastName = "El rojo",
            };

            //act
            customerManagementService.UpdateCustomer(customerDTO);
        }
        [TestMethod()]
        public void FindCustomersWithInvalidPageArgumentsReturnNull()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var resultInvalidPageIndex = customerManagementService.FindCustomers(-1, 0);
            var resultInvalidPageCount = customerManagementService.FindCustomers(1, 0);

            //Assert
            Assert.IsNull(resultInvalidPageIndex);
            Assert.IsNull(resultInvalidPageCount);
        }
        [TestMethod()]
        public void FindCustomersInPageReturnNullIfNotData()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.GetEnabledInt32Int32 = (index, count) =>
            {
                return new List<Customer>();
            };

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCustomers(0, 1);

            //Assert
            Assert.IsNull(result);

        }
        [TestMethod()]
        public void FindCustomersByFilterReturnNullIfNotData()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.AllMatchingISpecificationOfCustomer = (spec) =>
            {
                return new List<Customer>();
            };

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCustomers("text");

            //Assert
            Assert.IsNull(result);

        }
        [TestMethod()]
        public void FindCustomersInPageMaterializeResults()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.GetEnabledInt32Int32 = (index, count) =>
            {
                var customers = new List<Customer>();
                customers.Add(CustomerFactory.CreateCustomer("Jhon",
                                                            "El rojo",
                                                             Guid.NewGuid(),
                                                             new Address("city","zipCode","address line","address line2")));
                return customers;
            };


            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCustomers(0, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }

        [TestMethod()]
        public void FindCustomersByFilterMaterializeResults()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.AllMatchingISpecificationOfCustomer = (spec) =>
            {
                var customers = new List<Customer>();
                customers.Add(CustomerFactory.CreateCustomer("Jhon",
                                                            "El rojo",
                                                             Guid.NewGuid(),
                                                             new Address("city", "zipCode", "address line", "address line2")));
                return customers;
            };


            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCustomers("Jhon");

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }
        [TestMethod()]
        public void FindCustomerReturnNullIfCustomerIdIsEmpty()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCustomer(Guid.Empty);
            
            //Assert
            Assert.IsNull(result);
            
        }
        [TestMethod()]
        public void FindCustomerMaterializaResultIfExist()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.GetGuid = (guid) =>
            {
                return CustomerFactory.CreateCustomer("Jhon", 
                                                      "El rojo",
                                                      Guid.NewGuid(), 
                                                      new Address("city", "zipCode", "address line1", "address line2"));
                
            };

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCustomer(Guid.NewGuid());

            //Assert
            Assert.IsNotNull(result);
        }
        [TestMethod()]
        public void FindCountriesWithInvalidPageArgumentsReturnNull()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var countryRepository = new SICountryRepository();
            var customerRepository = new SICustomerRepository();

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var resultInvalidPageIndex = customerManagementService.FindCountries(-1, 0);
            var resultInvalidPageCount = customerManagementService.FindCountries(1, 0);

            //Assert
            Assert.IsNull(resultInvalidPageIndex);
            Assert.IsNull(resultInvalidPageCount);
        }
        [TestMethod()]
        public void FindCountriesInPageReturnNullIfNotData()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var countryRepository = new SICountryRepository();
            countryRepository.GetPagedInt32Int32ExpressionOfFuncOfCountryKPropertyBoolean<string>((index, count, order, ascending) =>
            {
                return new List<Country>();
            });

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCountries(0, 1);

            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public void FindCountriesInPageMaterializeResults()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var countryRepository = new SICountryRepository();
            countryRepository.GetPagedInt32Int32ExpressionOfFuncOfCountryKPropertyBoolean<string>((index, count, order, ascending) =>
            {
                return new List<Country>()
                {
                    new Country(){Id = Guid.NewGuid(),CountryName ="country name",CountryISOCode="country iso"}
                };
            });

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCountries(0, 1);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }
        [TestMethod()]
        public void FindCountriesByFilterMaterializeResults()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var countryRepository = new SICountryRepository();
            countryRepository.AllMatchingISpecificationOfCountry = (spec)=>
            {
                return new List<Country>()
                {
                    new Country(){Id = Guid.NewGuid(),CountryName ="country name",CountryISOCode="country iso"}
                };
            };

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCountries("filter");

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }
        [TestMethod()]
        public void FindCountriesByFilterReturnNullIfNotData()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var countryRepository = new SICountryRepository();
            countryRepository.AllMatchingISpecificationOfCountry = (spec) =>
            {
                return new List<Country>();
            };

            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

            //Act
            var result = customerManagementService.FindCountries("filter");

            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionWhenAdapterDependencyIsNull()
        {
            //Arrange
            ITypeAdapter adapter = null;
            var customerRepository = new SICustomerRepository();
            var countryRepository = new SICountryRepository();

            //act
            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionWhenCustomerRepositoryDependencyIsNull()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            SICustomerRepository customerRepository = null;
            var countryRepository = new SICountryRepository();

            //act
            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionWhenCountryRepositoryDependencyIsNull()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            SICountryRepository countryRepository = null;

            //act
            var customerManagementService = new CustomerAppService(adapter, countryRepository, customerRepository);

        }

        #region Initialize

        public ITypeAdapter PrepareTypeAdapter()
        {
            return new TypeAdapter(new RegisterTypesMap[] { new ERPModuleRegisterTypesMap() });
        }

        [ClassInitialize()]
        public static void ClassInitialze(TestContext context)
        {
            // Initialize default  factories

            LoggerFactory.SetCurrent(new TraceSourceLogFactory());
            EntityValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }

        #endregion
    }
}
