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
        private readonly ITourService _tourService;
        private readonly IPDFService _pDFService;

        public MenuViewModel MenuViewModel { get; }
        public ToursListViewModel ToursListViewModel { get; }
        public ToursOverviewViewModel ToursOverviewViewModel { get; }
        public ToursLogsViewModel ToursLogsViewModel { get; }
        public SearchBarViewModel SearchBarViewModel { get; }

        public MainWindowViewModel(ITourService tourService, IPDFService pdfService)
        {
            MenuViewModel = new MenuViewModel(tourService, pdfService);
            ToursListViewModel = new ToursListViewModel(tourService);
            ToursOverviewViewModel = new ToursOverviewViewModel(tourService);
            ToursLogsViewModel = new ToursLogsViewModel(tourService);
            SearchBarViewModel = new SearchBarViewModel();

            var mediator = new TourMediator(SearchBarViewModel, MenuViewModel, ToursListViewModel, ToursOverviewViewModel, ToursLogsViewModel);
            _tourService = tourService;
        }
    }
}
