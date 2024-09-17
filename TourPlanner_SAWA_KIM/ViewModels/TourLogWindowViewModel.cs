using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class TourLogWindowViewModel
    {
        private DateTime _date;

        public int Rating { get; set; }
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (value.Kind == DateTimeKind.Unspecified)
                {
                    _date = DateTime.SpecifyKind(value, DateTimeKind.Utc);
                }
                else
                {
                    _date = value;
                }
            }
        }
        public TimeSpan Duration { get; set; }
        public double Distance { get; set; }
        public string Difficulty { get; set; }
        public string Comment { get; set; }
        public ICommand ConfirmCommand { get; }
        public Action CloseAction { get; set; }

        public TourLogWindowViewModel(TourLog existingTour = null)
        {
            ConfirmCommand = new RelayCommand(Confirm);

            if(existingTour != null) // if window gets created modify existing tour log
            {
                Rating = existingTour.Rating;
                Date = existingTour.Date;
                Duration = existingTour.Duration;
                Distance = existingTour.Distance;
                Difficulty = existingTour.Difficulty;
                Comment = existingTour.Comment;
            }
        }

        private void Confirm()
        {
            if (Rating < 1 || Rating > 5)
            {
                System.Windows.MessageBox.Show("Rating must be between 1 and 5!", "Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            if (Date == DateTime.MinValue || Duration == TimeSpan.Zero || Distance <= 0 || string.IsNullOrWhiteSpace(Difficulty))
            {
                System.Windows.MessageBox.Show("Please fill in all fields!", "Validation Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            var window = System.Windows.Application.Current.Windows
                    .OfType<System.Windows.Window>()
                    .SingleOrDefault(w => w.IsActive);

            if (window != null)
            {
                window.DialogResult = true;  // This will make ShowDialog() return true
            }

            CloseAction?.Invoke();
        }
    }
}
