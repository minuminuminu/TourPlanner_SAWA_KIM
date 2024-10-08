﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Exceptions;
using TourPlanner_SAWA_KIM.Logging;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.DAL
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private ILoggerWrapper logger = LoggerFactory.GetLogger();

        public ApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:OpenRouteServiceApiKey"];
        }

        private async Task<(double latitude, double longitude)> GetCoordinatesAsync(string address)
        {
            // for special characters in the address
            var encodedAddress = Uri.EscapeDataString(address);
            var requestUrl = $"https://api.openrouteservice.org/geocode/search?api_key={_apiKey}&text={encodedAddress}";

            var response = await _httpClient.GetAsync(requestUrl);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(content);
                var coordinates = json["features"]?[0]?["geometry"]?["coordinates"];

                if (coordinates != null)
                {
                    double longitude = coordinates[0].Value<double>();
                    double latitude = coordinates[1].Value<double>();

                    return (latitude, longitude); // tuple, so we can return 2 values
                }
            }

            logger.Error("Failed to retrieve coordinates");
            throw new FailedToRetrieveCoordinatesException("Failed to retrieve coordinates");
        }

        public async Task<((double latitude, double longitude) fromCoords, (double latitude, double longitude) toCoords)> GetTourCoordinatesAsync(Tour tour)
        {
            // from
            var fromCoords = await GetCoordinatesAsync(tour.From);

            // to
            var toCoords = await GetCoordinatesAsync(tour.To);

            // tuple return
            return (fromCoords, toCoords);
        }

        public async Task<string> GetRouteRawJSONAsync(double fromLatitude, double fromLongitude, double toLatitude, double toLongitude, string transportType)
        {
            var requestUrl = $"https://api.openrouteservice.org/v2/directions/{transportType}?api_key={_apiKey}&start={fromLongitude.ToString(CultureInfo.InvariantCulture)},{fromLatitude.ToString(CultureInfo.InvariantCulture)}&end={toLongitude.ToString(CultureInfo.InvariantCulture)},{toLatitude.ToString(CultureInfo.InvariantCulture)}";
            Debug.WriteLine($"Request URL: {requestUrl}");

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Route retrieved successfully");
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    logger.Error($"Route retrieval failed with status code: {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Route retrieval failed with status code: {response.StatusCode}, Response content: {errorContent}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"HTTP Request error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An unexpected error occurred: {ex.Message}");
                Debug.WriteLine(ex.StackTrace); // Log stack trace for more details
            }

            Debug.WriteLine("API Client Route Failed");
            throw new FailedToRetrieveRouteException("Failed to retrieve route");
        }
    }
}
