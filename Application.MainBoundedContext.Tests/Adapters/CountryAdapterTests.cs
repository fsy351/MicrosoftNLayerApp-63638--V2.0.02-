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

    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.CountryAgg;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class CountryAdapterTests
    {
        [TestMethod]
        public void CountryToCountryDTOAdapter()
        {
            //Arrange
            Guid idCountry = IdentityGenerator.NewSequentialGuid();

            Country country = new Country()
            {
                Id = idCountry,
                CountryName = "Spain",
                CountryISOCode = "es-ES"
            };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var dto = adapter.Adapt<Country,CountryDTO>(country);

            //Assert
            Assert.AreEqual(idCountry, dto.Id);
            Assert.AreEqual(country.CountryName, dto.CountryName);
            Assert.AreEqual(country.CountryISOCode, dto.CountryISOCode);
        }
        [TestMethod]
        public void CountryEnumerableToCountryDTOList()
        {
            //Arrange
            Guid idCountry = IdentityGenerator.NewSequentialGuid();

            Country country = new Country()
            {
                Id = idCountry,
                CountryName = "Spain",
                CountryISOCode = "es-ES"
            };

            IEnumerable<Country> countries = new List<Country>() { country };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var dtos = adapter.Adapt<IEnumerable<Country>, List<CountryDTO>>(countries);

            //Assert
            Assert.IsNotNull(dtos);
            Assert.IsTrue(dtos.Any());
            Assert.IsTrue(dtos.Count == 1);

            var dto = dtos[0];

            Assert.AreEqual(idCountry, dto.Id);
            Assert.AreEqual(country.CountryName, dto.CountryName);
            Assert.AreEqual(country.CountryISOCode, dto.CountryISOCode);
        }

        ITypeAdapter PrepareTypeAdapter()
        {
            TypeAdapter adapter = new TypeAdapter(new RegisterTypesMap[] { new ERPModuleRegisterTypesMap() });

            return adapter;
        }
    }
}
