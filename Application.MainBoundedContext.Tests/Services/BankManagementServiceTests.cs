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

    using Microsoft.Samples.NLayerApp.Application.Seedwork;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOs;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork.Moles;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg.Moles;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Services;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg.Moles;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.NetFramework.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.NetFramework.Validator;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters.Moles;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Validator;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class BankManagementServiceTests
    {
        [TestMethod()]
        public void LockBankAccountReturnFalseIfIdentifierIsEmpty()
        {
            //Arrange
            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.LockBankAccount(Guid.Empty);

            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod()]
        public void LockBankAccountReturnFalseIfBankAccountNotExist()
        {
            //Arrange
            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();
            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            bankAccountRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };
            bankAccountRepository.GetGuid = (guid) =>
            {
                return null;
            };

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.LockBankAccount(Guid.NewGuid());

            //Assert
            Assert.IsFalse(result);
        }
        [TestMethod()]
        public void LockBankAccountReturnTrueIfBankAccountIsLocked()
        {
            //Arrange
            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();
            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            bankAccountRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };

            bankAccountRepository.GetGuid = (guid) =>
            {
                return new BankAccount() { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid() };
            };

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.LockBankAccount(Guid.NewGuid());

            //Assert
            Assert.IsTrue(result);
        }
        [TestMethod()]
        public void AddBankAccountReturnNullWhenBankAccountDTOIsNull()
        {
            //Arrange
            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository,transferService, adapter);

            //Act
            var result = bankingService.AddBankAccount(null);

            //Assert

            Assert.IsNull(result);
        }
        [TestMethod()]
        public void AddBankAccountReturnNullWhenCustomerIdIsEmpty()
        {
            //Arrange
            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();

            var dto = new BankAccountDTO()
            {
                CustomerId = Guid.Empty
            };

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.AddBankAccount(dto);

            //Assert

            Assert.IsNull(result);
        }
        [TestMethod()]
        public void AddBankAccountReturnNullWhenCustomerNotExist()
        {
            //Arrange
            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            SICustomerRepository customerRepository = new SICustomerRepository();
            customerRepository.GetGuid = (guid) => { return null; };

            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();

            var dto = new BankAccountDTO()
            {
                CustomerId = Guid.NewGuid()
            };

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.AddBankAccount(dto);

            //Assert

            Assert.IsNull(result);
        }

        [TestMethod()]
        public void AddBankAccountReturnDTOWhenSaveSucceed()
        {
            //Arrange
            IBankTransferService transferService = new BankTransferService();

            ITypeAdapter adapter = PrepareTypeAdapter();

            SICustomerRepository customerRepository = new SICustomerRepository();
            customerRepository.GetGuid = (guid) =>
            {
                return new Customer()
                {
                    Id = guid,
                    FirstName = "Jhon",
                    LastName = "El rojo"
                };
            };

            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            bankAccountRepository.AddBankAccount = (ba) => { };
            bankAccountRepository.UnitOfWorkGet = () =>
            {
                var uow = new SIUnitOfWork();
                uow.Commit = () => { };

                return uow;
            };


            var dto = new BankAccountDTO()
            {
                CustomerId = Guid.NewGuid(),
                BankAccountNumber = "BA"
            };

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.AddBankAccount(dto);

            //Assert
            Assert.IsNotNull(result);
            
        }
       
        [TestMethod()]
        public void FindBankAccountsReturnAllItems()
        {
            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            bankAccountRepository.GetAll = ()=>
            {
                var accounts = new List<BankAccount>()
                {
                    new BankAccount()
                    {
                        Id = Guid.NewGuid(),
                        BankAccountNumber = new BankAccountNumber("4444", "5555", "3333333333", "02"),
                        CustomerId =Guid.NewGuid()
                    }
                };

                return accounts;

            };

            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();
            

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.FindBankAccounts();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 1);

        }

        [TestMethod()]
        public void FindBankAccountActivitiesReturnNullWhenBankAccountIdIsEmpty()
        {
            //Arrange

            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = new SITypeAdapter();

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.FindBankAccountActivities(Guid.Empty);


            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public void FindBankAccountActivitiesReturnNullWhenBankAccountNotExists()
        {
            //Arrange

            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            bankAccountRepository.GetGuid = (guid) =>
            {
                return null;
            };
            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = new SITypeAdapter();

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.FindBankAccountActivities(Guid.NewGuid());


            //Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public void FindBankAccountActivitiesReturnAllItems()
        {
            //Arrange
            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            bankAccountRepository.GetGuid = (guid) =>
            {
                var bankAccount = new BankAccount()
                {
                    Id = Guid.NewGuid(),
                    BankAccountActivity = new HashSet<BankAccountActivity>()
                    {
                        new BankAccountActivity(){Id = Guid.NewGuid(),Date = DateTime.Now,Amount = 1000},
                        new BankAccountActivity(){Id = Guid.NewGuid(),Date = DateTime.Now,Amount = 1000},
                    }
                };

                return bankAccount;
            };

            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act
            var result = bankingService.FindBankAccountActivities(Guid.NewGuid());


            //Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
        }

        [TestMethod()]
        public void PerformBankTransfer()
        {
            //Arrange
            var sourceId = new Guid("3481009C-A037-49DB-AE05-44FF6DB67DEC");
            var bankAccountNumberSource = new BankAccountNumber("4444", "5555", "3333333333", "02");

            var source = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), bankAccountNumberSource);
            source.Id = sourceId;
            source.DepositMoney(1000,"initial");

            var sourceBankAccountDTO = new BankAccountDTO()
            {
                Id = sourceId,
                BankAccountNumber = source.Iban
            };

            var targetId = new Guid("8A091975-F783-4730-9E03-831E9A9435C1");
            var bankAccountNumberTarget = new BankAccountNumber("1111", "2222", "3333333333", "01");
            var target = BankAccountFactory.CreateBankAccount(Guid.NewGuid(), bankAccountNumberTarget);
            target.Id = targetId;


            var targetBankAccountDTO = new BankAccountDTO()
            {
                Id = targetId,
                BankAccountNumber = target.Iban
            };

            var accounts = new List<BankAccount>() { source, target };
            var accountsDTO = new List<BankAccountDTO>() { sourceBankAccountDTO, targetBankAccountDTO };

            SIBankAccountRepository bankAccountRepository = new SIBankAccountRepository();
            bankAccountRepository.GetGuid = (guid) =>
            {
                return accounts.Where(a => a.Id == guid).SingleOrDefault();
            };
            bankAccountRepository.UnitOfWorkGet = () =>
            {
                var unitOfWork = new SIUnitOfWork();
                unitOfWork.Commit = () => { };

                return unitOfWork;
            };

            SICustomerRepository customerRepository = new SICustomerRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();

            IBankAppService bankingService = new BankAppService(bankAccountRepository, customerRepository, transferService, adapter);

            //Act

            bankingService.PerformBankTransfer(sourceBankAccountDTO, targetBankAccountDTO, 100M);

        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionIfAdapterDependencyIsNull()
        {
            //Arrange
            SICustomerRepository customerRepository = new SICustomerRepository();
            SIBankAccountRepository bankAcccountRepository = new SIBankAccountRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = null;

            //Act
            IBankAppService bankingService = new BankAppService(bankAcccountRepository, customerRepository, transferService, adapter);

        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionIfBankTransferServiceDependencyIsNull()
        {
            //Arrange
            SICustomerRepository customerRepository = new SICustomerRepository();
            SIBankAccountRepository bankAcccountRepository = new SIBankAccountRepository();
            IBankTransferService transferService = null;
            ITypeAdapter adapter = PrepareTypeAdapter();

            //Act
            IBankAppService bankingService = new BankAppService(bankAcccountRepository, customerRepository, transferService, adapter);

        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionIfCustomerRepositoryDependencyIsNull()
        {
            //Arrange
            SICustomerRepository customerRepository = null;
            SIBankAccountRepository bankAcccountRepository = new SIBankAccountRepository();
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();

            //Act
            IBankAppService bankingService = new BankAppService(bankAcccountRepository, customerRepository, transferService, adapter);

        }
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorThrowExceptionIfBankAccountRepositoryDependencyIsNull()
        {
            //Arrange
            SICustomerRepository customerRepository = new SICustomerRepository();
            SIBankAccountRepository bankAcccountRepository = null;
            IBankTransferService transferService = new BankTransferService();
            ITypeAdapter adapter = PrepareTypeAdapter();

            //Act
            IBankAppService bankingService = new BankAppService(bankAcccountRepository, customerRepository, transferService, adapter);

        }

        #region Initialize

        public ITypeAdapter PrepareTypeAdapter()
        {
            return new TypeAdapter(new RegisterTypesMap[]{new BankingModuleRegisterTypesMap()});
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
