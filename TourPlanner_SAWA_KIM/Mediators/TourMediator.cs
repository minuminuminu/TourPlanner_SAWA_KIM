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
        private readonly MenuViewModel _menuViewModel;
        private readonly ToursListViewModel _toursListViewModel;
        private readonly ToursOverviewViewModel _toursOverviewViewModel;
        private readonly ToursLogsViewModel _toursLogsViewModel;
        private readonly SearchBarViewModel _searchBarViewModel;

        public TourMediator(SearchBarViewModel searchBarViewModel,  MenuViewModel menuViewModel, ToursListViewModel toursListViewModel, ToursOverviewViewModel tourOverviewViewModel, ToursLogsViewModel toursLogsViewModel)
        {
            _menuViewModel = menuViewModel;
            _menuViewModel.SetMediator(this);

            _toursListViewModel = toursListViewModel;
            _toursListViewModel.SetMediator(this);

            _toursOverviewViewModel = tourOverviewViewModel;
            _toursOverviewViewModel.SetMediator(this);

            _toursLogsViewModel = toursLogsViewModel;
            _toursLogsViewModel.SetMediator(this);

            _searchBarViewModel = searchBarViewModel;
            _searchBarViewModel.SetMediator(this);
        }

        public async Task Notify(object sender, string eventName)
        {
            // on tour click left side 
            if (eventName == "TourSelected" && _toursListViewModel.CurrentTour != null)
            {
                var selectedTour = _toursListViewModel.CurrentTour; // take clicked tour
                _toursOverviewViewModel.UpdateTourDetails(selectedTour); // tell right side 
                await _toursLogsViewModel.LoadTourLogsAsync(selectedTour.Id); // load tour logs for specific tour
                _toursLogsViewModel.SetSelectedTour(selectedTour); // set selected tour in logs
                _menuViewModel.SetSelectedTour(selectedTour); // set selected tour for import/export
            } else if(eventName == "TourRemoved")
            {
                _toursOverviewViewModel.ClearTourDetails();
                _toursLogsViewModel.ClearTourLogs();
                _menuViewModel.SetSelectedTour(null);
                _toursOverviewViewModel.ComputeAttributes(_toursLogsViewModel.TourLogs);
            } else if(eventName == "TourLogsLoaded")
            {
                _toursOverviewViewModel.ComputeAttributes(_toursLogsViewModel.TourLogs);
            } else if(eventName == "TourImported")
            {
                _toursListViewModel.AddTour(_menuViewModel.ImportedTour);
                await _toursLogsViewModel.AddImportedTourLogs(_menuViewModel.ImportedTour.ImportedTourLogsList);
            } else if(eventName == "Search")
            {
                _toursListViewModel.FilterTours(_searchBarViewModel.Content);
            }
        }
    }
}
