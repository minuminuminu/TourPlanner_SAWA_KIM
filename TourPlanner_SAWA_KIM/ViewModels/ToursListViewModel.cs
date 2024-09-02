using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner_SAWA_KIM.Mediators;
using TourPlanner_SAWA_KIM.Models;
using TourPlanner_SAWA_KIM.Views.Windows;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class ToursListViewModel : ViewModelBase
    {
        private Tour _currentTour;
        private IMediator _mediator;

        public ObservableCollection<Tour> Tours { get; set; }

        public ICommand AddTourCommand { get; }
        public ICommand RemoveTourCommand { get; }
        public ICommand ModifyTourCommand { get; }

        public Tour CurrentTour
        {
            get { return _currentTour; }
            set
            {
                //if ((_currentTour != value) && (value != null))
                if (_currentTour != value)
                {
                    _currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                    _mediator?.Notify(this, "TourSelected"); // raise "TourSelected" event and pass tour as param
                }
            }
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ToursListViewModel()
        {
            Tours = new ObservableCollection<Tour>();

            AddTourCommand = new RelayCommand(AddTour);
            RemoveTourCommand = new RelayCommand(RemoveTour);
            ModifyTourCommand = new RelayCommand(ModifyTour);

            LoadDummyTours();
        }

        private void AddTour()
        {
            var addTourWindow = new AddTourWindow();
            var addTourViewModel = new AddTourWindowViewModel();

            addTourWindow.DataContext = addTourViewModel;
            addTourViewModel.CloseAction = () => addTourWindow.Close();

            if (addTourWindow.ShowDialog() == true)
            {
                var newTour = new Tour(addTourViewModel.Name, addTourViewModel.Description, addTourViewModel.From, addTourViewModel.To, addTourViewModel.TransportType)
                {
                    Distance = addTourViewModel.Distance,
                    EstimatedTime = addTourViewModel.EstimatedTime
                };

                Tours.Add(newTour);
            }
        }

        private void RemoveTour()
        {
            if(CurrentTour == null)
            {
                MessageBox.Show("Select a Tour to delete first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            else
            {
                Tours.Remove(CurrentTour);
                CurrentTour = null;
                _mediator?.Notify(this, "TourRemoved");
            }
        }

        private void ModifyTour()
        {
            if (CurrentTour == null)
            {
                MessageBox.Show("Select a Tour to modify!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                _mediator?.Notify(CurrentTour, "TourModified");
            }
        }

        private void LoadDummyTours()
        {
            Tours.Add(new Tour("Mountain Hike", "A challenging mountain hike.", "Trailhead", "Summit", "Hiking")
            {
                Distance = 10.5,
                EstimatedTime = new TimeSpan(5, 0, 0),
                RouteImage = @"C:\Images\MountainHike.png"
            });

            Tours.Add(new Tour("Beach Run", "A run along the beach.", "Beach Start", "Beach End", "Running")
            {
                Distance = 5.0,
                EstimatedTime = new TimeSpan(0, 45, 0),
                RouteImage = @"C:\Images\BeachRun.png"
            });

            Tours.Add(new Tour("City Tour", "A relaxing tour through the city’s landmarks.", "City Center", "Old Town", "Walking")
            {
                Distance = 8.0,
                EstimatedTime = new TimeSpan(3, 0, 0),
                RouteImage = @"C:\Images\CityTour.png"
            });

            Tours.Add(new Tour("Forest Trek", "A peaceful trek through the dense forest.", "Forest Entrance", "Lake View", "Hiking")
            {
                Distance = 12.3,
                EstimatedTime = new TimeSpan(6, 30, 0),
                RouteImage = @"C:\Images\ForestTrek.png"
            });
        }
    }
}
