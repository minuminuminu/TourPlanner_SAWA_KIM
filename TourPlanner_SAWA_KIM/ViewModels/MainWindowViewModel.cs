using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.BLL;
using TourPlanner_SAWA_KIM.Mediators;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly TourService _tourService;

        public ToursListViewModel ToursListViewModel { get; }
        public ToursOverviewViewModel ToursOverviewViewModel { get; }
        public ToursLogsViewModel ToursLogsViewModel { get; }
        public SearchBarViewModel SearchBarViewModel { get; }

        public MainWindowViewModel(TourService tourService)
        {
            ToursListViewModel = new ToursListViewModel(tourService);
            ToursOverviewViewModel = new ToursOverviewViewModel();
            ToursLogsViewModel = new ToursLogsViewModel(tourService);
            SearchBarViewModel = new SearchBarViewModel();

            var mediator = new TourMediator(ToursListViewModel, ToursOverviewViewModel, ToursLogsViewModel);
            _tourService = tourService;
        }
    }
}
