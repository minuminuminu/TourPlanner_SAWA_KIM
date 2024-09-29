using NUnit.Framework;
using Moq;
using System.Windows;
using System;
using TourPlanner_SAWA_KIM.ViewModels;
using TourPlanner_SAWA_KIM.BLL;
using TourPlanner_SAWA_KIM.Mediators;
using System.Collections.ObjectModel;
using TourPlanner_SAWA_KIM.Models;
using TourPlanner_SAWA_KIM.Views.Windows;
using System.Text.Json;

namespace TourPlanner_SAWA_KIM.Tests
{
    [TestFixture]
    public class SearchBarViewModelTests
    {
        private SearchBarViewModel _viewModel;
        private Mock<IMediator> _mediatorMock;

        [SetUp]
        public void Setup()
        {
            _viewModel = new SearchBarViewModel();
            _mediatorMock = new Mock<IMediator>();
            _viewModel.SetMediator(_mediatorMock.Object);
        }

        [Test]
        public void SearchCommand_NotifiesMediator()
        {
            _viewModel.SearchCommand.Execute(null);

            _mediatorMock.Verify(m => m.Notify(_viewModel, "Search"), Times.Once);
        }
    }

    [TestFixture]
    public class ToursListViewModelTests
    {
        private ToursListViewModel _viewModel;
        private Mock<ITourService> _tourServiceMock;

        [SetUp]
        public void Setup()
        {
            _tourServiceMock = new Mock<ITourService>();
            _viewModel = new ToursListViewModel(_tourServiceMock.Object)
            {
                Tours = new ObservableCollection<Tour>()
            };
        }

        [Test]
        public void AddTour_AddsTourToCollection()
        {
            var tour = new Tour("New Tour", "Description", "From", "To", "Car");

            _viewModel.AddTour(tour);

            Assert.Contains(tour, _viewModel.Tours);
        }
    }

    [TestFixture]
    public class ToursListViewModelPropertyChangedTests
    {
        private ToursListViewModel _viewModel;
        private Mock<ITourService> _tourServiceMock;

        [SetUp]
        public void Setup()
        {
            _tourServiceMock = new Mock<ITourService>();
            _viewModel = new ToursListViewModel(_tourServiceMock.Object)
            {
                Tours = new ObservableCollection<Tour>()
            };
        }

        [Test]
        public void SettingSelectedTour_RaisesPropertyChangedEvent()
        {
            var tour = new Tour("Test Tour", "Description", "From", "To", "TransportType");
            bool eventRaised = false;
            _viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_viewModel.CurrentTour))
                {
                    eventRaised = true;
                }
            };

            _viewModel.CurrentTour = tour;

            Assert.IsTrue(eventRaised);
        }
    }


    [TestFixture]
    public class ToursListViewModelFilterTests
    {
        private ToursListViewModel _viewModel;
        private Mock<ITourService> _tourServiceMock;
        private List<Tour> _allTours;

        [SetUp]
        public void Setup()
        {
            _tourServiceMock = new Mock<ITourService>();
            _allTours = new List<Tour>
            {
                new Tour("Mountain Adventure", "Exciting mountain tour", "A", "B", "Bike"),
                new Tour("Beach Escape", "Relaxing beach tour", "C", "D", "Walk"),
                new Tour("City Exploration", "Discover the city", "E", "F", "Car")
            };
            _tourServiceMock.Setup(s => s.GetAllToursAsync()).ReturnsAsync(_allTours);
            _viewModel = new ToursListViewModel(_tourServiceMock.Object)
            {
                Tours = new ObservableCollection<Tour>(_allTours)
            };
        }

        [Test]
        public void FilterTours_FiltersBasedOnSearchText()
        {
            _viewModel.FilterTours("Mountain");

            Assert.AreEqual(1, _viewModel.Tours.Count);
            Assert.AreEqual("Mountain Adventure", _viewModel.Tours[0].Name);
        }

        [Test]
        public void FilterTours_NoSearchText_ShowsAllTours()
        {
            _viewModel.FilterTours("");

            Assert.AreEqual(3, _viewModel.Tours.Count);
        }

        [Test]
        public void FilterTours_NoMatches_ShowsEmptyCollection()
        {
            _viewModel.FilterTours("Desert");

            Assert.AreEqual(0, _viewModel.Tours.Count);
        }

        [Test]
        public void FilterTours_IsCaseInsensitive()
        {
            _viewModel.FilterTours("mountain");

            Assert.AreEqual(1, _viewModel.Tours.Count);
            Assert.AreEqual("Mountain Adventure", _viewModel.Tours[0].Name);
        }
    }

    [TestFixture]
    public class ToursLogsViewModelTests
    {
        private ToursLogsViewModel _viewModel;
        private Mock<ITourService> _tourServiceMock;
        private Mock<IMediator> _mediatorMock;
        private Tour _selectedTour;

        [SetUp]
        public void Setup()
        {
            _tourServiceMock = new Mock<ITourService>();
            _mediatorMock = new Mock<IMediator>();
            _viewModel = new ToursLogsViewModel(_tourServiceMock.Object);
            _viewModel.SetMediator(_mediatorMock.Object);

            if (Application.Current == null)
            {
                new Application();
            }
        }

        [Test]
        public async Task LoadTourLogsAsync_LoadsLogsForGivenTourId()
        {
            int tourId = 1;
            var tourLogs = new List<TourLog>
            {
                new TourLog(5, DateTime.Now, TimeSpan.FromHours(2), 10, "Easy", "Great tour") { Id = 1, TourId = tourId },
                new TourLog(4, DateTime.Now, TimeSpan.FromHours(1.5), 8, "Medium", "Good tour") { Id = 2, TourId = tourId }
            };
            _tourServiceMock.Setup(s => s.GetTourLogsByTourIdAsync(tourId)).ReturnsAsync(tourLogs);

            await _viewModel.LoadTourLogsAsync(tourId);

            Assert.AreEqual(2, _viewModel.TourLogs.Count);
            Assert.AreEqual(tourLogs[0], _viewModel.TourLogs[0]);
            Assert.AreEqual(tourLogs[1], _viewModel.TourLogs[1]);
            _mediatorMock.Verify(m => m.Notify(_viewModel, "TourLogsLoaded"), Times.Once);
        }

        [Test]
        public void ClearTourLogs_ClearsTourLogsCollection()
        {
            _viewModel.TourLogs.Add(new TourLog(5, DateTime.Now, TimeSpan.FromHours(2), 10, "Easy", "Great tour"));

            _viewModel.ClearTourLogs();

            Assert.IsEmpty(_viewModel.TourLogs);
        }

        [Test]
        public void SetSelectedTour_SetsSelectedTour()
        {
            var tour = new Tour("Test Tour", "Description", "From", "To", "TransportType") { Id = 1 };

            _viewModel.SetSelectedTour(tour);

            Assert.Pass("SetSelectedTour executed without exceptions.");
        }
    }


    [TestFixture]
    public class ToursOverviewViewModelTests
    {
        private ToursOverviewViewModel _viewModel;
        private Mock<ITourService> _tourServiceMock;

        [SetUp]
        public void Setup()
        {
            _tourServiceMock = new Mock<ITourService>();
            _viewModel = new ToursOverviewViewModel(_tourServiceMock.Object);
        }

        [Test]
        public void ComputeAttributes_CalculatesPopularityCorrectly()
        {
            var tourLogs = new ObservableCollection<TourLog>
            {
                new TourLog(5, DateTime.Now, TimeSpan.FromHours(2), 10, "Easy", "Good"),
                new TourLog(4, DateTime.Now, TimeSpan.FromHours(1.5), 8, "Medium", "Nice"),
                new TourLog(3, DateTime.Now, TimeSpan.FromHours(2.5), 12, "Hard", "Challenging")
            };

            _viewModel.ComputeAttributes(tourLogs);

            Assert.AreEqual(2, _viewModel.AttributePopularity); // 3 logs should map to popularity 2
        }

        [Test]
        public void ComputeAttributes_CalculatesChildFriendlinessCorrectly()
        {
            var tourLogs = new ObservableCollection<TourLog>
            {
                new TourLog(5, DateTime.Now, TimeSpan.FromHours(1.0), 5, "Easy", "Good"),
                new TourLog(4, DateTime.Now, TimeSpan.FromHours(1.0), 6, "Easy", "Nice"),
                new TourLog(3, DateTime.Now, TimeSpan.FromHours(1.0), 4, "Easy", "Fun")
            };


            _viewModel.ComputeAttributes(tourLogs);

            Assert.AreEqual(5, _viewModel.ChildFriendliness);
            Assert.AreEqual("Very child friendly", _viewModel.AttributeChildFriendliness);
        }

        [Test]
        public void UpdateTourDetails_SetsSelectedTour()
        {
            var tour = new Tour("Sample Tour", "Description", "From", "To", "TransportType") { Id = 1 };

            _viewModel.UpdateTourDetails(tour);

            Assert.AreEqual(tour, _viewModel.SelectedTour);
        }

        [Test]
        public void ClearTourDetails_SetsSelectedTourToNull()
        {
            var tour = new Tour("Sample Tour", "Description", "From", "To", "TransportType") { Id = 1 };
            _viewModel.SelectedTour = tour;

            _viewModel.ClearTourDetails();

            Assert.IsNull(_viewModel.SelectedTour);
        }
    }

    [TestFixture]
    public class TourWindowViewModelTests
    {
        private TourWindowViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new TourWindowViewModel();
            if (Application.Current == null)
            {
                new Application();
            }
        }

        [Test]
        public void Confirm_ClosesWindow_WhenFieldsAreValid()
        {
            _viewModel.Name = "Tour Name";
            _viewModel.Description = "Description";
            _viewModel.From = "Location A";
            _viewModel.To = "Location B";
            _viewModel.TransportType = "driving-car";

            bool windowClosed = false;
            _viewModel.CloseAction = () => windowClosed = true;

            _viewModel.ConfirmCommand.Execute(null);

            Assert.IsTrue(windowClosed);
        }

        [Test]
        public void Constructor_InitializesProperties_WhenExistingTourIsProvided()
        {
            var existingTour = new Tour("Existing Tour", "Existing Description", "From Location", "To Location", "cycling-regular");

            var viewModel = new TourWindowViewModel(existingTour);

            Assert.AreEqual("Existing Tour", viewModel.Name);
            Assert.AreEqual("Existing Description", viewModel.Description);
            Assert.AreEqual("From Location", viewModel.From);
            Assert.AreEqual("To Location", viewModel.To);
            Assert.AreEqual("cycling-regular", viewModel.TransportType);
        }
    }

    [TestFixture]
    public class TourLogWindowViewModelTests
    {
        private TourLogWindowViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _viewModel = new TourLogWindowViewModel();

            if (Application.Current == null)
            {
                new Application();
            }
        }

        [Test]
        public void Confirm_ClosesWindow_WhenFieldsAreValid()
        {
            _viewModel.Rating = 5;
            _viewModel.Date = DateTime.Now;
            _viewModel.Duration = TimeSpan.FromHours(2);
            _viewModel.Distance = 10;
            _viewModel.Difficulty = "Medium";
            _viewModel.Comment = "Great experience";

            bool windowClosed = false;
            _viewModel.CloseAction = () => windowClosed = true;

            _viewModel.ConfirmCommand.Execute(null);

            Assert.IsTrue(windowClosed);
        }
    }
}
