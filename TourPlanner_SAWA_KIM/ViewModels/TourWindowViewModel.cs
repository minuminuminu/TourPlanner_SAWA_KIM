using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class TourWindowViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransportType { get; set; }

        public ICommand ConfirmCommand { get; }

        public Action CloseAction { get; set; }

        public TourWindowViewModel(Tour existingTour = null)
        {
            ConfirmCommand = new RelayCommand(Confirm);

            if (existingTour != null) // if window gets created modify existing tour
            {
                Name = existingTour.Name;
                Description = existingTour.Description;
                From = existingTour.From;
                To = existingTour.To;
                TransportType = existingTour.TransportType;
            }
        }

        private void Confirm()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(From) || string.IsNullOrWhiteSpace(To) ||
                string.IsNullOrWhiteSpace(TransportType))
            {
                MessageBox.Show("Please fill in all fields!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var window = System.Windows.Application.Current.Windows
                    .OfType<System.Windows.Window>()
                    .SingleOrDefault(w => w.IsActive);

            if (window != null)
            {
                window.DialogResult = true;  // This will make ShowDialog() return true
            }

            CloseAction?.Invoke();  // Close the dialog window
        }
    }
}
