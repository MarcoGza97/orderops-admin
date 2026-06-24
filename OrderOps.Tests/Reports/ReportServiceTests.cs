using Moq;
using OrderOps.Application.Reports.Dtos;
using OrderOps.Application.Reports.Interfaces;
using OrderOps.Application.Reports.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Tests.Reports
{
    [TestFixture]
    public sealed class ReportServiceTests
    {
        private Mock<IReportReadRepository> _reportReadRepository = null!;
        private ReportService _reportService = null!;

        [SetUp]
        public void SetUp()
        {
            _reportReadRepository = new Mock<IReportReadRepository>();

            _reportService = new ReportService(
                _reportReadRepository.Object);
        }

        [Test]
        public async Task GetTopProductsAsync_ShouldReturnTopProducts()
        {
            var expected = new List<TopProductReportDto>
        {
            new(
                ProductId: 1,
                ProductName: "Keyboard",
                QuantitySold: 10,
                Revenue: 5000)
        };

            _reportReadRepository
                .Setup(repository => repository.GetTopProductsAsync())
                .ReturnsAsync(expected);

            var result = await _reportService.GetTopProductsAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].ProductId, Is.EqualTo(1));
            Assert.That(result[0].ProductName, Is.EqualTo("Keyboard"));
            Assert.That(result[0].QuantitySold, Is.EqualTo(10));
            Assert.That(result[0].Revenue, Is.EqualTo(5000));
        }

        [Test]
        public async Task GetSalesByCustomerAsync_ShouldReturnSalesByCustomer()
        {
            var expected = new List<SalesByCustomerReportDto>
        {
            new(
                CustomerId: 1,
                CustomerName: "ACME Corp",
                OrdersCount: 3,
                Revenue: 12000)
        };

            _reportReadRepository
                .Setup(repository => repository.GetSalesByCustomerAsync())
                .ReturnsAsync(expected);

            var result = await _reportService.GetSalesByCustomerAsync();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].CustomerId, Is.EqualTo(1));
            Assert.That(result[0].CustomerName, Is.EqualTo("ACME Corp"));
            Assert.That(result[0].OrdersCount, Is.EqualTo(3));
            Assert.That(result[0].Revenue, Is.EqualTo(12000));
        }

        [Test]
        public async Task GetLowStockProductsAsync_WhenThresholdIsProvided_ShouldUseThreshold()
        {
            var expected = new List<LowStockProductDto>
        {
            new(
                ProductId: 1,
                ProductName: "Mouse",
                Stock: 2)
        };

            _reportReadRepository
                .Setup(repository => repository.GetLowStockProductsAsync(3))
                .ReturnsAsync(expected);

            var result = await _reportService.GetLowStockProductsAsync(3);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].ProductId, Is.EqualTo(1));
            Assert.That(result[0].ProductName, Is.EqualTo("Mouse"));
            Assert.That(result[0].Stock, Is.EqualTo(2));

            _reportReadRepository.Verify(
                repository => repository.GetLowStockProductsAsync(3),
                Times.Once);
        }

        [Test]
        public async Task GetLowStockProductsAsync_WhenThresholdIsNotProvided_ShouldUseDefaultThreshold()
        {
            _reportReadRepository
                .Setup(repository => repository.GetLowStockProductsAsync(5))
                .ReturnsAsync([]);

            await _reportService.GetLowStockProductsAsync();

            _reportReadRepository.Verify(
                repository => repository.GetLowStockProductsAsync(5),
                Times.Once);
        }
    }
}
