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
using System.ComponentModel;
using System.Diagnostics;

namespace TourPlanner_SAWA_KIM.Views
{
    /// <summary>
    /// Interaction logic for ToursOverview.xaml
    /// </summary>
    public partial class ToursOverview : UserControl
    {
        private ToursOverviewViewModel _viewModel;
        private bool _isWebViewReady = false;

        public ToursOverview()
        {
            InitializeComponent();
            InitializeWebView2Async(); 

            _viewModel = DataContext as ToursOverviewViewModel;
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
            else
            {
                DataContextChanged += ToursOverview_DataContextChanged;
            }
        }

        private void ToursOverview_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            _viewModel = DataContext as ToursOverviewViewModel;
            if (_viewModel != null)
            {
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private async void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.WriteLine("1");
            if (e.PropertyName == nameof(ToursOverviewViewModel.DirectionsJson))
            {
                Debug.WriteLine("2");
                if (_viewModel.DirectionsJson != null)
                {
                    Debug.WriteLine("3");
                    await webView.EnsureCoreWebView2Async(null);
                    Debug.WriteLine("4");

                    if (_isWebViewReady)
                    {
                        try
                        {
                            string script = $"updateDirections({_viewModel.DirectionsJson});";
                            Debug.WriteLine("5");

                            await webView.CoreWebView2.ExecuteScriptAsync(script);

                            Debug.WriteLine("6");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error executing script: {ex.Message}");
                        }
                    }
                    else
                    {
                        Debug.WriteLine("WebView2 not ready yet.");
                    }
                }
            }
        }

        private async void CoreWebView2_DOMContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            _isWebViewReady = true;
            if (_viewModel != null && _viewModel.DirectionsJson != null)
            {
                try
                {
                    string script = $"updateDirections({_viewModel.DirectionsJson});";
                    await webView.CoreWebView2.ExecuteScriptAsync(script);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error executing script in DOMContentLoaded: {ex.Message}");
                }
            }
        }

        private async void InitializeWebView2Async()
        {
            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.DOMContentLoaded += CoreWebView2_DOMContentLoaded;
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = System.IO.Path.Combine(appDir, "Resources/leaflet.html");
            webView.CoreWebView2.Navigate(filePath);
        }
    }
}
