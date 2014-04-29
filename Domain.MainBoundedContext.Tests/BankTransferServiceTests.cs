
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

    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Services;

    [TestClass()]
    public class BankTransferServiceTests
    {
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PerformTransferThrowExceptionIfSourceCantWithdrawedWithLockedAccount()
        {
            //Arrange
            var source = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), new BankAccountNumber("1111","2222","3333333333","01"));
            source.DepositMoney(1000, "initial load");
            source.Lock();

            var target = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), new BankAccountNumber("1111", "2222", "3333333333", "01"));


            //Act
            var bankTransferService = new BankTransferService();
            bankTransferService.PerformTransfer(10, source, target);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PerformTransferThrowExceptionIfSourceCantWithdrawedWithExceedAmoung()
        {
            //Arrange
            var source = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), new BankAccountNumber("1111", "2222", "3333333333", "01"));
            source.DepositMoney(1000, "initial load");

            var target = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), new BankAccountNumber("1111", "2222", "3333333333", "01"));


            //Act
            var bankTransferService = new BankTransferService();
            bankTransferService.PerformTransfer(2000, source, target);
        }
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void PerformTransferThrowExceptionIfTargetIsLockedAccount()
        {
            //Arrange
            var source = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), new BankAccountNumber("1111", "2222", "3333333333", "01"));
            source.DepositMoney(1000, "initial load");

            var target = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), new BankAccountNumber("1111", "2222", "3333333333", "01"));
            target.Lock();

            //Act
            var bankTransferService = new BankTransferService();
            bankTransferService.PerformTransfer(10, source, target);
        }
        [TestMethod()]
        public void PerformTransferCreateActivities()
        {
            //Arrange
            var source = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), new BankAccountNumber("1111", "2222", "3333333333", "01"));
            source.DepositMoney(1000, "initial load");

            var target = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), new BankAccountNumber("1111", "2222", "3333333333", "01"));
            

            //Act

            int activities = source.BankAccountActivity.Count;

            var bankTransferService = new BankTransferService();
            bankTransferService.PerformTransfer(50, source, target);

            //Assert
            Assert.IsNotNull(source.BankAccountActivity);
            Assert.AreEqual(++activities, source.BankAccountActivity.Count);
            
        }
    }
}
