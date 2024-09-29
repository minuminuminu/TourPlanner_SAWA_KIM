using iText.Commons.Utils;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.BLL;
using TourPlanner_SAWA_KIM.DAL;
using TourPlanner_SAWA_KIM.Exceptions;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.Tests
{
    [TestFixture]
    public class TourServiceTests
    {
        private Mock<ITourRepository> _tourRepositoryMock;
        private Mock<ITourLogRepository> _tourLogRepositoryMock;
        private Mock<IApiClient> _apiClientMock;
        private ITourService _tourService;

        [SetUp]
        public void Setup()
        {
            _tourRepositoryMock = new Mock<ITourRepository>();
            _tourLogRepositoryMock = new Mock<ITourLogRepository>();
            _apiClientMock = new Mock<IApiClient>();
            _tourService = new TourService(_tourRepositoryMock.Object, _tourLogRepositoryMock.Object, _apiClientMock.Object);
        }

        [Test]
        public async Task GetTourByIdAsync_ReturnsTour_WhenTourExists()
        {
            int tourId = 1;
            var expectedTour = new Tour("Sample Tour", "Description", "From", "To", "TransportType")
            {
                Id = tourId
            };
            _tourRepositoryMock.Setup(repo => repo.GetTourByIdAsync(tourId)).ReturnsAsync(expectedTour);

            var result = await _tourService.GetTourByIdAsync(tourId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedTour, result);
        }


        [Test]
        public void GetTourByIdAsync_ThrowsArgumentException_WhenTourDoesNotExist()
        {
            int tourId = 1;
            _tourRepositoryMock.Setup(repo => repo.GetTourByIdAsync(tourId)).ReturnsAsync((Tour)null);

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _tourService.GetTourByIdAsync(tourId));
            Assert.AreEqual($"Tour with ID {tourId} not found", ex.Message);
        }

        [Test]
        public void AddTourAndGeoCodesAsync_ThrowsFailedToImportTourException_WhenTourIsNull()
        {
            Tour tour = null;

            var ex = Assert.ThrowsAsync<FailedToImportTourException>(async () => await _tourService.AddTourAndGeoCodesAsync(tour));
            Assert.AreEqual("Tour is null", ex.Message);
        }
    }
}
