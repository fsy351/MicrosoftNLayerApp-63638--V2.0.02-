//===================================================================================
// Microsoft Developer and Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is released under the terms of the MS-LPL license, 
// http://microsoftnlayerapp.codeplex.com/license
//===================================================================================

namespace Microsoft.Samples.NLayerApp.Infrastructure.Data.MainBoundedContext.UnitOfWork
{

    using System.Data.Entity;

    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.OrderAgg;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;

    using Microsoft.Samples.NLayerApp.Infrastructure.Data.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CustomerAgg;

    /// <summary>
    /// This is a contract for 'Main Bounded-Context' Unit Of Work,
    /// you can use this contract for implementing the real
    /// dependency to your O/RM or, for creating a mock... 
    /// Also, setting this abstraction adds more information about 
    /// the existent sets in non generic repository methods.
    /// 
    /// This is not the contract for switching  
    /// your persistent infrastructure, of course....
    /// </summary>
    public interface IMainBCUnitOfWork
        :IQueryableUnitOfWork
    {
        IDbSet<Customer> Customers { get; }

        IDbSet<Product> Products { get; }

        IDbSet<Order> Orders { get; }

        IDbSet<Country> Countries { get; }

        IDbSet<BankAccount> BankAccounts { get; }
    }
}
