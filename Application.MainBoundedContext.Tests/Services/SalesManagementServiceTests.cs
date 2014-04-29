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


namespace Application.MainBoundedContext.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services;
    using Microsoft.Samples.NLayerApp.Application.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg.Moles;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg.Moles;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg.Moles;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork.Moles;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.NetFramework.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Validator;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class SalesManagementServiceTests
    {
        [TestMethod()]
        public void FindOrdersInPageReturnNullWhenPageDataIsInvalid()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var resultInvalidPageIndex = salesManagement.FindOrders(-1, 1);
            var resultInvalidPageCount = salesManagement.FindOrders(0, 0);

            //Assert
            Assert.IsNull(resultInvalidPageIndex);
            Assert.IsNull(resultInvalidPageCount);
        }
        [TestMethod()]
        public void FindOrdersInPageReturnNullWhenNoData()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();
            orderRepository.GetPagedInt32Int32ExpressionOfFuncOfOrderKPropertyBoolean<DateTime>((index, count, order, ascending) =>
            {
                return new List<Order>();
            });


            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindOrders(-1, 1);


            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public void FindOrdersInDateRangeReturnNullWhenNoData()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();
            orderRepository.AllMatchingISpecificationOfOrder = (spec) =>
            {
                return new List<Order>();
            };

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindOrders(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(+2));


            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public void FindOrderReturnNullIfCustomerIdIsEmpty()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindOrders(Guid.Empty);


            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public void FindOrdersMaterializeResultsIfCustomerExist()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();
            orderRepository.GetFilteredExpressionOfFuncOfOrderBoolean = (filter) =>
            {
                var orders = new List<Order>();
                orders.Add(OrderFactory.CreateOrder(Guid.NewGuid(), "name", "city", "address", "zipcode"));

                return orders;
            };

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindOrders(Guid.NewGuid());


            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);
        }

        [TestMethod()]
        public void FindOrdersInPageMaterializeResults()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();
            orderRepository.GetPagedInt32Int32ExpressionOfFuncOfOrderKPropertyBoolean<DateTime>((index, count, order, ascending) =>
            {
                return new List<Order>()
                {
                    new Order(){Id = Guid.NewGuid(),CustomerId = Guid.NewGuid()}
                };
            });
            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindOrders(0, 1);

            //Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());

        }
        [TestMethod()]
        public void FindOrdersInDateRangeMaterializeResults()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            orderRepository.AllMatchingISpecificationOfOrder = (spec) =>
            {
                return new List<Order>()
                {
                    new Order(){Id = Guid.NewGuid(),CustomerId = Guid.NewGuid()}
                };
            };

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindOrders(DateTime.Now.AddDays(-2), DateTime.Now.AddDays(-1));

            //Assert

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());

        }

        [TestMethod()]
        public void FindProductsInPageReturnNullWhenPageIsInvalid()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var resultInvalidPageIndex = salesManagement.FindProducts(-1, 1);
            var resultInvalidPageCount = salesManagement.FindProducts(0, 0);

            //Assert
            Assert.IsNull(resultInvalidPageIndex);
            Assert.IsNull(resultInvalidPageCount);
        }

        [TestMethod()]
        public void FindProductsInPageReturnNullWhenNoData()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var orderRepository = new SIOrderRepository();
            var productRepository = new SIProductRepository();
            productRepository.GetPagedInt32Int32ExpressionOfFuncOfProductKPropertyBoolean<string>((index, count, order, ascending) =>
            {
                return new List<Product>();
            });

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindProducts(0, 1);


            //Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void FindProductsInPageMaterializeResults()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();
            productRepository.GetPagedInt32Int32ExpressionOfFuncOfProductKPropertyBoolean<string>((index, count, order, ascending) =>
            {
                return new List<Product>()
                {
                    new Book(){Id= Guid.NewGuid(),Title = "title",UnitPrice = 10,Description ="description",ISBN ="isbn",Publisher ="publisher"},
                    new Software(){Id= Guid.NewGuid(),Title = "title",UnitPrice = 10,Description ="description",LicenseCode ="license code"},
                };
            });


            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindProducts(0, 2);


            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
        }
        [TestMethod()]
        public void FindProductsByFilterMaterializeResults()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();
            productRepository.AllMatchingISpecificationOfProduct = (spec) =>
            {
                return new List<Product>()
                {
                    new Book(){Id= Guid.NewGuid(),Title = "title",UnitPrice = 10,Description ="description",ISBN ="isbn",Publisher ="publisher"},
                    new Software(){Id= Guid.NewGuid(),Title = "title",UnitPrice = 10,Description ="description",LicenseCode ="license code"},
                };
            };

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //act
            var result = salesManagement.FindProducts("filter text");

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
        }
        [TestMethod()]
        public void AddNewOrderWithNotValidDataReturnNull()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            var order = new OrderDTO() // order is not valid when customer id is empty
            {
                CustomerId = Guid.Empty
            };

            //act
            var result = salesManagement.AddNewOrder(order);

            //assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public void AddNewValidOrderReturnAddedOrder()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.GetGuid = (guid) =>
            {
                //default credit limit is 1000
                var customer = CustomerFactory.CreateCustomer("Jhon", "El rojo", Guid.NewGuid(), new Address("city", "zipCode", "addressline1", "addressline2"));
                customer.Id = IdentityGenerator.NewSequentialGuid();

                return customer;
            };


            orderRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };
            orderRepository.AddOrder = (order) => { };

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            var dto = new OrderDTO()
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = "Address",
                ShippingCity = "city",
                ShippingName = "name",
                ShippingZipCode = "zipcode",
                OrderLines = new List<OrderLineDTO>()
                {
                    new OrderLineDTO(){ProductId = Guid.NewGuid(),Amount = 1,Discount = 0,UnitPrice = 20}
                }
            };

            //act
            var result = salesManagement.AddNewOrder(dto);

            //assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id != Guid.Empty);
            Assert.AreEqual(result.ShippingAddress, dto.ShippingAddress);
            Assert.AreEqual(result.ShippingCity, dto.ShippingCity);
            Assert.AreEqual(result.ShippingName, dto.ShippingName);
            Assert.AreEqual(result.ShippingZipCode, dto.ShippingZipCode);
            Assert.IsTrue(result.OrderLines.Count == 1);
            Assert.IsTrue(result.OrderLines.All(ol=>ol.Id != Guid.Empty));
        }
        [TestMethod()]
        public void AddNewOrderWithTotalGreaterCustomerCreditReturnNull()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();
            var customerRepository = new SICustomerRepository();
            customerRepository.GetGuid = (guid) =>
            {
                //default credit limit is 1000
                var customer = CustomerFactory.CreateCustomer("Jhon", "El rojo", Guid.NewGuid(), new Address("city", "zipCode", "addressline1", "addressline2"));
                customer.Id = IdentityGenerator.NewSequentialGuid();

                return customer;
            };


            orderRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };
            orderRepository.AddOrder = (order) => { };

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            var dto = new OrderDTO()
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = "Address",
                ShippingCity = "city",
                ShippingName = "name",
                ShippingZipCode = "zipcode",
                OrderLines = new List<OrderLineDTO>()
                {
                    new OrderLineDTO(){ProductId = Guid.NewGuid(),Amount = 1,Discount = 0,UnitPrice = 2000}
                }
            };

            //act
            var result = salesManagement.AddNewOrder(dto);

            //assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void AddNewSoftwareWithNullDataReturnNull()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //Act

            var result = salesManagement.AddNewSoftware(null);

            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        [ExpectedException(typeof(ApplicationValidationErrorsException))]
        public void AddNewSoftwareThrowExceptionWhenDataIsInvalid()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            var dto = new SoftwareDTO()
            {
                Title = "The title",
                Description = "",
                LicenseCode = "license",
                AmountInStock = 10,
                UnitPrice = -1//this is a not valid value
            };

            //Act
            var result = salesManagement.AddNewSoftware(dto);
        }
        [TestMethod()]
        public void AddNewBookWithNullDataReturnNull()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            //Act

            var result = salesManagement.AddNewBook(null);

            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        [ExpectedException(typeof(ApplicationValidationErrorsException))]
        public void AddNewBookThrowExceptionWhenDataIsInvalid()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var productRepository = new SIProductRepository();
            var orderRepository = new SIOrderRepository();

            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            var dto = new BookDTO()
            {
                Title = "The title",
                Description = "description",
                Publisher = "license",
                ISBN = "isbn",
                AmountInStock = 10,
                UnitPrice = -1//this is a not valid value
            };

            //Act
            var result = salesManagement.AddNewBook(dto);

            
        }
        [TestMethod()]
        public void AddNewBookReturnAddedBook()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var orderRepository = new SIOrderRepository();
            var productRepository = new SIProductRepository();

            productRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };

            productRepository.AddProduct = (product) => { };


            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            var dto = new BookDTO()
            {
                Title = "The title",
                Description = "description",
                Publisher = "license",
                ISBN = "isbn",
                AmountInStock = 10,
                UnitPrice = 10
            };

            //Act
            var result = salesManagement.AddNewBook(dto);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id != Guid.Empty);
            Assert.AreEqual(result.Title, dto.Title);
            Assert.AreEqual(result.Description, dto.Description);
            Assert.AreEqual(result.Publisher, dto.Publisher);
            Assert.AreEqual(result.ISBN, dto.ISBN);
            Assert.AreEqual(result.AmountInStock, dto.AmountInStock);
            Assert.AreEqual(result.UnitPrice, dto.UnitPrice);
        }
        [TestMethod()]
        public void AddNewSoftwareReturnAddedSoftware()
        {
            //Arrange 
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var orderRepository = new SIOrderRepository();
            var productRepository = new SIProductRepository();

            productRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };

            productRepository.AddProduct = (product) => { };


            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);

            var dto = new SoftwareDTO()
            {
                Title = "The title",
                Description = "description",
                LicenseCode = "license code",
                AmountInStock = 10,
                UnitPrice = 10
            };

            //Act
            var result = salesManagement.AddNewSoftware(dto);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Id != Guid.Empty);
            Assert.AreEqual(result.Title, dto.Title);
            Assert.AreEqual(result.Description, dto.Description);
            Assert.AreEqual(result.LicenseCode, dto.LicenseCode);
            Assert.AreEqual(result.AmountInStock, dto.AmountInStock);
            Assert.AreEqual(result.UnitPrice, dto.UnitPrice);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionIfAdapterDependencyIsNull()
        {
            //Arrange
            ITypeAdapter adapter =null;
            var customerRepository = new SICustomerRepository();
            var orderRepository = new SIOrderRepository();
            var productRepository = new SIProductRepository();

            //Act
            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionIfOrderRepositoryDependencyIsNull()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            SIOrderRepository orderRepository = null;
            var productRepository = new SIProductRepository();

            //Act
            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionIfProductRepositoryDependencyIsNull()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            var customerRepository = new SICustomerRepository();
            var orderRepository = new SIOrderRepository();
            SIProductRepository productRepository = null;

            //Act
            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository,customerRepository);
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionIfCustomerRepositoryDependencyIsNull()
        {
            //Arrange
            var adapter = PrepareTypeAdapter();
            SICustomerRepository customerRepository = null;
            var orderRepository = new SIOrderRepository();
            var productRepository = new SIProductRepository();

            //Act
            var salesManagement = new SalesAppService(adapter, productRepository, orderRepository, customerRepository);
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

