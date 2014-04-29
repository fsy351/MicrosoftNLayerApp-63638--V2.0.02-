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


namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Transactions;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOs;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.Resources;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Services;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Logging;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Validator;
    using Microsoft.Samples.NLayerApp.Application.Seedwork;

    /// <summary>
    /// The bank management service implementation
    /// </summary>
    public class BankAppService
        : IBankAppService
    {
        #region Members

        IBankAccountRepository _bankAccountRepository;
        ICustomerRepository _customerRepository;
        IBankTransferService _transferService;
        ITypeAdapter _adapter;

        #endregion

        #region Constructor

        /// <summary>
        /// Create a new instance 
        /// </summary>
        public BankAppService(IBankAccountRepository bankAccountRepository, // the bank account repository dependency
                                     ICustomerRepository customerRepository, // the customer repository dependency
                                     IBankTransferService transferService, // bank transfer domain services
                                     ITypeAdapter adapter) // the entities - dto adapters
        {
            //check preconditions
            if (bankAccountRepository == null)
                throw new ArgumentNullException("bankAccountRepository");

            if (customerRepository == null)
                throw new ArgumentNullException("customerRepository");

            if (adapter == null)
                throw new ArgumentNullException("adapter");

            if (transferService == null)
                throw new ArgumentNullException("trasferService");

            _bankAccountRepository = bankAccountRepository;
            _customerRepository = customerRepository;
            _adapter = adapter;
            _transferService = transferService;
        }

        #endregion

        #region IBankAppService Members

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/>
        /// </summary>
        /// <param name="bankAccountDTO"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/></returns>
        public BankAccountDTO AddBankAccount(BankAccountDTO bankAccountDTO)
        {
            if (bankAccountDTO != null && bankAccountDTO.CustomerId != Guid.Empty)
            {
                //check if exists the customer for this bank account
                var associatedCustomer = _customerRepository.Get(bankAccountDTO.CustomerId);

                if (associatedCustomer != null) // if the customer exist
                {
                    //Create a new bank account  number
                    var accountNumber = CalculateNewBankAccountNumber();

                    //Create account from factory and set persistent id
                    var account = BankAccountFactory.CreateBankAccount(associatedCustomer, accountNumber);

                    //set the poid
                    account.Id = IdentityGenerator.NewSequentialGuid();

                    //save bank account
                    SaveBankAccount(account);

                    //return dto
                    return _adapter.Adapt<BankAccount, BankAccountDTO>(account);
                }
                else //the customer for this bank account not exist, cannot create a new bank account
                {
                    LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotCreateBankAccountForNonExistingCustomer);
                    return null;
                }
            }
            else // if bank account is null or customer identifier  is empty, cannot create a new bank account
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotAddNullBankAccountOrInvalidCustomer);
                return null;
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/>
        /// </summary>
        /// <param name="bankAccountId"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/></returns>
        public bool LockBankAccount(Guid bankAccountId)
        {
            if (bankAccountId == Guid.Empty) // if identifier is empty return false
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotLockBankAccountWithEmptyIdentifier);
                return false;
            }

            //recover bank account, lock and commit changes
            var unitOfWork = _bankAccountRepository.UnitOfWork;
            var bankAccount = _bankAccountRepository.Get(bankAccountId);

            if (bankAccount != null)
            {
                bankAccount.Lock();

                unitOfWork.Commit();

                return true;
            }
            else // if not exist the bank account return false
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotLockNonExistingBankAccount, bankAccountId);
                return false;
            }
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/>
        /// </summary>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/></returns>
        public List<BankAccountDTO> FindBankAccounts()
        {
            var bankAccounts = _bankAccountRepository.GetAll();

            if (bankAccounts != null
                &&
                bankAccounts.Any())
            {
                return _adapter.Adapt<IEnumerable<BankAccount>, List<BankAccountDTO>>(bankAccounts);
            }
            else // no results
                return null;
        }

        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/>
        /// </summary>
        /// <param name="fromAccount"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/></param>
        /// <param name="toAccount"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/></param>
        /// <param name="amount"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/></param>
        public void PerformBankTransfer(BankAccountDTO fromAccount, BankAccountDTO toAccount, decimal amount)
        {
            //Application-Logic Process: 
            // 1º Get Accounts objects from Repositories
            // 2º Start Transaction
            // 3º Call PerformTransfer method in Domain Service
            // 4º If no exceptions, commit the unit of work and complete transaction

            if (BankAccountHasIdentity(fromAccount)
                &&
                BankAccountHasIdentity(toAccount))
            {
                //get the current unit of work
                var unitOfWork = _bankAccountRepository.UnitOfWork;

                var source = _bankAccountRepository.Get(fromAccount.Id);
                var target = _bankAccountRepository.Get(toAccount.Id);

                if (source != null & target != null) // if all accounts exist
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        //perform transfer
                        _transferService.PerformTransfer(amount, source, target);

                        //comit unit of work
                        unitOfWork.Commit();

                        //complete transaction
                        scope.Complete();
                    }
                }
                else
                    LoggerFactory.CreateLog().LogError(Messages.error_CannotPerformTransferInvalidAccounts);
            }
            else
                LoggerFactory.CreateLog().LogError(Messages.error_CannotPerformTransferInvalidAccounts);

        }
        /// <summary>
        /// <see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.Services.IBankAppService"/>
        /// </summary>
        /// <param name="bankAccountId"><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.IBankManagementService"/></param>
        /// <returns><see cref="Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.Services.IBankManagementService"/></returns>
        public List<BankActivityDTO> FindBankAccountActivities(Guid bankAccountId)
        {
            if (bankAccountId != Guid.Empty)
            {
                var account = _bankAccountRepository.Get(bankAccountId);

                if (account != null)
                {
                    return _adapter.Adapt<IEnumerable<BankAccountActivity>, List<BankActivityDTO>>(account.BankAccountActivity);
                }
                else // the bank account not exist
                {
                    LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotGetActivitiesForInvalidOrNotExistingBankAccount);
                    return null;
                }
            }
            else
            {
                LoggerFactory.CreateLog().LogWarning(Messages.warning_CannotGetActivitiesForInvalidOrNotExistingBankAccount);
                return null;
            }
        }

        #endregion

        #region Private Methods

        void SaveBankAccount(BankAccount bankAccount)
        {
            //validate bank account
            var validator = EntityValidatorFactory.CreateValidator();

            if (validator.IsValid<BankAccount>(bankAccount)) // save entity
            {
                _bankAccountRepository.Add(bankAccount);
                _bankAccountRepository.UnitOfWork.Commit();
            }
            else //throw validation errors
                throw new ApplicationValidationErrorsException(validator.GetInvalidMessages(bankAccount));
        }

        BankAccountNumber CalculateNewBankAccountNumber()
        {
            var bankAccountNumber = new BankAccountNumber();

            //simulate bank account number creation....

            bankAccountNumber.OfficeNumber = "2354";
            bankAccountNumber.NationalBankCode = "2134";
            bankAccountNumber.CheckDigits = "02";
            bankAccountNumber.AccountNumber = new Random().Next(1, Int32.MaxValue).ToString();

            return bankAccountNumber;

        }

        bool BankAccountHasIdentity(BankAccountDTO bankAccountDTO)
        {
            //return true is bank account dto has identity
            return (bankAccountDTO != null && bankAccountDTO.Id != Guid.Empty);
        }

        #endregion
    }
}
