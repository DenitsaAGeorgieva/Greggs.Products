using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Greggs.Products.UnitTests
{
    public class UnitTest1
    {
        private readonly Mock<IDataAccess> _mockDataAccess;
        private readonly Mock<ILogger<ProductController>> _mockLogger;
        private readonly ProductController _controller;
        private const decimal ConversionRate = 1.11m;

        public UnitTest1()
        {
            _mockDataAccess = new Mock<IDataAccess>();
            _mockLogger = new Mock<ILogger<ProductController>>();
            _controller = new ProductController(_mockLogger.Object, _mockDataAccess.Object);
        }

        [Fact]
        public async Task Get_ReturnsProductsWithPricesInEuros()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Name = "Product1", PriceInPounds = 10m },
                new Product { Name = "Product2", PriceInPounds = 20m }
            };

            _mockDataAccess.Setup(m => m.List(It.IsAny<int>(), It.IsAny<int>())).Returns(products);

            // Act
            var result = await _controller.Get();

            // Assert
            var resultList = result.ToList();
            Assert.Equal(2, resultList.Count);
            Assert.Equal(10m * ConversionRate, resultList[0].PriceInEuro);
            Assert.Equal(20m * ConversionRate, resultList[1].PriceInEuro);
        }
    }
}
