using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.DAL
{
    public class TourLogRepository : ITourLogRepository
    {
        private readonly AppDbContext _context;

        public TourLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourLog>> GetTourLogsByTourIdAsync(int tourId)
        {
            return await _context.TourLogs.Where(tl => tl.TourId == tourId).ToListAsync();
        }

        public async Task<TourLog> AddTourLogAsync(TourLog tourLog)
        {
            try
            {
                if (tourLog.Tour != null)
                {
                    _context.Tours.Attach(tourLog.Tour);
                }
                else
                {
                    var assignedTour = await _context.Tours.FindAsync(tourLog.TourId);
                    if (assignedTour == null)
                    {
                        throw new ArgumentException("Invalid TourId. The Tour does not exist.");
                    }

                    tourLog.Tour = assignedTour;
                }

                _context.TourLogs.Add(tourLog);
                await _context.SaveChangesAsync();
                return tourLog;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"An error occurred while adding the TourLog: {ex.Message}", ex);
            }
        }

        public async Task<TourLog> UpdateTourLogAsync(TourLog tourLog)
        {
            _context.TourLogs.Update(tourLog);
            await _context.SaveChangesAsync();
            return tourLog;
        }

        public async Task DeleteTourLogAsync(int id)
        {
            var tourLog = await _context.TourLogs.FindAsync(id);

            if (tourLog == null)
            {
                throw new ArgumentException($"TourLog with ID {id} not found");
            }

            _context.TourLogs.Remove(tourLog);
            await _context.SaveChangesAsync();
        }
    }
}
