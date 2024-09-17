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
using TourPlanner_SAWA_KIM.Mediators;
using TourPlanner_SAWA_KIM.Models;
using TourPlanner_SAWA_KIM.Views.Windows;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class ToursListViewModel : ViewModelBase
    {
        private Tour _currentTour;
        private IMediator _mediator;
        private readonly TourService _tourService;

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

        public ToursListViewModel(TourService tourService)
        {
            Tours = new ObservableCollection<Tour>();

            AddTourCommand = new RelayCommand(AddTour);
            RemoveTourCommand = new RelayCommand(RemoveTour);
            ModifyTourCommand = new RelayCommand(ModifyTour);

            _tourService = tourService;

            //LoadDummyTours();
            LoadTours();
        }

        //private void AddTour()
        //{
        //    var addTourWindow = new AddTourWindow();
        //    var addTourViewModel = new AddTourWindowViewModel();

        //    addTourWindow.DataContext = addTourViewModel;
        //    addTourViewModel.CloseAction = () => addTourWindow.Close();

        //    if (addTourWindow.ShowDialog() == true)
        //    {
        //        var newTour = new Tour(addTourViewModel.Name, addTourViewModel.Description, addTourViewModel.From, addTourViewModel.To, addTourViewModel.TransportType)
        //        {
        //            Distance = addTourViewModel.Distance,
        //            EstimatedTime = addTourViewModel.EstimatedTime
        //        };

        //        Tours.Add(newTour);
        //    }
        //}

        private async void AddTour()
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

                try
                {
                    var addedTour = await _tourService.AddTourAsync(newTour);
                    Tours.Add(addedTour);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        //private void RemoveTour()
        //{
        //    if(CurrentTour == null)
        //    {
        //        MessageBox.Show("Select a Tour to delete first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    } 
        //    else
        //    {
        //        Tours.Remove(CurrentTour);
        //        CurrentTour = null;
        //        _mediator?.Notify(this, "TourRemoved");
        //    }
        //}

        private async void RemoveTour()
        {
            if (CurrentTour == null)
            {
                MessageBox.Show("Select a Tour to delete first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    await _tourService.DeleteTourAsync(CurrentTour.Id);
                    Tours.Remove(CurrentTour);
                    CurrentTour = null;
                    _mediator?.Notify(this, "TourRemoved");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        //private void ModifyTour()
        //{
        //    if (CurrentTour == null)
        //    {
        //        MessageBox.Show("Select a Tour to modify!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    else
        //    {

        //        _mediator?.Notify(CurrentTour, "TourModified");
        //    }
        //}

        private async void ModifyTour()
        {
            if (CurrentTour == null)
            {
                MessageBox.Show("Select a Tour to modify!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var modifyTourWindow = new AddTourWindow();
                var modifyTourViewModel = new AddTourWindowViewModel(CurrentTour);

                modifyTourWindow.DataContext = modifyTourViewModel;
                modifyTourViewModel.CloseAction = () => modifyTourWindow.Close();

                if (modifyTourWindow.ShowDialog() == true)
                {
                    CurrentTour.Name = modifyTourViewModel.Name;
                    CurrentTour.Description = modifyTourViewModel.Description;
                    CurrentTour.From = modifyTourViewModel.From;
                    CurrentTour.To = modifyTourViewModel.To;
                    CurrentTour.TransportType = modifyTourViewModel.TransportType;
                    CurrentTour.Distance = modifyTourViewModel.Distance;
                    CurrentTour.EstimatedTime = modifyTourViewModel.EstimatedTime;

                    try
                    {
                        await _tourService.UpdateTourAsync(CurrentTour);

                        RaisePropertyChangedEvent(nameof(CurrentTour));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        private async void LoadTours()
        {
            try
            {
                var tours = await _tourService.GetAllToursAsync();

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
