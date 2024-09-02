using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Mediators;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class ToursOverviewViewModel : ViewModelBase
    {
        private Tour _selectedTour;
        private IMediator _mediator;

        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                //if ((_selectedTour != value) && (value != null))
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    RaisePropertyChangedEvent(nameof(SelectedTour));
                }
            }
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void UpdateTourDetails(Tour tour)
        {
            SelectedTour = tour;
        }

        public void ClearTourDetails()
        {
            SelectedTour = null;
        }
    }
}
