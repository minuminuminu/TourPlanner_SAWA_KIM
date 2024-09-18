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
            webView.CoreWebView2.Navigate("about:blank");
        }
    }
}
