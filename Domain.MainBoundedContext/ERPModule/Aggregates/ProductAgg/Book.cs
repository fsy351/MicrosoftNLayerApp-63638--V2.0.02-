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


namespace Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg
{
    /// <summary>
    /// The book product
    /// </summary>
    public class Book
        : Product
    {
        #region Properties

        /// <summary>
        /// Get or set the publisher of this book
        /// </summary>
        public string Publisher { get; set; }

        /// <summary>
        /// Get or set related ISBN
        /// </summary>
        public string ISBN { get; set; }

        #endregion
    }
}
