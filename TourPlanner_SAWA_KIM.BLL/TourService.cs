using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.DAL;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourLogRepository _tourLogRepository;

        public TourService(ITourRepository tourRepository, ITourLogRepository tourLogRepository)
        {
            _tourRepository = tourRepository;
            _tourLogRepository = tourLogRepository;
        }

        public async Task<IEnumerable<Tour>> GetAllToursAsync()
        {
            return await _tourRepository.GetAllToursAsync();
        }

        public async Task<Tour> GetTourByIdAsync(int id)
        {
            var tour = await _tourRepository.GetTourByIdAsync(id);

            if (tour == null)
            {
                throw new ArgumentException($"Tour with ID {id} not found");
            }

            return tour;
        }

        public async Task<Tour> AddTourAsync(Tour tour)
        {
            return await _tourRepository.AddTourAsync(tour);
        }

        public async Task<Tour> UpdateTourAsync(Tour tour)
        {
            return await _tourRepository.UpdateTourAsync(tour);
        }

        public async Task DeleteTourAsync(int id)
        {
            await _tourRepository.DeleteTourAsync(id);
        }

        public async Task<IEnumerable<TourLog>> GetTourLogsByTourIdAsync(int tourId)
        {
            return await _tourLogRepository.GetTourLogsByTourIdAsync(tourId);
        }

        public async Task<TourLog> AddTourLogAsync(TourLog tourLog)
        {
            return await _tourLogRepository.AddTourLogAsync(tourLog);
        }

        public async Task<TourLog> UpdateTourLogAsync(TourLog tourLog)
        {
            return await _tourLogRepository.UpdateTourLogAsync(tourLog);
        }

        public async Task DeleteTourLogAsync(int id)
        {
            await _tourLogRepository.DeleteTourLogAsync(id);
        }
    }
}
