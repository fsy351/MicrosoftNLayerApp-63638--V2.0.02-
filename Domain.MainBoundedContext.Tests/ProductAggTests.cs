

namespace Domain.MainBoundedContext.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using System.ComponentModel.DataAnnotations;

    [TestClass()]
    public class ProductAggTests
    {
        [TestMethod()]
        public void ProductFactoryCreateAValidProduct()
        {
            //Arrange

            string title = "title";
            string description ="description";

            //Act
            Product product = ProductFactory.CreateProduct<Software>(title, description,0,0);

            var validationContext = new ValidationContext(product, null, null);
            var validationResuls = product.Validate(validationContext);


            //Assert
            Assert.IsNotNull(product);
            Assert.AreEqual(title, product.Title);
            Assert.AreEqual(description, product.Description);
            Assert.AreEqual(0, product.UnitPrice);
            Assert.AreEqual(0, product.AmountInStock);
            Assert.IsInstanceOfType(product, typeof(Software));

            Assert.IsFalse(validationResuls.Any());
        }
    }
}
