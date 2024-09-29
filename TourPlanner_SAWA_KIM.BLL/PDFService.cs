using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using TourPlanner_SAWA_KIM.Logging;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public class PDFService : IPDFService
    {
        private ILoggerWrapper logger = LoggerFactory.GetLogger();

        public void GenerateTourReport(string filename, Tour tour)
        {
            logger.Debug($"Starting to generate tour report for '{tour.Name}' into file '{filename}'.");

            try
            {
                using (PdfWriter writer = new PdfWriter(filename))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdf))
                        {
                            Paragraph header = new Paragraph("Tour Report")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                .SetFontSize(14)
                                .SetBold();
                            document.Add(header);

                            document.Add(new Paragraph($"Tour Name: {tour.Name}").SetFontSize(11).SetBold());
                            document.Add(new Paragraph($"Description: {tour.Description}").SetFontSize(11));
                            document.Add(new Paragraph($"From: {tour.From}, To: {tour.To}").SetFontSize(11));
                            document.Add(new Paragraph($"Distance: {tour.Distance} km").SetFontSize(11));
                            document.Add(new Paragraph($"Transport: {tour.TransportType}").SetFontSize(11));
                            document.Add(new Paragraph($"Estimated Time: {tour.EstimatedTime}").SetFontSize(11));

                            document.Add(new Paragraph("\n"));

                            Paragraph logsHeader = new Paragraph("Tour Logs")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                                .SetFontSize(12)
                                .SetBold();
                            document.Add(logsHeader);

                            foreach (var tourlog in tour.TourLogs)
                            {
                                document.Add(new Paragraph($"Date: {tourlog.Date.ToShortDateString()}").SetFontSize(10).SetBold());
                                document.Add(new Paragraph($"Duration: {tourlog.Duration}").SetFontSize(10));
                                document.Add(new Paragraph($"Distance: {tourlog.Distance} km").SetFontSize(10));
                                document.Add(new Paragraph($"Difficulty: {tourlog.Difficulty}").SetFontSize(10));
                                document.Add(new Paragraph($"Rating: {tourlog.Rating}/5").SetFontSize(10));
                                document.Add(new Paragraph($"Comment: {tourlog.Comment}").SetFontSize(10));

                                document.Add(new Paragraph("\n----------------------------\n"));
                            }

                            if (!tour.TourLogs.Any())
                            {
                                document.Add(new Paragraph("No tour logs available for this tour.").SetFontSize(10).SetItalic());
                            }

                            document.Close();
                        }
                    }
                }

                logger.Debug($"Successfully generated tour report for '{tour.Name}' into file '{filename}'.");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                logger.Error($"Access denied when generating tour report for '{tour.Name}': {uaEx.Message}");
                MessageBox.Show($"Access denied: {uaEx.Message}", "Permission Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DirectoryNotFoundException dnEx)
            {
                logger.Error($"Directory not found when generating tour report for '{tour.Name}': {dnEx.Message}");
                MessageBox.Show($"Directory not found: {dnEx.Message}", "Directory Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ioEx)
            {
                logger.Error($"I/O error when generating tour report for '{tour.Name}': {ioEx.Message}");
                MessageBox.Show($"I/O Error: {ioEx.Message}", "I/O Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                logger.Fatal($"An unexpected error occurred when generating tour report for '{tour.Name}': {ex}");
                MessageBox.Show($"An unexpected error occurred: {ex}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GenerateSummaryReport(string filename, IEnumerable<Tour> tours)
        {
            logger.Debug($"Starting to generate summary report into file '{filename}'.");

            try
            {
                using (PdfWriter writer = new PdfWriter(filename))
                {
                    using (PdfDocument pdf = new PdfDocument(writer))
                    {
                        using (Document document = new Document(pdf))
                        {
                            Paragraph header = new Paragraph("Summary Report")
                                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                                .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                                .SetFontSize(14)
                                .SetBold();
                            document.Add(header);

                            document.Add(new Paragraph("\n"));

                            foreach (var tour in tours)
                            {
                                if (tour.TourLogs != null && tour.TourLogs.Any())
                                {
                                    var averageDuration = TimeSpan.FromTicks((long)tour.TourLogs.Average(tl => tl.Duration.Ticks));
                                    var averageDistance = tour.TourLogs.Average(tl => tl.Distance);
                                    var averageRating = tour.TourLogs.Average(tl => tl.Rating);

                                    document.Add(new Paragraph($"Tour Name: {tour.Name}").SetFontSize(11).SetBold());
                                    document.Add(new Paragraph($"Description: {tour.Description}").SetFontSize(11));
                                    document.Add(new Paragraph($"From: {tour.From}, To: {tour.To}").SetFontSize(11));
                                    document.Add(new Paragraph($"Transport: {tour.TransportType}").SetFontSize(11));

                                    document.Add(new Paragraph($"Average Duration: {averageDuration}").SetFontSize(11));
                                    document.Add(new Paragraph($"Average Distance: {averageDistance:F2} km").SetFontSize(11)); // Format to 2 decimal places
                                    document.Add(new Paragraph($"Average Rating: {averageRating:F1}/5").SetFontSize(11)); // Format to 1 decimal place
                                }
                                else
                                {
                                    document.Add(new Paragraph($"Tour Name: {tour.Name}").SetFontSize(11).SetBold());
                                    document.Add(new Paragraph($"Description: {tour.Description}").SetFontSize(11));
                                    document.Add(new Paragraph($"From: {tour.From}, To: {tour.To}").SetFontSize(11));
                                    document.Add(new Paragraph($"Transport: {tour.TransportType}").SetFontSize(11));
                                    document.Add(new Paragraph("No logs available for this tour").SetFontSize(10).SetItalic());
                                }

                                document.Add(new Paragraph("\n----------------------------\n"));
                            }

                            document.Close();
                        }
                    }
                }

                logger.Debug($"Successfully generated summary report into file '{filename}'.");
            }
            catch (UnauthorizedAccessException uaEx)
            {
                logger.Error($"Access denied when generating summary report: {uaEx.Message}");
                MessageBox.Show($"Access denied: {uaEx.Message}", "Permission Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DirectoryNotFoundException dnEx)
            {
                logger.Error($"Directory not found when generating summary report: {dnEx.Message}");
                MessageBox.Show($"Directory not found: {dnEx.Message}", "Directory Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ioEx)
            {
                logger.Error($"I/O error when generating summary report: {ioEx.Message}");
                MessageBox.Show($"I/O Error: {ioEx.Message}", "I/O Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                logger.Fatal($"An unexpected error occurred when generating summary report: {ex}");
                MessageBox.Show($"An unexpected error occurred: {ex}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
