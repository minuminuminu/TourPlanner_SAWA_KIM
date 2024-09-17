//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace TourPlanner_SAWA_KIM.Models
//{
//    public class Tour
//    {
//        public int Id { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public string From { get; set; }
//        public string To { get; set; }
//        public string TransportType { get; set; }
//        public double Distance { get; set; }
//        public TimeSpan EstimatedTime { get; set; }
//        public string RouteImage { get; set; } = string.Empty;

//        // Constructor initializing essential properties
//        public Tour(string name, string description, string from, string to, string transportType)
//        {
//            Name = name;
//            Description = description;
//            From = from;
//            To = to;
//            TransportType = transportType;
//        }

//        // Parameterless constructor for ORM and serialization purposes
//        public Tour() { }
//    }
//}


using System;
using System.ComponentModel;

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
            Name = name;
            Description = description;
            From = from;
            To = to;
            TransportType = transportType;
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
