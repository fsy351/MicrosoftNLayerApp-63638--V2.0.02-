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
			
namespace Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOAdapters
{
    using System.Collections.Generic;

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOAdapters.Maps;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.BankingModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.BankingModule.Aggregates.BankAccountAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    /// <summary>
    /// Register types maps
    /// </summary>
    public class BankingModuleRegisterTypesMap
        :RegisterTypesMap
    {
        #region Constructor

        /// <summary>
        /// Create a new instace of BankingModuleRegisterTypesMap.
        /// This class is not inteneded to be used directly. The TypeAdapter
        /// infrastructure load automatically and recover information of mapping
        /// </summary>
        public BankingModuleRegisterTypesMap()
        {
            SetMaps();
        }

        #endregion

        #region Set Maps 

        void SetMaps()
        {
            RegisterMap<BankAccountActivity, BankActivityDTO>(new BankActivityToBankActivityDTOMap());
            RegisterMap<IEnumerable<BankAccountActivity>, List<BankActivityDTO>>(new BankActivityEnumerableToBankActivityDTOListMap());

            RegisterMap<BankAccount, BankAccountDTO>(new BankAccountToBankAccountDTOMap());
            RegisterMap<IEnumerable<BankAccount>, List<BankAccountDTO>>(new BankAccountEnumerableToBankAccountDTOListMap());
        }

        #endregion
    }
}
