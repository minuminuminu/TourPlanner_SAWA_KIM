using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.DAL;
using TourPlanner_SAWA_KIM.Exceptions;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourLogRepository _tourLogRepository;
        private readonly IApiClient _apiClient;

        public TourService(ITourRepository tourRepository, ITourLogRepository tourLogRepository, IApiClient apiClient)
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

        private OpenRouteServiceResponse GetRouteParsedJSONAsync(string rawJSON)
        {
            return JsonSerializer.Deserialize<OpenRouteServiceResponse>(rawJSON);
        }

        private void SetDistanceAndDuration(Tour tour, OpenRouteServiceResponse routeObjectJSON)
        {
            var distance = routeObjectJSON.Features?[0]?.Properties?.Segments?[0]?.Distance;
            var duration = routeObjectJSON.Features?[0]?.Properties?.Segments?[0]?.Duration;

            if (distance == null || duration == null)
            {
                Debug.WriteLine("Distance or duration was null");
                Debug.WriteLine($"Distance: {distance}, Duration: {duration}");
                throw new FailedToRetrieveRouteException("Failed to retrieve route");
            }

            tour.Distance = (double)(distance / 1000);  // meter to km
            tour.EstimatedTime = TimeSpan.FromSeconds((double)duration); // seconds to TimeSpan C#
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
            if(tour == null)
            {
                throw new FailedToImportTourException("Tour is null");
            }

            await SetTourGeoDataAsync(tour);

            return await AddTourAsync(tour);
        }

        public async Task<Tour> ImportTour(string filename)
        {
            using FileStream openStream = File.OpenRead(filename);

            try
            {
                Tour? tourFromFile = await JsonSerializer.DeserializeAsync<Tour>(openStream);
                if(tourFromFile == null)
                {
                    throw new FailedToImportTourException("Failed to import tour");
                }

                var importedTour = await AddTourAndGeoCodesAsync(tourFromFile);

                return importedTour;
            }
            catch (Exception ex)
            {
                throw new FailedToImportTourException($"Failed to import tour: {ex.Message}");
            }
        }

        public async Task ExportTour(Tour tour, string filename)
        {
            if(tour == null)
            {
                throw new FailedToExportTourException("Tour is null");
            }

            var associatedTourLogs = await _tourLogRepository.GetTourLogsByTourIdAsync(tour.Id);
            tour.ImportedTourLogsList = associatedTourLogs;

            await using FileStream createStream = File.Create(filename);
            await JsonSerializer.SerializeAsync(createStream, tour);
        }

        public async Task<Tour> GetSingleTourWithLogsByIdAsync(int id)
        {
            var tour = await _tourRepository.GetSingleTourWithLogsByIdAsync(id);
            if (tour == null)
            {
                throw new ArgumentException($"Tour with ID {id} not found");
            }
            return tour;
        }

        public async Task<IEnumerable<Tour>> GetAllToursWithLogs()
        {
            return await _tourRepository.GetAllToursWithLogs();
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
