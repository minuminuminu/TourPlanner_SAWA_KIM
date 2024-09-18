using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.DAL;
using TourPlanner_SAWA_KIM.Exceptions;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public class TourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourLogRepository _tourLogRepository;
        private readonly ApiClient _apiClient;

        public TourService(ITourRepository tourRepository, ITourLogRepository tourLogRepository, ApiClient apiClient)
        {
            _tourRepository = tourRepository;
            _tourLogRepository = tourLogRepository;
            _apiClient = apiClient;
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

        private async Task<((double latitude, double longitude) fromCoords, (double latitude, double longitude) toCoords)> GetGeoCodeByTourAsync(Tour tour)
        {
            var (fromCoords, toCoords) = await _apiClient.GetTourCoordinatesAsync(tour);
            return (fromCoords, toCoords);
        }

        private async Task<Tour> AddTourAsync(Tour tour)
        {
            return await _tourRepository.AddTourAsync(tour);
        }

        private void SetGeoCodes(Tour tour, (double latitude, double longitude) from, (double latitude, double longitude) to)
        {
            tour.FromLatitude = from.latitude;
            tour.FromLongitude = from.longitude;

            tour.ToLatitude = to.latitude;
            tour.ToLongitude = to.longitude;
        }

        private JObject GetRouteParsedJSONAsync(string rawJSON)
        {
            return JObject.Parse(rawJSON);
        }

        private void SetDistanceAndDuration(Tour tour, JObject routeObjectJSON)
        {
            var distance = routeObjectJSON["features"]?[0]?["properties"]?["segments"]?[0]?["distance"]?.Value<double>();
            var duration = routeObjectJSON["features"]?[0]?["properties"]?["segments"]?[0]?["duration"]?.Value<double>();

            if (distance == null || duration == null)
            {
                Debug.WriteLine("Distance or duration was null");
                Debug.WriteLine($"Distance: {distance.Value}, Duration: {duration.Value}");
                throw new FailedToRetrieveRouteException("Failed to retrieve route");
            }

            tour.Distance = distance.Value / 1000;  // meter to km
            tour.EstimatedTime = TimeSpan.FromSeconds(duration.Value); // seconds to TimeSpan C#
        }

        public async Task<string> GetRouteRawJSONAsync(Tour tour)
        {
            return await _apiClient.GetRouteRawJSONAsync(tour.FromLatitude, tour.FromLongitude, tour.ToLatitude, tour.ToLongitude, tour.TransportType);
        }

        private async Task SetTourGeoDataAsync(Tour tour)
        {
            var (from, to) = await GetGeoCodeByTourAsync(tour);
            SetGeoCodes(tour, from, to);

            var rawJSON = await GetRouteRawJSONAsync(tour);
            var routeObjectJSON = GetRouteParsedJSONAsync(rawJSON);

            SetDistanceAndDuration(tour, routeObjectJSON);
        }

        public async Task<Tour> AddTourAndGeoCodesAsync(Tour tour)
        {
            await SetTourGeoDataAsync(tour);

            return await AddTourAsync(tour);
        }

        public async Task<Tour> UpdateTourAsync(Tour tour)
        {
            await SetTourGeoDataAsync(tour);

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
