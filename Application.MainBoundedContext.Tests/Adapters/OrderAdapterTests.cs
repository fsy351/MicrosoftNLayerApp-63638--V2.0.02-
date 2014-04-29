

namespace Application.MainBoundedContext.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;

    [TestClass()]
    public class OrderAdapterTests
    {
        [TestMethod()]
        public void OrderToOrderDTOAdapter()
        {
            //Arrange

            Customer customer = new Customer();
            customer.Id = IdentityGenerator.NewSequentialGuid();
            customer.FirstName ="Unai";
            customer.LastName ="Zorrilla";

            Product product = new Software();
            product.Id = IdentityGenerator.NewSequentialGuid();
            product.Title = "the product title";
            product.Description = "the product description";

            Order order = new Order();
            order.Id = IdentityGenerator.NewSequentialGuid();
            order.OrderDate = DateTime.Now;
            order.ShippingInformation = new ShippingInfo("shippingName","shippingAddress","shippingCity","shippingZipCode");
            order.SetCustomer(customer);

            var orderLine = order.CreateOrderLine(product, 10, 10, 10);
            order.AddOrderLine(orderLine);

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var orderDTO = adapter.Adapt<Order, OrderDTO>(order);

            //Assert
            Assert.AreEqual(orderDTO.Id, order.Id);
            Assert.AreEqual(orderDTO.OrderDate, order.OrderDate);
            Assert.AreEqual(orderDTO.DeliveryDate, order.DeliveryDate);

            Assert.AreEqual(orderDTO.ShippingAddress, order.ShippingInformation.ShippingAddress);
            Assert.AreEqual(orderDTO.ShippingCity, order.ShippingInformation.ShippingCity);
            Assert.AreEqual(orderDTO.ShippingName, order.ShippingInformation.ShippingName);
            Assert.AreEqual(orderDTO.ShippingZipCode, order.ShippingInformation.ShippingZipCode);

            Assert.AreEqual(orderDTO.CustomerFullName, order.Customer.FullName);
            Assert.AreEqual(orderDTO.CustomerId, order.Customer.Id);


            Assert.IsNotNull(orderDTO.OrderLines);
            Assert.IsTrue(orderDTO.OrderLines.Any());

            Assert.AreEqual(orderDTO.OrderLines[0].Id, orderLine.Id);
            Assert.AreEqual(orderDTO.OrderLines[0].Amount, orderLine.Amount);
            Assert.AreEqual(orderDTO.OrderLines[0].Discount, orderLine.Discount);
            Assert.AreEqual(orderDTO.OrderLines[0].UnitPrice, orderLine.UnitPrice);
            Assert.AreEqual(orderDTO.OrderLines[0].TotalLine, orderLine.TotalLine);
            Assert.AreEqual(orderDTO.OrderLines[0].ProductId, product.Id);
            Assert.AreEqual(orderDTO.OrderLines[0].ProductTitle, product.Title);

        }

        [TestMethod()]
        public void EnumerableOrderToOrderListDTOAdapter()
        {
            //Arrange

            Customer customer = new Customer();
            customer.Id = IdentityGenerator.NewSequentialGuid();
            customer.FirstName = "Unai";
            customer.LastName = "Zorrilla";

            Product product = new Software();
            product.Id = IdentityGenerator.NewSequentialGuid();
            product.Title = "the product title";
            product.Description = "the product description";

            Order order = new Order();
            order.Id = IdentityGenerator.NewSequentialGuid();
            order.OrderDate = DateTime.Now;
            order.ShippingInformation = new ShippingInfo("shippingName", "shippingAddress", "shippingCity", "shippingZipCode");
            order.SetCustomer(customer);

            var line = order.CreateOrderLine(product.Id, 1, 200, 0);
            order.AddOrderLine(line);

            var orders = new List<Order>() { order };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var orderListDTO = adapter.Adapt<IEnumerable<Order>, List<OrderListDTO>>(orders);

            //Assert
            Assert.AreEqual(orderListDTO[0].Id, order.Id);
            Assert.AreEqual(orderListDTO[0].OrderDate, order.OrderDate);
            Assert.AreEqual(orderListDTO[0].DeliveryDate, order.DeliveryDate);
            Assert.AreEqual(orderListDTO[0].TotalOrder, order.GetOrderTotal());

            Assert.AreEqual(orderListDTO[0].ShippingAddress, order.ShippingInformation.ShippingAddress);
            Assert.AreEqual(orderListDTO[0].ShippingCity, order.ShippingInformation.ShippingCity);
            Assert.AreEqual(orderListDTO[0].ShippingName, order.ShippingInformation.ShippingName);
            Assert.AreEqual(orderListDTO[0].ShippingZipCode, order.ShippingInformation.ShippingZipCode);

            Assert.AreEqual(orderListDTO[0].CustomerFullName, order.Customer.FullName);
            Assert.AreEqual(orderListDTO[0].CustomerId, order.Customer.Id);


           

        }

        [TestMethod()]
        public void OrderLineToOrderLineDTOAdapter()
        {
            //Arrange
            Product product = new Software();
            product.Id = IdentityGenerator.NewSequentialGuid();
            product.Title = "The product title";
            product.Description = "The product description";

            OrderLine orderLine = new OrderLine()
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                Amount = 1,
                Discount = 0,
                UnitPrice = 10,
                ProductId = product.Id,
            };
            orderLine.SetProduct(product);


            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var orderLineDTO = adapter.Adapt<OrderLine, OrderLineDTO>(orderLine);

            //Assert
            Assert.AreEqual(orderLineDTO.Amount, orderLine.Amount);
            Assert.AreEqual(orderLineDTO.Id, orderLine.Id);
            Assert.AreEqual(orderLineDTO.Discount, orderLine.Discount);
            Assert.AreEqual(orderLineDTO.ProductId, orderLine.ProductId);
            Assert.AreEqual(orderLineDTO.ProductTitle, orderLine.Product.Title);
            Assert.AreEqual(orderLineDTO.UnitPrice, orderLine.UnitPrice);
            Assert.AreEqual(orderLineDTO.TotalLine, orderLine.TotalLine);

        }

        [TestMethod()]
        public void EnumerableOrderLineToListOrderLineDTOAdapter()
        {
            //Arrange
            Product product = new Software();
            product.Id = IdentityGenerator.NewSequentialGuid();
            product.Title = "The product title";
            product.Description = "The product description";

            OrderLine orderLine = new OrderLine()
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                Amount = 1,
                Discount = 0,
                UnitPrice = 10,
                ProductId = product.Id,
            };
            orderLine.SetProduct(product);

            IEnumerable<OrderLine> lines = new List<OrderLine>(){orderLine};

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var orderLinesDTO = adapter.Adapt<IEnumerable<OrderLine>, List<OrderLineDTO>>(lines);

            //Assert
            Assert.IsNotNull(orderLinesDTO);
            Assert.IsTrue(orderLinesDTO.Any());

            Assert.AreEqual(orderLinesDTO[0].Amount, orderLine.Amount);
            Assert.AreEqual(orderLinesDTO[0].Id, orderLine.Id);
            Assert.AreEqual(orderLinesDTO[0].Discount, orderLine.Discount);
            Assert.AreEqual(orderLinesDTO[0].ProductId, orderLine.ProductId);
            Assert.AreEqual(orderLinesDTO[0].ProductTitle, orderLine.Product.Title);
            Assert.AreEqual(orderLinesDTO[0].UnitPrice, orderLine.UnitPrice);
            Assert.AreEqual(orderLinesDTO[0].TotalLine, orderLine.TotalLine);

        }

        ITypeAdapter PrepareTypeAdapter()
        {
            TypeAdapter adapter = new TypeAdapter(new RegisterTypesMap[] { new ERPModuleRegisterTypesMap() });

            return adapter;
        }
    }
}
