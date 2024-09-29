using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner_SAWA_KIM.BLL;
using TourPlanner_SAWA_KIM.Exceptions;
using TourPlanner_SAWA_KIM.Logging;
using TourPlanner_SAWA_KIM.Mediators;
using TourPlanner_SAWA_KIM.Models;
using TourPlanner_SAWA_KIM.Views.Windows;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class ToursListViewModel : ViewModelBase
    {
        private Tour _currentTour;
        private IMediator _mediator;
        private readonly ITourService _tourService;
        private ILoggerWrapper logger = LoggerFactory.GetLogger();
        private List<Tour> _allTours;

        public ObservableCollection<Tour> Tours { get; set; }

        public ICommand AddTourCommand { get; }
        public ICommand RemoveTourCommand { get; }
        public ICommand ModifyTourCommand { get; }

        public Tour CurrentTour
        {
            get { return _currentTour; }
            set
            {
                if (_currentTour != value)
                {
                    _currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                    _mediator?.Notify(this, "TourSelected"); // raise "TourSelected" event and pass tour as param"
                }
            }
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ToursListViewModel(ITourService tourService)
        {
            Tours = new ObservableCollection<Tour>();
            _allTours = new List<Tour>();

            AddTourCommand = new RelayCommand(async () => await AddTour());
            RemoveTourCommand = new RelayCommand(async () => await RemoveTour());
            ModifyTourCommand = new RelayCommand(async () => await ModifyTour());

            _tourService = tourService;

            LoadTours(); 
        }

        public void AddTour(Tour tour)
        {
            Tours.Add(tour);
        }

        public void FilterTours(string searchText)
        {
            Tours.Clear();
            if (string.IsNullOrEmpty(searchText))
            {
                LoadTours();
            }
            else
            {
                var filteredTours = _allTours.Where(tour =>
                    tour.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    tour.From.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    tour.To.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    tour.Description.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                foreach (var tour in filteredTours)
                {
                    Tours.Add(tour);
                }
            }
        }

        private async Task AddTour()
        {
            var addTourWindow = new AddTourWindow();
            var addTourViewModel = new TourWindowViewModel();

            addTourWindow.DataContext = addTourViewModel;
            addTourViewModel.CloseAction = () => addTourWindow.Close();

            if (addTourWindow.ShowDialog() == true)
            {
                var newTour = new Tour(
                    addTourViewModel.Name,
                    addTourViewModel.Description,
                    addTourViewModel.From,
                    addTourViewModel.To,
                    addTourViewModel.TransportType
                );

                try
                {
                    logger.Debug($"Attempting to add new tour: {newTour.Name}");
                    var addedTour = await _tourService.AddTourAndGeoCodesAsync(newTour);
                    Tours.Add(addedTour);
                    logger.Debug($"Successfully added tour: {addedTour.Name} with ID {addedTour.Id}");
                }
                catch (FailedToRetrieveCoordinatesException ex)
                {
                    logger.Error($"Failed to retrieve coordinates for tour '{newTour.Name}': {ex.Message}");
                    System.Windows.MessageBox.Show("Failed to find locations", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                catch (FailedToRetrieveRouteException ex)
                {
                    logger.Error($"Failed to retrieve route for tour '{newTour.Name}': {ex.Message}");
                    System.Windows.MessageBox.Show("Failed to create route", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    logger.Error($"Failed to add tour '{newTour.Name}': {ex.Message}");
                    System.Windows.MessageBox.Show($"Failed to add tour: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private async Task RemoveTour()
        {
            if (CurrentTour == null)
            {
                MessageBox.Show("Select a Tour to delete first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    logger.Debug($"Attempting to remove tour with ID {CurrentTour.Id} and Name '{CurrentTour.Name}'");
                    await _tourService.DeleteTourAsync(CurrentTour.Id);
                    logger.Debug($"Successfully removed tour with ID {CurrentTour.Id}");
                    Tours.Remove(CurrentTour);
                    CurrentTour = null;
                    _mediator?.Notify(this, "TourRemoved");
                }
                catch (Exception ex)
                {
                    logger.Error($"Failed to remove tour with ID {CurrentTour.Id}: {ex.Message}");
                    MessageBox.Show($"Failed to remove tour: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private async Task ModifyTour()
        {
            if (CurrentTour == null)
            {
                MessageBox.Show("Select a Tour to modify!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var modifyTourWindow = new AddTourWindow();
                var modifyTourViewModel = new TourWindowViewModel(CurrentTour);

                modifyTourWindow.DataContext = modifyTourViewModel;
                modifyTourViewModel.CloseAction = () => modifyTourWindow.Close();

                if (modifyTourWindow.ShowDialog() == true)
                {
                    CurrentTour.Name = modifyTourViewModel.Name;
                    CurrentTour.Description = modifyTourViewModel.Description;
                    CurrentTour.From = modifyTourViewModel.From;
                    CurrentTour.To = modifyTourViewModel.To;
                    CurrentTour.TransportType = modifyTourViewModel.TransportType;

                    try
                    {
                        logger.Debug($"Attempting to update tour with ID {CurrentTour.Id}");
                        await _tourService.UpdateTourAsync(CurrentTour);
                        logger.Debug($"Successfully updated tour with ID {CurrentTour.Id}");
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Failed to update tour with ID {CurrentTour.Id}: {ex.Message}");
                        System.Windows.MessageBox.Show($"Failed to update tour: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
            }
        }

        private async void LoadTours()
        {
            try
            {
                var tours = await _tourService.GetAllToursAsync();
                _allTours = tours.ToList();

                foreach (var tour in tours)
                {
                    Tours.Add(tour);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
