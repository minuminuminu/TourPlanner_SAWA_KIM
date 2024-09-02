using System.Configuration;
using System.Data;
using System.Windows;
using TourPlanner_SAWA_KIM.ViewModels;

namespace TourPlanner_SAWA_KIM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //private void App_OnStartup(object sender, StartupEventArgs e)
        //{
        //    var searchBarViewModel = new SearchBarViewModel();
        //    var toursListViewModel = new ToursListViewModel();
        //    var toursLogsViewModel = new ToursLogsViewModel();
        //    var toursOverviewViewModel = new ToursOverviewViewModel();

        //    var wnd = new MainWindow
        //    {
        //        DataContext = new MainWindowViewModel(),
        //        SearchBar = { DataContext = searchBarViewModel },
        //        ToursList = { DataContext = toursListViewModel },
        //        ToursLogs = { DataContext = toursLogsViewModel },
        //        ToursOverview = { DataContext = toursOverviewViewModel }
        //    };

        //    wnd.Show();
        //}
    }
}
