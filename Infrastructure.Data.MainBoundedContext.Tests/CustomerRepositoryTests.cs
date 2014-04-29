
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
			

namespace Infrastructure.Data.MainBoundedContext.Tests
{
    using System;
    using System.Linq;

    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;

    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainBoundedContext.ERPModule.Repositories;
    using Microsoft.Samples.NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class CustomerRepositoryTests
    {
        [TestMethod()]
        public void CustomerRepositoryGetMethodReturnCustomerWithPicture()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);

            var customerId = new Guid("43A38AC8-EAA9-4DF0-981F-2685882C7C45");
            Customer customer = null;

            //Act
            customer = customerRepository.Get(customerId);

            //Assert
            Assert.IsNotNull(customer);
            Assert.IsNotNull(customer.Picture);
            Assert.IsTrue(customer.Id == customerId);
        }

        [TestMethod()]
        public void CustomerRepositoryGetMethodReturnNullWhenIdIsEmpty()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);

            Customer customer = null;

            //Act
            customer = customerRepository.Get(Guid.Empty);

            //Assert
            Assert.IsNull(customer);
        }

        [TestMethod()]
        public void CustomerRepositoryGetEnalbedReturnOnlyEnabledCustomers()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);

            
            //Act
            var result = customerRepository.GetEnabled(0, 10);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.All(c => c.IsEnabled));
        }

        [TestMethod()]
        public void CustomerRepositoryAddNewItemSaveItem()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);

            var countryId = new Guid("32BB805F-40A4-4C37-AA96-B7945C8C385C");

            var customer = CustomerFactory.CreateCustomer("Felix", "Trend", countryId, new Address("city", "zipCode", "addressLine1", "addressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            customer.Picture = new Picture()
            {
                Id = customer.Id
            };

            //Act

            customerRepository.Add(customer);
            customerRepository.UnitOfWork.Commit();

            //Assert

            var result = customerRepository.Get(customer.Id);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id == customer.Id);
        }

        [TestMethod()]
        public void CustomerRepositoryGetAllReturnMaterializedAllItems()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);

            //Act
            var allItems = customerRepository.GetAll();

            //Assert
            Assert.IsNotNull(allItems);
            Assert.IsTrue(allItems.Any());
        }

        [TestMethod()]
        public void CustomerRepositoryAllMatchingMethodReturnEntitiesWithSatisfiedCriteria()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);
            
            var spec = CustomerSpecifications.EnabledCustomers();

            //Act
            var result = customerRepository.AllMatching(spec);

            //Assert
            Assert.IsNotNull(result.All(c => c.IsEnabled));

        }

        [TestMethod()]
        public void CustomerRepositoryFilterMethodReturnEntitisWithSatisfiedFilter()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);

            //Act
            var result = customerRepository.GetFiltered(c => c.CreditLimit > 0);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(c => c.CreditLimit>0));
        }

        [TestMethod()]
        public void CustomerRepositoryPagedMethodReturnEntitiesInPageFashion()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);

            //Act
            var pageI = customerRepository.GetPaged(0, 1, b => b.Id, false);
            var pageII = customerRepository.GetPaged(1, 1, b => b.Id, false);

            //Assert
            Assert.IsNotNull(pageI);
            Assert.IsTrue(pageI.Count() == 1);

            Assert.IsNotNull(pageII);
            Assert.IsTrue(pageII.Count() == 1);

            Assert.IsFalse(pageI.Intersect(pageII).Any());
        }
        [TestMethod()]
        public void CustomerRepositoryRemoveItemDeleteIt()
        {
            //Arrange
            var unitOfWork = new MainBCUnitOfWork();
            ICustomerRepository customerRepository = new CustomerRepository(unitOfWork);

            var countryId = new Guid("32BB805F-40A4-4C37-AA96-B7945C8C385C");

            var customer = CustomerFactory.CreateCustomer("Frank", "Frank", countryId, new Address("city", "zipCode", "addressline1", "addressline2"));

            customer.Id = IdentityGenerator.NewSequentialGuid();
            customer.Picture = new Picture()
            {
                Id = customer.Id
            };

            customerRepository.Add(customer);
            customerRepository.UnitOfWork.Commit();

            //Act

            customerRepository.Remove(customer);
            customerRepository.UnitOfWork.Commit();

            var result = customerRepository.Get(customer.Id);

            //Assert
            Assert.IsNull(result);
        }
    }
}
