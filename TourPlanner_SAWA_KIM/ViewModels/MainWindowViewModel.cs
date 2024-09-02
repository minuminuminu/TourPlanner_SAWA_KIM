using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Mediators;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ToursListViewModel ToursListViewModel { get; }
        public ToursOverviewViewModel ToursOverviewViewModel { get; }
        public ToursLogsViewModel ToursLogsViewModel { get; }
        public SearchBarViewModel SearchBarViewModel { get; }

        public MainWindowViewModel()
        {
            ToursListViewModel = new ToursListViewModel();
            ToursOverviewViewModel = new ToursOverviewViewModel();
            ToursLogsViewModel = new ToursLogsViewModel();
            SearchBarViewModel = new SearchBarViewModel();

            var mediator = new TourMediator(ToursListViewModel, ToursOverviewViewModel);
        }
    }
}
