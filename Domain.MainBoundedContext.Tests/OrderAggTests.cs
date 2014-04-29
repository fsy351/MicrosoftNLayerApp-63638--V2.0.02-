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
			

namespace Domain.MainBoundedContext.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using System.ComponentModel.DataAnnotations;

    [TestClass()]
    public class OrderAggTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void OrderCannotSetTransientCustomer()
        {
            //Arrange 
            Customer customer = new Customer();

            Order order = new Order();

            //Act
            order.SetCustomer(customer);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void OrderCannotSetNullCustomer()
        {
            //Arrange 
            Customer customer = new Customer();

            Order order = new Order();

            //Act
            order.SetCustomer(customer);
        }

        [TestMethod()]
        public void OrderSetDeliveredSetDateAndState()
        {
            //Arrange 
            Order order = new Order();

            //Act
            order.SetOrderAsDelivered();

            //Assert
            Assert.IsTrue(order.IsDelivered);
            Assert.IsNotNull(order.DeliveryDate);
            Assert.IsTrue(order.DeliveryDate != default(DateTime));
        }
        [TestMethod()]
        public void OrderAddOrderLineFixOrderId()
        {
            //Arrange
            string shippingName= "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.Id = IdentityGenerator.NewSequentialGuid();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);
            order.Id = IdentityGenerator.NewSequentialGuid();

            var line = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(), 1, 1, 0);

            //Act
            order.AddOrderLine(line);

            //Assert
            Assert.AreEqual(order.Id, line.OrderId);
        }

        [TestMethod()]
        public void OrderGetTotalOrderSumLines()
        {
            //Arrange
            string shippingName= "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.Id = IdentityGenerator.NewSequentialGuid();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            OrderLine line1 = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(),1,500, 10);
            OrderLine line2 = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(),2,300, 10);

            //Act
            order.AddOrderLine(line1);
            order.AddOrderLine(line2);

            decimal expected = ((1 * 500) * (1 -(10M/100M))) + ((2*300) *(1-(10M/100M)));
            decimal actual = order.GetOrderTotal();

            //Assert
            Assert.AreEqual(expected,actual);
        }
        [TestMethod()]
        public void OrderDiscountInOrderLineCanBeZero()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.Id = IdentityGenerator.NewSequentialGuid();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            OrderLine line1 = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(), 1, 500, 0);
            OrderLine line2 = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(), 2, 300, 0);

            //Act
            order.AddOrderLine(line1);
            order.AddOrderLine(line2);

            decimal expected = ((1 * 500) * (1 - (0M / 100M))) + ((2 * 300) * (1 - (0M / 100M)));
            decimal actual = order.GetOrderTotal();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void OrderDiscountLessThanZeroIsEqualToZeroDiscount()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.Id = IdentityGenerator.NewSequentialGuid();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            OrderLine line1 = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(), 1, 500, -10);
            OrderLine line2 = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(), 2, 300, -10);

            //Act
            order.AddOrderLine(line1);
            order.AddOrderLine(line2);

            decimal expected = ((1 * 500) * (1 - (0M / 100M))) + ((2 * 300) * (1 - (0M / 100M)));
            decimal actual = order.GetOrderTotal();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void OrderDiscountGreatherThan100IsEqualTo100Discount()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.Id = IdentityGenerator.NewSequentialGuid();

            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);

            OrderLine line1 = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(), 1, 500, 101);
            OrderLine line2 = order.CreateOrderLine(IdentityGenerator.NewSequentialGuid(), 2, 300, 101);

            //Act
            order.AddOrderLine(line1);
            order.AddOrderLine(line2);

            decimal expected = ((1 * 500) * (1 - (100M / 100M))) + ((2 * 300) * (1 - (100M / 100M)));
            decimal actual = order.GetOrderTotal();

            //Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void OrderFactoryCreateValidOrder()
        {
            //Arrange
            
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            Customer customer = new Customer();
            customer.Id = IdentityGenerator.NewSequentialGuid();

            //Act
            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);
            var validationContext = new ValidationContext(order, null, null);
            var validationResult = order.Validate(validationContext);

            //Assert
            ShippingInfo shippingInfo = new ShippingInfo(shippingName, shippingAddress, shippingCity, shippingZipCode);

            Assert.AreEqual(shippingInfo, order.ShippingInformation);
            Assert.AreEqual(order.Customer, customer);
            Assert.AreEqual(order.CustomerId, customer.Id);
            Assert.IsFalse(order.IsDelivered);
            Assert.IsNull(order.DeliveryDate);
            Assert.IsTrue(order.OrderDate != default(DateTime));
            Assert.IsFalse(validationResult.Any());
        }
        [TestMethod()]
        public void IsCreditValidForOrderReturnTrueIfTotalOrderIsLessThanCustomerCredit()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            var customer = CustomerFactory.CreateCustomer("jhon", "el rojo", Guid.NewGuid(), new Address("city", "zipCode", "address line1", "addres line2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            //Act
            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);
            var orderLine = order.CreateOrderLine(Guid.NewGuid(), 1, 240, 0); // this is less that 1000 ( default customer credit )

            order.AddOrderLine(orderLine);

            //assert
            var result = order.IsCreditValidForOrder();

            //Assert
            Assert.IsTrue(result);


        }
        [TestMethod()]
        public void IsCreditValidForOrderReturnFalseIfTotalOrderIsGreaterThanCustomerCredit()
        {
            //Arrange
            string shippingName = "shippingName";
            string shippingCity = "shippingCity";
            string shippingZipCode = "shippingZipCode";
            string shippingAddress = "shippingAddress";

            var customer = CustomerFactory.CreateCustomer("jhon", "el rojo", Guid.NewGuid(), new Address("city", "zipCode", "address line1", "addres line2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            //Act
            Order order = OrderFactory.CreateOrder(customer, shippingName, shippingCity, shippingAddress, shippingZipCode);
            var orderLine = order.CreateOrderLine(Guid.NewGuid(), 100, 240, 0); // this is greater that 1000 ( default customer credit )

            order.AddOrderLine(orderLine);

            //assert
            var result = order.IsCreditValidForOrder();

            //Assert
            Assert.IsFalse(result);
        }
    }
}
