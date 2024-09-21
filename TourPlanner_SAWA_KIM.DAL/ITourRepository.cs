using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.DAL
{
    public interface ITourRepository
    {
        Task<IEnumerable<Tour>> GetAllToursAsync();
        Task<Tour?> GetTourByIdAsync(int id);
        Task<Tour> AddTourAsync(Tour tour);
        Task<Tour> UpdateTourAsync(Tour tour);
        Task DeleteTourAsync(int id);
        Task<Tour?> GetSingleTourWithLogsByIdAsync(int id);
        Task<IEnumerable<Tour>> GetAllToursWithLogs();
    }
}
