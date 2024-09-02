using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class AddTourWindowViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransportType { get; set; }
        public double Distance { get; set; }
        public TimeSpan EstimatedTime { get; set; }

        public ICommand ConfirmCommand { get; }

        public Action CloseAction { get; set; }

        public AddTourWindowViewModel()
        {
            ConfirmCommand = new RelayCommand(Confirm);
        }

        private void Confirm()
        {
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
