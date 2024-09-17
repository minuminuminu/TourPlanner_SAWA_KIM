using System;
using System.ComponentModel;
using System.Diagnostics;

namespace TourPlanner_SAWA_KIM.Models
{
    public class Tour : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _from;
        private string _to;
        private string _transportType;
        private double _distance;
        private TimeSpan _estimatedTime;
        private string _routeImage = string.Empty;

        public int Id { get; set; }
        public ICollection<TourLog> TourLogs { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string From
        {
            get => _from;
            set
            {
                if (_from != value)
                {
                    _from = value;
                    OnPropertyChanged(nameof(From));
                }
            }
        }

        public string To
        {
            get => _to;
            set
            {
                if (_to != value)
                {
                    _to = value;
                    OnPropertyChanged(nameof(To));
                }
            }
        }

        public string TransportType
        {
            get => _transportType;
            set
            {
                if (_transportType != value)
                {
                    _transportType = value;
                    OnPropertyChanged(nameof(TransportType));
                }
            }
        }

        public double Distance
        {
            get => _distance;
            set
            {
                if (_distance != value)
                {
                    _distance = value;
                    OnPropertyChanged(nameof(Distance));
                }
            }
        }

        public TimeSpan EstimatedTime
        {
            get => _estimatedTime;
            set
            {
                if (_estimatedTime != value)
                {
                    _estimatedTime = value;
                    OnPropertyChanged(nameof(EstimatedTime));
                }
            }
        }

        public string RouteImage
        {
            get => _routeImage;
            set
            {
                if (_routeImage != value)
                {
                    _routeImage = value;
                    OnPropertyChanged(nameof(RouteImage));
                }
            }
        }

        // Constructor initializing essential properties
        public Tour(string name, string description, string from, string to, string transportType)
        {
            _name = name;
            _description = description;
            _from = from;
            _to = to;
            _transportType = transportType;
        }

        // Parameterless constructor for ORM and serialization purposes
        public Tour() { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
