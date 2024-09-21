using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public class PDFService
    {
        public void GenerateTourReport(string filename, Tour tour, IEnumerable<TourLog> tourLogs)
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

                        foreach (var tourlog in tourLogs)
                        {
                            document.Add(new Paragraph($"Date: {tourlog.Date.ToShortDateString()}").SetFontSize(10).SetBold());
                            document.Add(new Paragraph($"Duration: {tourlog.Duration}").SetFontSize(10));
                            document.Add(new Paragraph($"Distance: {tourlog.Distance} km").SetFontSize(10));
                            document.Add(new Paragraph($"Difficulty: {tourlog.Difficulty}").SetFontSize(10));
                            document.Add(new Paragraph($"Rating: {tourlog.Rating}/5").SetFontSize(10));
                            document.Add(new Paragraph($"Comment: {tourlog.Comment}").SetFontSize(10));

                            document.Add(new Paragraph("\n----------------------------\n"));
                        }

                        if (!tourLogs.Any())
                        {
                            document.Add(new Paragraph("No tour logs available for this tour.").SetFontSize(10).SetItalic());
                        }

                        document.Close();
                    }
                }
            }
        }

        public void GenerateSummaryReport(string filename, IEnumerable<Tour> tours, IEnumerable<TourLog> tourLogs)
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
                            var tourLog = tourLogs.FirstOrDefault(tl => tl.TourId == tour.Id);

                            document.Add(new Paragraph($"Tour Name: {tour.Name}").SetFontSize(11).SetBold());
                            document.Add(new Paragraph($"Description: {tour.Description}").SetFontSize(11));
                            document.Add(new Paragraph($"From: {tour.From}, To: {tour.To}").SetFontSize(11));
                            document.Add(new Paragraph($"Transport: {tour.TransportType}").SetFontSize(11));

                            if (tourLog != null)
                            {
                                document.Add(new Paragraph($"Average Duration: {tourLog.Duration}").SetFontSize(11));
                                document.Add(new Paragraph($"Average Distance: {tourLog.Distance} km").SetFontSize(11));
                                document.Add(new Paragraph($"Average Rating: {tourLog.Rating}/5").SetFontSize(11));
                            }
                            else
                            {
                                document.Add(new Paragraph("No logs available for this tour").SetFontSize(10).SetItalic());
                            }

                            document.Add(new Paragraph("\n----------------------------\n"));
                        }

                        document.Close();
                    }
                }
            }
        }
    }
}
