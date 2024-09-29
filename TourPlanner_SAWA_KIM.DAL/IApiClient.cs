using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.DAL
{
    public interface IApiClient
    {
        Task<((double latitude, double longitude) fromCoords, (double latitude, double longitude) toCoords)> GetTourCoordinatesAsync(Tour tour);
        Task<string> GetRouteRawJSONAsync(double fromLatitude, double fromLongitude, double toLatitude, double toLongitude, string transportType);
    }
}
