using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.DAL
{
    public interface ITourLogRepository
    {
        Task<IEnumerable<TourLog>> GetTourLogsByTourIdAsync(int tourId);
        Task<TourLog> AddTourLogAsync(TourLog tourLog);
        Task<TourLog> UpdateTourLogAsync(TourLog tourLog);
        Task DeleteTourLogAsync(int id);
    }
}
