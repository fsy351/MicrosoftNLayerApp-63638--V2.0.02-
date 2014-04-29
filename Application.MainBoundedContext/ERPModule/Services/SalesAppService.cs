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


namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using Microsoft.Samples.NLayerApp.Application.Seedwork;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.Resources;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Validator;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;

    /// <summary>
    /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
    /// </summary>
    public class SalesAppService
        : ISalesAppService
    {
        #region Members

        ITypeAdapter _typeAdapter;
        IOrderRepository _orderRepository;
        IProductRepository _productRepository;
        ICustomerRepository _customerRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance of sales management service
        /// </summary>
        /// <param name="typeAdapter">The type adapter to use</param>
        /// <param name="orderRepository">The associated order repository</param>
        /// <param name="productRepository">The associated product repository</param>
        public SalesAppService(ITypeAdapter typeAdapter, //the type adapter
                                      IProductRepository productRepository,//associated product repository
                                      IOrderRepository orderRepository,
                                      ICustomerRepository customerRepository) //the associated order repository
        {
            if (typeAdapter == null)
                throw new ArgumentNullException("typeAdapter");

            if (orderRepository == null)
                throw new ArgumentNullException("orderRepository");

            if (productRepository == null)
                throw new ArgumentNullException("productRepository");

            if (customerRepository == null)
                throw new ArgumentNullException("customerRepository");
            

            _typeAdapter = typeAdapter;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;

        }
        #endregion

        #region ISalesAppService Members

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
        /// </summary>
        /// <param name="pageIndex"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        /// <param name="pageCount"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></returns>
        public List<OrderListDTO> FindOrders(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_InvalidArgumentForFindOrders);
                return null;
            }
            //recover orders in paged fashion
            var orders = _orderRepository.GetPaged<DateTime>(pageIndex, pageCount, o => o.OrderDate, false);

            if (orders != null
                &&
                orders.Any())
            {
                return _typeAdapter.Adapt<IEnumerable<Order>, List<OrderListDTO>>(orders);
            }
            else // no data
                return null;
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
        /// </summary>
        /// <param name="dateFrom"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        /// <param name="dateTo"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></returns>
        public List<OrderListDTO> FindOrders(DateTime? dateFrom, DateTime? dateTo)
        {
            //create the specification ( how to filter orders from dates..)
            var spec = OrdersSpecifications.OrderFromDateRange(dateFrom, dateTo);

            //recover orders
            var orders = _orderRepository.AllMatching(spec);

            if (orders != null
               &&
               orders.Any())
            {
                return _typeAdapter.Adapt<IEnumerable<Order>, List<OrderListDTO>>(orders);
            }
            else //no data
                return null;

        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
        /// </summary>
        /// <param name="customerId"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></returns>
        public List<OrderListDTO> FindOrders(Guid customerId)
        {
            if (customerId != Guid.Empty)
            {
                var orders = _orderRepository.GetFiltered(o => o.CustomerId == customerId);

                if (orders != null
                   &&
                   orders.Any())
                {
                    return _typeAdapter.Adapt<IEnumerable<Order>, List<OrderListDTO>>(orders);
                }
                else //no data..
                    return null;
            }
            else // if customerId is empty return null
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotGetOrdersFromEmptyCustomerId);
                return null;
            }
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
        /// </summary>
        /// <param name="pageIndex"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        /// <param name="pageCount"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></returns>
        public List<ProductDTO> FindProducts(int pageIndex, int pageCount)
        {
            if (pageIndex < 0 || pageCount <= 0)
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_InvalidArgumentForFindProducts);
                return null;
            }

            //recover products
            var products = _productRepository.GetPaged<string>(pageIndex, pageCount, p => p.Title, false);

            if (products != null
                &&
                products.Any())
            {
                return _typeAdapter.Adapt<IEnumerable<Product>, List<ProductDTO>>(products);
            }
            else // no data
                return null;
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
        /// </summary>
        /// <param name="text"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></returns>
        public List<ProductDTO> FindProducts(string text)
        {
            //create the specification ( howto find products for any string ) 
            var spec = ProductSpecifications.ProductFullText(text);

            //recover products
            var products = _productRepository.AllMatching(spec);

            //adapt results
            return _typeAdapter.Adapt<IEnumerable<Product>, List<ProductDTO>>(products);
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
        /// </summary>
        /// <param name="orderDto"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        public OrderDTO AddNewOrder(OrderDTO orderDto)
        {
            //if orderdto data is not valid
            if (orderDto == null || orderDto.CustomerId== Guid.Empty)
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotAddOrderWithNullInformation);
                return null;
            }

            var customer = _customerRepository.Get(orderDto.CustomerId);

            if (customer == null)//if customer is null cannot create a new order
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotCreateOrderForNonExistingCustomer);
                return null;
            }

            //Create a new order entity
            var newOrder = CreateNewOrder(orderDto,customer);

            if (newOrder.IsCreditValidForOrder()) //if total order is less than credit 
            {
                //save order
                SaveOrder(newOrder);

                return _typeAdapter.Adapt<Order, OrderDTO>(newOrder);
            }
            else //total order is greater than credit
            {
                LoggerFactory.CreateLog().LogInfo(Messages.info_OrderTotalIsGreaterCustomerCredit);
                return null;
            }
        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
        /// </summary>
        /// <param name="software"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        public SoftwareDTO AddNewSoftware(SoftwareDTO software)
        {
            if (software == null)
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotAddSoftwareWithNullInformation);
                return null;
            }

            //Create the softare entity
            var newSoftware = ProductFactory.CreateProduct<Software>(software.Title, software.Description,0,0);

            newSoftware.LicenseCode = software.LicenseCode;
            newSoftware.UnitPrice = software.UnitPrice;
            newSoftware.AmountInStock = software.AmountInStock;

            //Assign the poid
            newSoftware.Id = IdentityGenerator.NewSequentialGuid();

            //save software
            SaveProduct(newSoftware);

            //return software dto
            return _typeAdapter.Adapt<Software, SoftwareDTO>(newSoftware);

        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/>
        /// </summary>
        /// <param name="book"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.ISalesAppService"/></param>
        public BookDTO AddNewBook(BookDTO book)
        {
            if (book == null)
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotAddSoftwareWithNullInformation);
                return null;
            }

            //Create the softare entity
            Book newBook = ProductFactory.CreateProduct<Book>(book.Title, book.Description,0,0);

            newBook.ISBN = book.ISBN;
            newBook.Publisher = book.Publisher;
            newBook.UnitPrice = book.UnitPrice;
            newBook.AmountInStock = book.AmountInStock;

            //Assign the poid
            newBook.Id = IdentityGenerator.NewSequentialGuid();

            //save software
            SaveProduct(newBook);

            //return software dto
            return _typeAdapter.Adapt<Book, BookDTO>(newBook);

        }
        #endregion

        #region Private Methods

        void SaveOrder(Order order)
        {
            var entityValidator = EntityValidatorFactory.CreateValidator();

            if (entityValidator.IsValid(order))//if entity is valid save. 
            {
                //add order and commit changes
                _orderRepository.Add(order);
                _orderRepository.UnitOfWork.Commit();
            }
            else // if not valid throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(order));
        }
        Order CreateNewOrder(OrderDTO dto, Customer associatedCustomer)
        {
            //Create a new order entity from factory
            Order newOrder = OrderFactory.CreateOrder(associatedCustomer, 
                                                     dto.ShippingName, 
                                                     dto.ShippingCity, 
                                                     dto.ShippingAddress, 
                                                     dto.ShippingZipCode);

            //set the poid
            newOrder.Id = IdentityGenerator.NewSequentialGuid();

            //if have lines..add
            if (dto.OrderLines != null)
            {
                foreach (var line in dto.OrderLines)
                {
                    var orderLine = newOrder.CreateOrderLine(line.ProductId, line.Amount, line.UnitPrice, line.Discount);
                    newOrder.AddOrderLine(orderLine);
                }
            }

            return newOrder;
        }
        void SaveProduct(Product product)
        {
            var entityValidator = EntityValidatorFactory.CreateValidator();

            if (entityValidator.IsValid(product)) // if is valid
            {
                _productRepository.Add(product);
                _productRepository.UnitOfWork.Commit();
            }
            else //if not valid, throw validation errors
                throw new ApplicationValidationErrorsException(entityValidator.GetInvalidMessages(product));
        }

        #endregion
    }
}
