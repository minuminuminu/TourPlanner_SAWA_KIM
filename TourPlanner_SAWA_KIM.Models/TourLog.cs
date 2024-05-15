using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner_SAWA_KIM.Models
{
    public class TourLog
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }
        public int Difficulty { get; set; }
        public double TotalDistance { get; set; }
        public TimeSpan TotalTime { get; set; }
        public int Rating { get; set; }

        // Constructor initializing essential properties
        public TourLog(int tourId, DateTime dateTime, string comment, int difficulty, double totalDistance, TimeSpan totalTime, int rating)
        {
            TourId = tourId;
            DateTime = dateTime;
            Comment = comment;
            Difficulty = difficulty;
            TotalDistance = totalDistance;
            TotalTime = totalTime;
            Rating = rating;
        }

        // Parameterless constructor for ORM and serialization purposes
        public TourLog() { }
    }
}
