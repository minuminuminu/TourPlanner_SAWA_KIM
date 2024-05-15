using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.BLL;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class TourTabControlViewModel : ViewModelBase
    {
        private Tour _selectedTour;

        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public TourTabControlViewModel()
        {
            TourMediator.Instance.TourSelected += OnTourSelected;
        }

        private void OnTourSelected(Tour selectedTour)
        {
            SelectedTour = selectedTour;
        }
    }
}
