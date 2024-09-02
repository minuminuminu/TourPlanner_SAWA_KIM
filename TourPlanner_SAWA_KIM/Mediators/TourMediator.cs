using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.ViewModels;

namespace TourPlanner_SAWA_KIM.Mediators
{ 
    // https://refactoring.guru/design-patterns/mediator/csharp/example
    public class TourMediator : IMediator
    {
        private readonly ToursListViewModel _toursListViewModel;
        private readonly ToursOverviewViewModel _toursOverviewViewModel;

        public TourMediator(ToursListViewModel toursListViewModel, ToursOverviewViewModel tourOverviewViewModel)
        {
            _toursListViewModel = toursListViewModel;
            _toursListViewModel.SetMediator(this);

            _toursOverviewViewModel = tourOverviewViewModel;
            _toursOverviewViewModel.SetMediator(this);
        }

        public void Notify(object sender, string eventName)
        {
            // on tour click left side 
            if (eventName == "TourSelected")
            {
                var selectedTour = _toursListViewModel.CurrentTour; // take clicked tour
                _toursOverviewViewModel.UpdateTourDetails(selectedTour); // tell right side 
            } else if(eventName == "TourRemoved")
            {
                _toursOverviewViewModel.ClearTourDetails();
            }
        }
    }
}
