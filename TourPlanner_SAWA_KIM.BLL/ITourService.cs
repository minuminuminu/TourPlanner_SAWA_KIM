using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public interface ITourService
    {
        Task<IEnumerable<Tour>> GetAllToursAsync();
        Task<Tour> GetTourByIdAsync(int id);
        Task<string> GetRouteRawJSONAsync(Tour tour);
        Task<Tour> AddTourAndGeoCodesAsync(Tour tour);
        Task<Tour> ImportTour(string filename);
        Task ExportTour(Tour tour, string filename);
        Task<Tour> GetSingleTourWithLogsByIdAsync(int id);
        Task<IEnumerable<Tour>> GetAllToursWithLogs();
        Task<Tour> UpdateTourAsync(Tour tour);
        Task DeleteTourAsync(int id);
        Task<IEnumerable<TourLog>> GetTourLogsByTourIdAsync(int tourId);
        Task<TourLog> AddTourLogAsync(TourLog tourLog);
        Task<TourLog> UpdateTourLogAsync(TourLog tourLog);
        Task DeleteTourLogAsync(int id);
    }
}
