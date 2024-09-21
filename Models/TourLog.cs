using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TourPlanner_SAWA_KIM.Models
{
    public class TourLog : INotifyPropertyChanged
    {
        private int _rating;
        private DateTime _date;
        private TimeSpan _duration;
        private double _distance;
        private string _difficulty;
        private string _comment;

        public int TourId { get; set; }

        [JsonIgnore]
        public Tour Tour { get; set; }
        [JsonIgnore]
        public int Id { get; set; }

        public int Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    if (value.Kind == DateTimeKind.Unspecified)
                    {
                        _date = DateTime.SpecifyKind(value, DateTimeKind.Utc);
                    }
                    else
                    {
                        _date = value;
                    }
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        public TimeSpan Duration
        {
            get { return _duration; }
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    OnPropertyChanged(nameof(Duration));
                }
            }
        }

        public double Distance
        {
            get { return _distance; }
            set
            {
                if (_distance != value)
                {
                    _distance = value;
                    OnPropertyChanged(nameof(Distance));
                }
            }
        }

        public string Difficulty
        {
            get { return _difficulty; }
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    OnPropertyChanged(nameof(Difficulty));
                }
            }
        }

        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_comment != value)
                {
                    _comment = value;
                    OnPropertyChanged(nameof(Comment));
                }
            }
        }

        public TourLog(int rating, DateTime date, TimeSpan duration, double distance, string difficulty, string comment)
        {
            _rating = rating;
            _date = date;
            _duration = duration;
            _distance = distance;
            _difficulty = difficulty;
            _comment = comment;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
