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

        public TourService(ITourRepository tourRepository)
        {
            _tourRepository = tourRepository;
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
    }
}
