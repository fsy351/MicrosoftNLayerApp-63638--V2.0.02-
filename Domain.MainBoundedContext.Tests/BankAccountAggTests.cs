

namespace Domain.MainBoundedContext.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using System.ComponentModel.DataAnnotations;

    [TestClass()]
    public class BankAccountAggTests
    {
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAccountCannotSetTransientCustomer()
        {
            //Arrange
            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(),new Address("city","zipcode","AddressLine1","AddressLine2"));
            
            var bankAccount = new BankAccount();

            //Act
            bankAccount.SetCustomer(customer);
        }
        [TestMethod()]
        public void BankAccountSetCustomerFixCustomerId()
        {
            //Arrange
            Guid countryId = IdentityGenerator.NewSequentialGuid();
            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", countryId, new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();
            //Act
            BankAccount bankAccount = new BankAccount();
            bankAccount.SetCustomer(customer);

            //Assert
            Assert.AreEqual(customer.Id, bankAccount.CustomerId);
        }
        [TestMethod()]
        public void BankAccountFactoryCreateValidBankAccount()
        {
            //Arrange
            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");

            BankAccount bankAccount = null;

            //Act
            bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = customer.Validate(validationContext);

            //Assert
            Assert.IsNotNull(bankAccount);
            Assert.IsTrue(bankAccount.BankAccountNumber == bankAccountNumber);
            Assert.IsFalse(bankAccount.Locked);
            Assert.IsTrue(bankAccount.CustomerId == customer.Id);

            Assert.IsFalse(validationResults.Any());
        }

        [TestMethod()]
        public void BankAccountLockSetLocked()
        {
            //Arrange

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");

            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.Lock();

            //Assert
            Assert.IsTrue(bankAccount.Locked);

        }
        [TestMethod()]
        public void BankAccountUnLockSetUnLocked()
        {
            //Arrange

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");

            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.Lock();
            bankAccount.UnLock();

            //Assert
            Assert.IsFalse(bankAccount.Locked);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BankAccountCannotWithDrawMoneyInLockedAccount()
        {
            //Arrange

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);
            bankAccount.Lock();

            //Act
            bankAccount.WithdrawMoney(200, "the reason..");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BankAccountCannotWithDrawMoneyIfBalanceIsLessThanAmountAccount()
        {
            //Arrange

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.WithdrawMoney(200, "the reason..");
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void BankAccountCannotDepositMoneyInLockedAccount()
        {
            //Arrange

            var customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);
            bankAccount.Lock();

            //Act
            bankAccount.DepositMoney(200, "the reason..");
        }
        [TestMethod()]
        public void BankAccountDepositMoneyAnotateActivity()
        {
            //Arrange

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            
            BankAccount bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.DepositMoney(1000, activityReason);

            //Assert
            Assert.IsTrue(bankAccount.Balance == 1000);
            Assert.IsNotNull(bankAccount.BankAccountActivity);
            Assert.IsNotNull(bankAccount.BankAccountActivity.Any());
            Assert.IsTrue(bankAccount.BankAccountActivity.Single().Amount == 1000);
            Assert.IsTrue(bankAccount.BankAccountActivity.Single().ActivityDescription == activityReason);
        }
        [TestMethod()]
        public void BankAccountWithdrawMoneyAnotateActivity()
        {
            //Arrange

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();


            BankAccount bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            //Act
            bankAccount.DepositMoney(1000, activityReason);
            bankAccount.WithdrawMoney(1000, activityReason);

            //Assert
            Assert.IsTrue(bankAccount.Balance == 0);
            Assert.IsNotNull(bankAccount.BankAccountActivity);
            Assert.IsNotNull(bankAccount.BankAccountActivity.Any());
            Assert.IsTrue(bankAccount.BankAccountActivity.Last().Amount == -1000);
            Assert.IsTrue(bankAccount.BankAccountActivity.Last().ActivityDescription == activityReason);
        }
        [TestMethod()]
        [ExpectedException(typeof(OverflowException))]
        public void BankAccountDepositMaxDecimalThrowOverflowBalance()
        {
            //Arrange

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();


            BankAccount bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            bankAccount.DepositMoney(1, activityReason);
            bankAccount.DepositMoney(Decimal.MaxValue, activityReason);            
        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void BankAccountCannotDepositMoneyLessThanZero()
        {
            //Arrange

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();


            BankAccount bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            bankAccount.DepositMoney(-100, activityReason);
        }
        [TestMethod()]
        public void BankAccountDepositAndWithdrawSetBalance()
        {
            //Arrange

            var bankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", "01");
            string activityReason = "reason";

            Customer customer = CustomerFactory.CreateCustomer("Unai", "Zorrilla Castro", IdentityGenerator.NewSequentialGuid(), new Address("city", "zipcode", "AddressLine1", "AddressLine2"));
            customer.Id = IdentityGenerator.NewSequentialGuid();


            BankAccount bankAccount = BankAccountFactory.CreateBankAccount(customer, bankAccountNumber);

            Assert.AreEqual(bankAccount.Balance,0);

            bankAccount.DepositMoney(1000, activityReason);
            Assert.AreEqual(bankAccount.Balance, 1000);

            bankAccount.WithdrawMoney(250, activityReason);
            Assert.AreEqual(bankAccount.Balance, 750);
        }
        [TestMethod()]
        public void BankAccountWithNullBankAccountNumberProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = null;

            //act
            var validationContext = new ValidationContext(bankAccount,null,null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("BankAccountNumber"));
        }
        [TestMethod()]
        public void BankAccountWithNullOfficeNumberProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber(null, "2222", "3333333333", "01");
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("OfficeNumber"));
        }
        [TestMethod()]
        public void BankAccountWithEmptyOfficeNumberProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber(string.Empty, "2222", "3333333333", "01");
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("OfficeNumber"));
        }
        [TestMethod()]
        public void BankAccountWithNullNationalBankCodeNumberProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111",null, "3333333333", "01");
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("NationalBankCode"));
        }
        [TestMethod()]
        public void BankAccountWithEmptyNationalBankCodeProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111",string.Empty, "3333333333", "01");
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("NationalBankCode"));
        }
        [TestMethod()]
        public void BankAccountWithNullAccountNumberProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111","2222", null, "01");
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("AccountNumber"));
        }
        [TestMethod()]
        public void BankAccountWithEmptyAccountNumberProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111", string.Empty, string.Empty, "01");
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("AccountNumber"));
        }
        [TestMethod()]
        public void BankAccountWithCheckDigistsProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333",null);
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("CheckDigits"));
        }
        [TestMethod()]
        public void BankAccountWithEmptyCheckDigistsProduceValidationError()
        {
            //Arrange
            BankAccount bankAccount = new BankAccount();
            bankAccount.BankAccountNumber = new BankAccountNumber("1111", "2222", "3333333333", string.Empty);
            //act
            var validationContext = new ValidationContext(bankAccount, null, null);
            var validationResults = bankAccount.Validate(validationContext);

            //assert
            Assert.IsNotNull(validationResults);
            Assert.IsTrue(validationResults.Any());
            Assert.IsTrue(validationResults.First().MemberNames.Contains("CheckDigits"));
        }
    }
}
