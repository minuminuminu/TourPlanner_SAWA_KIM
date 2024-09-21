using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public class PDFService
    {
        public void GenerateTourReport(string filename, Tour tour)
        {
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
            }
            catch (UnauthorizedAccessException uaEx)
            {
                MessageBox.Show($"Access denied: {uaEx.Message}", "Permission Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (DirectoryNotFoundException dnEx)
            {
                MessageBox.Show($"Directory not found: {dnEx.Message}", "Directory Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException ioEx)
            {
                MessageBox.Show($"I/O Error: {ioEx.Message}", "I/O Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.ToString()}", "Unexpected Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void GenerateSummaryReport(string filename, IEnumerable<Tour> tours)
        {
            using (PdfWriter writer = new PdfWriter(filename))
            {
                using (PdfDocument pdf = new PdfDocument(writer))
                {
                    using (Document document = new Document(pdf))
                    {
                        // Add report header
                        Paragraph header = new Paragraph("Summary Report")
                            .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                            .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                            .SetFontSize(14)
                            .SetBold();
                        document.Add(header);

                        document.Add(new Paragraph("\n"));

                        // Loop through each tour to create a summary of its logs
                        foreach (var tour in tours)
                        {
                            // Calculate averages if there are logs
                            if (tour.TourLogs != null && tour.TourLogs.Any())
                            {
                                var averageDuration = TimeSpan.FromTicks((long)tour.TourLogs.Average(tl => tl.Duration.Ticks));
                                var averageDistance = tour.TourLogs.Average(tl => tl.Distance);
                                var averageRating = tour.TourLogs.Average(tl => tl.Rating);

                                document.Add(new Paragraph($"Tour Name: {tour.Name}").SetFontSize(11).SetBold());
                                document.Add(new Paragraph($"Description: {tour.Description}").SetFontSize(11));
                                document.Add(new Paragraph($"From: {tour.From}, To: {tour.To}").SetFontSize(11));
                                document.Add(new Paragraph($"Transport: {tour.TransportType}").SetFontSize(11));

                                // Add calculated averages
                                document.Add(new Paragraph($"Average Duration: {averageDuration}").SetFontSize(11));
                                document.Add(new Paragraph($"Average Distance: {averageDistance:F2} km").SetFontSize(11)); // Format to 2 decimal places
                                document.Add(new Paragraph($"Average Rating: {averageRating:F1}/5").SetFontSize(11)); // Format to 1 decimal place
                            }
                            else
                            {
                                // Handle case when no logs are available for this tour
                                document.Add(new Paragraph($"Tour Name: {tour.Name}").SetFontSize(11).SetBold());
                                document.Add(new Paragraph($"Description: {tour.Description}").SetFontSize(11));
                                document.Add(new Paragraph($"From: {tour.From}, To: {tour.To}").SetFontSize(11));
                                document.Add(new Paragraph($"Transport: {tour.TransportType}").SetFontSize(11));
                                document.Add(new Paragraph("No logs available for this tour").SetFontSize(10).SetItalic());
                            }

                            // Add separator between tours
                            document.Add(new Paragraph("\n----------------------------\n"));
                        }

                        document.Close();
                    }
                }
            }
        }
    }
}
