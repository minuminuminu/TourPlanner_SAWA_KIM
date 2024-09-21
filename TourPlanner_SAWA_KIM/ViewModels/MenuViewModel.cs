using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner_SAWA_KIM.BLL;
using TourPlanner_SAWA_KIM.Exceptions;
using TourPlanner_SAWA_KIM.Mediators;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private Tour _selectedTour;
        private IMediator _mediator;
        private readonly TourService _tourService;
        private readonly PDFService _pdfService;

        public Tour ImportedTour { get; set; }
        public ICommand GenTourReport { get; }
        public ICommand GenSummaryReport { get; }
        public ICommand ExitProgram { get; }
        public ICommand ImportTour { get; }
        public ICommand ExportTour { get; }
        public ICommand ToggleDarkMode { get; }


        public MenuViewModel(TourService tourService, PDFService pdfService)
        {
            ExitProgram = new RelayCommand(ExitApplication);
            ImportTour = new RelayCommand(OpenImportDialog);
            ExportTour = new RelayCommand(OpenSaveDialog);
            ToggleDarkMode = new RelayCommand(ToggleDark);
            GenTourReport = new RelayCommand(GenerateTourReport);
            GenSummaryReport = new RelayCommand(GenerateTourSummary);

            _tourService = tourService;
            _pdfService = pdfService;   
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        private void ToggleDark()
        {
            var app = Application.Current;
            if (app == null) return;

            var dictionaries = app.Resources.MergedDictionaries;
            var darkTheme = dictionaries.FirstOrDefault(d => d.Source?.OriginalString == "/Themes/DarkTheme.xaml");
            var lightTheme = dictionaries.FirstOrDefault(d => d.Source?.OriginalString == "/Themes/LightTheme.xaml");

            if (darkTheme != null)
            {
                dictionaries.Remove(darkTheme);
                dictionaries.Add(new ResourceDictionary { Source = new Uri("/Themes/LightTheme.xaml", UriKind.Relative) });
            }
            else
            {
                dictionaries.Remove(lightTheme);
                dictionaries.Add(new ResourceDictionary { Source = new Uri("/Themes/DarkTheme.xaml", UriKind.Relative) });
            }
        }

        private async void OpenImportDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Tour-data files (*.json)|*.json";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;

                try
                {
                    ImportedTour = await _tourService.ImportTour(fileName);
                    foreach(var log in ImportedTour.ImportedTourLogsList)
                    {
                        log.TourId = ImportedTour.Id;
                    }

                    await _mediator.Notify(this, "TourImported");
                }
                catch (FailedToImportTourException)
                {
                    MessageBox.Show($"Error importing file!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void OpenSaveDialog()
        {
            if(_selectedTour == null)
            {
                MessageBox.Show("Select a Tour to export first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Tour-data files (*.json)|*.json";
            saveFileDialog.DefaultExt = ".json";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                try
                {
                    await _tourService.ExportTour(_selectedTour, fileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Error saving file: {e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void GenerateTourSummary()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Tour-Report files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = ".pdf";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                try
                {
                    var tours = await _tourService.GetAllToursWithLogs();
                    _pdfService.GenerateSummaryReport(fileName, tours);
                }
                catch (ArgumentNullException argEx)
                {
                    MessageBox.Show($"A required argument was missing: {argEx.ParamName}", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    MessageBox.Show($"Invalid operation: {invalidOpEx.Message}", "Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FileNotFoundException fileEx)
                {
                    MessageBox.Show($"File not found: {fileEx.FileName}", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show($"I/O Error while generating report: {ioEx.Message}", "I/O Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}\n\nDetails:\n{ex.StackTrace}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void GenerateTourReport()
        {
            if (_selectedTour == null)
            {
                MessageBox.Show("Select a Tour first!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Tour-Report files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = ".pdf";

            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;

                try
                {
                    var tour = await _tourService.GetSingleTourWithLogsByIdAsync(_selectedTour.Id);
                    _pdfService.GenerateTourReport(fileName, tour);
                }
                catch (ArgumentNullException argEx)
                {
                    MessageBox.Show($"A required argument was missing: {argEx.ParamName}", "Argument Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (InvalidOperationException invalidOpEx)
                {
                    MessageBox.Show($"Invalid operation: {invalidOpEx.Message}", "Operation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (FileNotFoundException fileEx)
                {
                    MessageBox.Show($"File not found: {fileEx.FileName}", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (IOException ioEx)
                {
                    MessageBox.Show($"I/O Error while generating report: {ioEx.Message}", "I/O Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An unexpected error occurred: {ex.Message}\n\nDetails:\n{ex.StackTrace}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ExitApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }

        public void SetSelectedTour(Tour selectedTour)
        {
            _selectedTour = selectedTour;
        }
    }
}
