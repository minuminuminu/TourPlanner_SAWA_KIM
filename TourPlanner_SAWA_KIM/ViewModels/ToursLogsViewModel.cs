using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner_SAWA_KIM.BLL;
using TourPlanner_SAWA_KIM.Mediators;
using TourPlanner_SAWA_KIM.Models;
using TourPlanner_SAWA_KIM.Views.Windows;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class ToursLogsViewModel : ViewModelBase
    {
        private readonly TourService _tourService;
        private IMediator _mediator;
        private TourLog _selectedTourLog;
        private Tour? _selectedTour;

        public ObservableCollection<TourLog> TourLogs { get; set; }
        public ICommand AddTourLogCommand { get; }
        public ICommand RemoveTourLogCommand { get; }
        public ICommand ModifyTourLogCommand { get; }

        public TourLog SelectedTourLog
        {
            get { return _selectedTourLog; }
            set
            {
                if (_selectedTourLog != value)
                {
                    _selectedTourLog = value;
                    RaisePropertyChangedEvent(nameof(SelectedTourLog));
                }
            }
        }

        public ToursLogsViewModel(TourService tourService)
        {
            _tourService = tourService;
            TourLogs = new ObservableCollection<TourLog>();
            _selectedTour = null;

            AddTourLogCommand = new RelayCommand(async () => await AddTourLogAsync());
            RemoveTourLogCommand = new RelayCommand(async () => await RemoveTourLogAsync());
            ModifyTourLogCommand = new RelayCommand(async () => await ModifyTourLog());
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        private async Task AddTourLogAsync()
        {
            if (_selectedTour == null)
            {
                System.Windows.MessageBox.Show("Please select a tour to add a tour log to!", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            var addTourLogWindow = new AddTourLogWindow();
            var addTourLogWindowViewModel = new TourLogWindowViewModel();

            addTourLogWindow.DataContext = addTourLogWindowViewModel;
            addTourLogWindowViewModel.CloseAction = () => addTourLogWindow.Close();

            if(addTourLogWindow.ShowDialog() == true)
            {
                var newTourLog = new TourLog(addTourLogWindowViewModel.Rating, addTourLogWindowViewModel.Date, addTourLogWindowViewModel.Duration, addTourLogWindowViewModel.Distance, addTourLogWindowViewModel.Difficulty, addTourLogWindowViewModel.Comment);
                newTourLog.TourId = _selectedTour.Id;

                try
                {
                    var addedTourLog = await _tourService.AddTourLogAsync(newTourLog);
                    TourLogs.Add(addedTourLog);

                    _mediator?.Notify(this, "TourLogsLoaded");
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Failed to add tour log: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
        }

        private async Task RemoveTourLogAsync()
        {
            if (SelectedTourLog == null)
            {
                System.Windows.MessageBox.Show("Please select a tour log to remove!", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            try
            {
                await _tourService.DeleteTourLogAsync(SelectedTourLog.Id);
                TourLogs.Remove(SelectedTourLog);

                _mediator?.Notify(this, "TourLogsLoaded");
                //_selectedTour = null; why did i do this again
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Failed to remove tour log: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private async Task ModifyTourLog()
        {
            if (SelectedTourLog == null)
            {
                System.Windows.MessageBox.Show("Please select a tour log to modify!", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }
            else
            {
                var modifyTourLogWindow = new AddTourLogWindow();
                var modifyTourLogWindowViewModel = new TourLogWindowViewModel(SelectedTourLog);

                modifyTourLogWindow.DataContext = modifyTourLogWindowViewModel;
                modifyTourLogWindowViewModel.CloseAction = () => modifyTourLogWindow.Close();

                if (modifyTourLogWindow.ShowDialog() == true)
                {
                    SelectedTourLog.Rating = modifyTourLogWindowViewModel.Rating;
                    SelectedTourLog.Date = modifyTourLogWindowViewModel.Date;
                    SelectedTourLog.Duration = modifyTourLogWindowViewModel.Duration;
                    SelectedTourLog.Distance = modifyTourLogWindowViewModel.Distance;
                    SelectedTourLog.Difficulty = modifyTourLogWindowViewModel.Difficulty;
                    SelectedTourLog.Comment = modifyTourLogWindowViewModel.Comment;

                    try
                    {
                        await _tourService.UpdateTourLogAsync(SelectedTourLog);

                        _mediator?.Notify(this, "TourLogsLoaded");
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"Failed to update tour log: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                    }
                }
            }
        }

        public async Task LoadTourLogsAsync(int tourId)
        {
            var tourLogs = await _tourService.GetTourLogsByTourIdAsync(tourId);
            TourLogs.Clear();
            foreach (var tourLog in tourLogs)
            {
                TourLogs.Add(tourLog);
            }

            _mediator?.Notify(this, "TourLogsLoaded");
        }

        public void ClearTourLogs()
        {
            TourLogs.Clear();
        }

        public void SetSelectedTour(Tour selectedTour)
        {
            _selectedTour = selectedTour;
        }
    }
}
