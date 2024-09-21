using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourPlanner_SAWA_KIM.ViewModels;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace TourPlanner_SAWA_KIM.Views
{
    /// <summary>
    /// Interaction logic for ToursOverview.xaml
    /// </summary>
    public partial class ToursOverview : UserControl
    {
        public ToursOverview()
        {
            InitializeComponent();
            InitializeWebView2Async();
        }

        private async void InitializeWebView2Async()
        {
            await webView.EnsureCoreWebView2Async(null);
            // Assuming "example.html" is at the root of your project directory and set to "Copy to Output Directory"
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(appDir, "Resources/leaflet.html");
            webView.CoreWebView2.Navigate(filePath);
        }
    }
}
