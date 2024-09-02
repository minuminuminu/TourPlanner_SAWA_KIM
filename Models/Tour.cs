using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner_SAWA_KIM.Models
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string TransportType { get; set; }
        public double Distance { get; set; }
        public TimeSpan EstimatedTime { get; set; }
        public string RouteImage { get; set; } = string.Empty;

        // Constructor initializing essential properties
        public Tour(string name, string description, string from, string to, string transportType)
        {
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
        }

        // Parameterless constructor for ORM and serialization purposes
        public Tour() { }
    }
}
