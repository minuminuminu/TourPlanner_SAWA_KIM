using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;
using TourPlanner_SAWA_KIM.ViewModels;
using TourPlanner_SAWA_KIM.DAL;
using TourPlanner_SAWA_KIM.BLL;
using Microsoft.Extensions.Configuration.Json;


namespace TourPlanner_SAWA_KIM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var dbContext = new AppDbContext(configuration);

            try
            {
                dbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ITourRepository tourRepository = new TourRepository(dbContext);
            var tourService = new TourService(tourRepository);
            var mainWindowViewModel = new MainWindowViewModel(tourService);
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainWindowViewModel;

            mainWindow.Show();
        }
    }
}
