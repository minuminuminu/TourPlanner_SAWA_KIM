using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public interface IPDFService
    {
        void GenerateTourReport(string filename, Tour tour);
        void GenerateSummaryReport(string filename, IEnumerable<Tour> tours);
        
    }
}
