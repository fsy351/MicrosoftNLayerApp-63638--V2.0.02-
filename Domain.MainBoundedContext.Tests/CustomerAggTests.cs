

namespace Domain.MainBoundedContext.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class CustomerAggTests
    {

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CustomerCannotAssociateTransientCountry()
        {
            //Arrange
            Country country = new Country()
            {
                CountryName ="Spain"
            };

            //Act
            Customer customer = new Customer();
            customer.SetCountry(country);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void CustomerCannotAssociateNullCountry()
        {
            //Arrange
            Country country = new Country()
            {
                CountryName = "Spain"
            };

            //Act
            Customer customer = new Customer();
            customer.SetCountry(null);
        }
        [TestMethod()]
        public void CustomerSetCountryFixCountryId()
        {
            //Arrange
            Country country = new Country();
            country.Id = IdentityGenerator.NewSequentialGuid();

            //Act
            Customer customer = new Customer();
            customer.SetCountry(country);

            //Assert
            Assert.AreEqual(country.Id, customer.CountryId);   
        }
        [TestMethod()]
        public void CustomerDisableSetIsEnabledToFalse()
        {
            //Arrange 
            Customer customer = new Customer();

            //Act
            customer.Disable();

            //assert
            Assert.IsFalse(customer.IsEnabled);   
        }
        [TestMethod()]
        public void CustomerEnableSetIsEnabledToTrue()
        {
            //Arrange 
            Customer customer = new Customer();

            //Act
            customer.Enable();

            //assert
            Assert.IsTrue(customer.IsEnabled);
        }
        [TestMethod()]
        public void CustomerFactoryWithCountryEntityCreateValidCustomer()
        {
            //Arrange
            string lastName = "El rojo";
            string firstName = "Jhon";

            Guid countryId = IdentityGenerator.NewSequentialGuid();
            Country country = new Country()
            {
                Id = countryId
            };

            //Act
            Customer customer = CustomerFactory.CreateCustomer(firstName,lastName, country,new Address("city","zipcode","AddressLine1","AddressLine2"));
            var validationContext = new ValidationContext(customer, null, null);
            var validationResults = customer.Validate(validationContext);

            //Assert
            Assert.AreEqual(customer.LastName, lastName);
            Assert.AreEqual(customer.FirstName, firstName);
            Assert.AreEqual(customer.Country, country);
            Assert.AreEqual(customer.CountryId, countryId);
            Assert.AreEqual(customer.IsEnabled, true);
            Assert.AreEqual(customer.CreditLimit, 1000M);

            Assert.IsFalse(validationResults.Any());
        }
        [TestMethod()]
        public void CustomerFactoryWithCountryIdEntityCreateValidCustomer()
        {
            //Arrange
            string lastName = "El rojo";
            string firstName = "Jhon";

            Guid countryId = IdentityGenerator.NewSequentialGuid();
          

            //Act
            Customer customer = CustomerFactory.CreateCustomer(firstName, lastName, countryId, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            var validationContext = new ValidationContext(customer, null, null);
            var validationResults = customer.Validate(validationContext);

            //Assert
            Assert.AreEqual(customer.LastName, lastName);
            Assert.AreEqual(customer.FirstName, firstName);
            Assert.AreEqual(customer.Country, null);
            Assert.AreEqual(customer.CountryId, countryId);
            Assert.AreEqual(customer.IsEnabled, true);
            Assert.AreEqual(customer.CreditLimit, 1000M);

            Assert.IsFalse(validationResults.Any());
        }
        [TestMethod()]
        public void CustomerFactoryWithCreditLimitCreateValidCustomer()
        {
            //Arrange
            string lastName = "El rojo";
            string firstName = "Jhon";

            Guid countryId = IdentityGenerator.NewSequentialGuid();


            //Act
            Customer customer = CustomerFactory.CreateCustomer(firstName, lastName, countryId,2000M);
            var validationContext = new ValidationContext(customer, null, null);
            var validationResults = customer.Validate(validationContext);

            //Assert
            Assert.AreEqual(customer.LastName, lastName);
            Assert.AreEqual(customer.FirstName, firstName);
            Assert.AreEqual(customer.Country, null);
            Assert.AreEqual(customer.CountryId, countryId);
            Assert.AreEqual(customer.IsEnabled, true);
            Assert.AreEqual(customer.CreditLimit, 2000M);

            Assert.IsFalse(validationResults.Any());
        }
    }
}
