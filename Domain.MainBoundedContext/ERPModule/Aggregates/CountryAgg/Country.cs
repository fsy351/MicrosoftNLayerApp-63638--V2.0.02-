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


namespace Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.Resources;

    /// <summary>
    /// The country entity
    /// </summary>
    public class Country
        :Entity, IValidatableObject
    {
        #region Properties

        /// <summary>
        /// Get or set the Country Name
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Get or set the Country ISO Code
        /// </summary>
        public string CountryISOCode { get; set; }

        #endregion

        #region IValidatableObject Members

        /// <summary>
        /// <see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/>
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();

            //-->Check country name
            if (String.IsNullOrEmpty(this.CountryName)
               ||
               String.IsNullOrWhiteSpace(this.CountryName))
            {
                validationResults.Add(new ValidationResult(Messages.validation_CountryCountryNameCannotBeNull,
                                                           new string[] { "CountryName" }));
            }

            //-->Check Country ISOCode

            if (String.IsNullOrEmpty(this.CountryISOCode)
               ||
               String.IsNullOrWhiteSpace(this.CountryISOCode))
            {
                validationResults.Add(new ValidationResult(Messages.validation_CountryCountryISOCodeCannotBeNull,
                                                           new string[] { "CountryISOCode" }));
            }

            return validationResults;
        }

        #endregion
    }
}
