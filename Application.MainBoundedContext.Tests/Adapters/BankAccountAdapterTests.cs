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
    using System.Linq;
    using System.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;

    [TestClass()]
    public class BankAccountAdapterTests
    {
        [TestMethod()]
        public void AdaptBankActivityToBankActivityDTO()
        {
            //Arrange
            BankAccountActivity activity = new BankAccountActivity();

            activity.Id = IdentityGenerator.NewSequentialGuid();
            activity.Date = DateTime.Now;
            activity.Amount = 1000;
            activity.ActivityDescription = "transfer...";
            

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var activityDTO = adapter.Adapt<BankAccountActivity, BankActivityDTO>(activity);

            //Assert
            Assert.AreEqual(activity.Date, activityDTO.Date);
            Assert.AreEqual(activity.Amount, activityDTO.Amount);
            Assert.AreEqual(activity.ActivityDescription, activityDTO.ActivityDescription);
        }
        [TestMethod()]
        public void AdaptEnumerableBankActivityToListBankActivityDTO()
        {
            //Arrange
            BankAccountActivity activity = new BankAccountActivity();

            activity.Id = IdentityGenerator.NewSequentialGuid();
            activity.Date = DateTime.Now;
            activity.Amount = 1000;
            activity.ActivityDescription = "transfer...";

            IEnumerable<BankAccountActivity> activities = new List<BankAccountActivity>() { activity };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var activitiesDTO = adapter.Adapt<IEnumerable<BankAccountActivity>, List<BankActivityDTO>>(activities);

            //Assert
            Assert.IsNotNull(activitiesDTO);
            Assert.IsTrue(activitiesDTO.Count()==1);

            Assert.AreEqual(activity.Date, activitiesDTO[0].Date);
            Assert.AreEqual(activity.Amount, activitiesDTO[0].Amount);
            Assert.AreEqual(activity.ActivityDescription, activitiesDTO[0].ActivityDescription);
        }
        [TestMethod()]
        public void AdaptBankAccountToBankAccountDTO()
        {
            //Arrange
            var customer = CustomerFactory.CreateCustomer("jhon", "el rojo", Guid.NewGuid(), new Address("", "", "", ""));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            BankAccount account = new BankAccount();
            account.Id = IdentityGenerator.NewSequentialGuid();
            account.BankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02");
            account.SetCustomer(customer);
            account.DepositMoney(1000, "reason");
            account.Lock();

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var bankAccountDTO = adapter.Adapt<BankAccount, BankAccountDTO>(account);


            //Assert
            Assert.AreEqual(account.Id, bankAccountDTO.Id);
            Assert.AreEqual(account.Iban, bankAccountDTO.BankAccountNumber);
            Assert.AreEqual(account.Balance, bankAccountDTO.Balance);
            Assert.AreEqual(account.Customer.FirstName, bankAccountDTO.CustomerFirstName);
            Assert.AreEqual(account.Customer.LastName, bankAccountDTO.CustomerLastName);
            Assert.AreEqual(account.Locked, bankAccountDTO.Locked);
        }
        [TestMethod()]
        public void AdaptEnumerableBankAccountToListBankAccountListDTO()
        {
            //Arrange
            var customer = CustomerFactory.CreateCustomer("jhon", "el rojo", Guid.NewGuid(), new Address("", "", "", ""));
            customer.Id = IdentityGenerator.NewSequentialGuid();

            BankAccount account = new BankAccount();
            account.Id = IdentityGenerator.NewSequentialGuid();
            account.BankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02");
            account.SetCustomer(customer);
            account.DepositMoney(1000, "reason");
            var accounts = new List<BankAccount>() { account };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var bankAccountsDTO = adapter.Adapt<IEnumerable<BankAccount>, List<BankAccountDTO>>(accounts);


            //Assert
            Assert.IsNotNull(bankAccountsDTO);
            Assert.IsTrue(bankAccountsDTO.Count == 1);

            Assert.AreEqual(account.Id, bankAccountsDTO[0].Id);
            Assert.AreEqual(account.Iban, bankAccountsDTO[0].BankAccountNumber);
            Assert.AreEqual(account.Balance, bankAccountsDTO[0].Balance);
            Assert.AreEqual(account.Customer.FirstName, bankAccountsDTO[0].CustomerFirstName);
            Assert.AreEqual(account.Customer.LastName, bankAccountsDTO[0].CustomerLastName);
        }

        ITypeAdapter PrepareTypeAdapter()
        {
            TypeAdapter adapter = new TypeAdapter(new RegisterTypesMap[] { new BankingModuleRegisterTypesMap() });

            return adapter;
        }
    }
}
