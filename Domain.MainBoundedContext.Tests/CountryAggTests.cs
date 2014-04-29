

namespace Domain.MainBoundedContext.Tests
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class CountryAggTests
    {
        [TestMethod()]
        public void CountryWithNullNameProduceValidationError()
        {
            //Arrange
            Country country = new Country();
            country.CountryName = null;

            ValidationContext validationContext = new ValidationContext(country, null, null);

            //Act
            var validationResults = country.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("CountryName"));
        }
        [TestMethod()]
        public void CountryWithEmptyNameProduceValidationError()
        {
            //Arrange
            Country country = new Country();
            country.CountryName = null;

            ValidationContext validationContext = new ValidationContext(country, null, null);

            //Act
            var validationResults = country.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("CountryName"));
        }
        [TestMethod()]
        public void CountryWithNullIsoCodeProduceValidationError()
        {
            //Arrange
            Country country = new Country();
            country.CountryName = "Spain";
            country.CountryISOCode = null;

            ValidationContext validationContext = new ValidationContext(country, null, null);

            //Act
            var validationResults = country.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("CountryISOCode"));
        }
        [TestMethod()]
        public void CountryWithEmptyIsoCodeProduceValidationError()
        {
            //Arrange
            Country country = new Country();
            country.CountryName = "Spain";
            country.CountryISOCode = string.Empty;

            ValidationContext validationContext = new ValidationContext(country, null, null);

            //Act
            var validationResults = country.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("CountryISOCode"));
        }
    }
}
