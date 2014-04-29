

namespace Application.MainBoundedContext.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Samples.NLayerApp.Domain.MainBoundedContext.ERPModule.Aggregates.ProductAgg;
    using Microsoft.Samples.NLayerApp.Domain.Seedwork;
    using Microsoft.Samples.NLayerApp.Infrastructure.Crosscutting.Adapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOAdapters;
    using Microsoft.Samples.NLayerApp.Application.MainBoundedContext.ERPModule.DTOs;

    [TestClass()]
    public class ProductAdapterTests
    {
        [TestMethod()]
        public void ProductToProductDTOAdapter()
        {
            //Arrange
            Product product = new Software()
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                Title ="the title",
                UnitPrice = 10,
                Description = "The description",
                AmountInStock = 10
            };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var productDTO = adapter.Adapt<Product, ProductDTO>(product);

            //Assert
            Assert.AreEqual(product.Id, productDTO.Id);
            Assert.AreEqual(product.Title, productDTO.Title);
            Assert.AreEqual(product.Description, productDTO.Description);
            Assert.AreEqual(product.AmountInStock, productDTO.AmountInStock);
            Assert.AreEqual(product.UnitPrice, productDTO.UnitPrice);
        }

        [TestMethod()]
        public void EnumerableProductToListProductDTOAdapter()
        {
            //Arrange
            var products = new List<Software>()
            {
                new Software()
                {
                    Id = IdentityGenerator.NewSequentialGuid(),
                    Title = "the title",
                    UnitPrice = 10,
                    Description = "The description",
                    AmountInStock = 10
                }
            };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var productsDTO = adapter.Adapt<IEnumerable<Product>, List<ProductDTO>>(products);

            //Assert
            Assert.AreEqual(products[0].Id, productsDTO[0].Id);
            Assert.AreEqual(products[0].Title, productsDTO[0].Title);
            Assert.AreEqual(products[0].Description, productsDTO[0].Description);
            Assert.AreEqual(products[0].AmountInStock, productsDTO[0].AmountInStock);
            Assert.AreEqual(products[0].UnitPrice, productsDTO[0].UnitPrice);
        }

        [TestMethod()]
        public void SoftwareToSoftwareDTOAdapter()
        {
            //Arrange
            Software software = new Software()
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                Title = "the title",
                UnitPrice = 10,
                Description = "The description",
                AmountInStock = 10,
                LicenseCode = "AB001"
            };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var softwareDTO = adapter.Adapt<Software, SoftwareDTO>(software);

            //Assert
            Assert.AreEqual(software.Id, softwareDTO.Id);
            Assert.AreEqual(software.Title, softwareDTO.Title);
            Assert.AreEqual(software.Description, softwareDTO.Description);
            Assert.AreEqual(software.AmountInStock, softwareDTO.AmountInStock);
            Assert.AreEqual(software.UnitPrice, softwareDTO.UnitPrice);
            Assert.AreEqual(software.LicenseCode, softwareDTO.LicenseCode);
        }

        [TestMethod()]
        public void EnumerableSoftwareToListSoftwareDTOAdapter()
        {
            //Arrange
            var softwares = new List<Software>()
            {
                new Software()
                {
                    Id = IdentityGenerator.NewSequentialGuid(),
                    Title = "the title",
                    UnitPrice = 10,
                    Description = "The description",
                    AmountInStock = 10,
                    LicenseCode = "AB001"
                }
            };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var softwaresDTO = adapter.Adapt<IEnumerable<Software>, List<SoftwareDTO>>(softwares);

            //Assert
            Assert.AreEqual(softwares[0].Id, softwaresDTO[0].Id);
            Assert.AreEqual(softwares[0].Title, softwaresDTO[0].Title);
            Assert.AreEqual(softwares[0].Description, softwaresDTO[0].Description);
            Assert.AreEqual(softwares[0].AmountInStock, softwaresDTO[0].AmountInStock);
            Assert.AreEqual(softwares[0].UnitPrice, softwaresDTO[0].UnitPrice);
            Assert.AreEqual(softwares[0].LicenseCode, softwaresDTO[0].LicenseCode);
        }

        [TestMethod()]
        public void BookToBookDTOAdapter()
        {
            //Arrange
            var book = new Book()
            {
                Id = IdentityGenerator.NewSequentialGuid(),
                Title = "the title",
                UnitPrice = 10,
                Description = "The description",
                AmountInStock = 10,
                ISBN ="ABD12",
                Publisher= "Krasis Press"
            };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var bookDTO = adapter.Adapt<Book, BookDTO>(book);

            //Assert
            Assert.AreEqual(book.Id, bookDTO.Id);
            Assert.AreEqual(book.Title, bookDTO.Title);
            Assert.AreEqual(book.Description, bookDTO.Description);
            Assert.AreEqual(book.AmountInStock, bookDTO.AmountInStock);
            Assert.AreEqual(book.UnitPrice, bookDTO.UnitPrice);
            Assert.AreEqual(book.ISBN, bookDTO.ISBN);
            Assert.AreEqual(book.Publisher, bookDTO.Publisher);
        }

        [TestMethod()]
        public void EnumerableBookToListBookDTOAdapter()
        {
            //Arrange
            var books = new List<Book>()
            {
                new Book()
                {
                    Id = IdentityGenerator.NewSequentialGuid(),
                    Title = "the title",
                    UnitPrice = 10,
                    Description = "The description",
                    AmountInStock = 10,
                    ISBN = "ABD12",
                    Publisher = "Krasis Press"
                }
            };

            //Act
            ITypeAdapter adapter = PrepareTypeAdapter();
            var booksDTO = adapter.Adapt<IEnumerable<Book>, List<BookDTO>>(books);

            //Assert
            Assert.AreEqual(books[0].Id, booksDTO[0].Id);
            Assert.AreEqual(books[0].Title, booksDTO[0].Title);
            Assert.AreEqual(books[0].Description, booksDTO[0].Description);
            Assert.AreEqual(books[0].AmountInStock, booksDTO[0].AmountInStock);
            Assert.AreEqual(books[0].UnitPrice, booksDTO[0].UnitPrice);
            Assert.AreEqual(books[0].ISBN, booksDTO[0].ISBN);
            Assert.AreEqual(books[0].Publisher, booksDTO[0].Publisher);
        }

        ITypeAdapter PrepareTypeAdapter()
        {
            TypeAdapter adapter = new TypeAdapter(new RegisterTypesMap[] { new ERPModuleRegisterTypesMap() });

            return adapter;
        }
    }
}
