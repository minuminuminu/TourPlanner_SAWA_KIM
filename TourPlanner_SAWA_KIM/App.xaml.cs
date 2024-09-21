using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data;
using System.Windows;
using TourPlanner_SAWA_KIM.ViewModels;
using TourPlanner_SAWA_KIM.DAL;
using TourPlanner_SAWA_KIM.BLL;
using Microsoft.Extensions.Configuration.Json;
using System.Net.Http;


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

            HttpClient httpClient = new HttpClient();

            var dbContext = new AppDbContext(configuration);
            var apiClient = new ApiClient(httpClient, configuration);

            try
            {
                //dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            ITourRepository tourRepository = new TourRepository(dbContext);
            ITourLogRepository tourLogRepository = new TourLogRepository(dbContext);

            var tourService = new TourService(tourRepository, tourLogRepository, apiClient);
            var pdfService = new PDFService();

            var mainWindowViewModel = new MainWindowViewModel(tourService, pdfService);
            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainWindowViewModel;

            mainWindow.Show();
        }
    }
}
