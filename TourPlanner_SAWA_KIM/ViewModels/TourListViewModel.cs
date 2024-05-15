using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner_SAWA_KIM.BLL;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class TourListViewModel : ViewModelBase
    {
        private ObservableCollection<Tour> _tours;
        private Tour _selectedTour;

        public ObservableCollection<Tour> Tours
        {
            get { return _tours; }
            set
            {
                _tours = value;
                RaisePropertyChangedEvent();
            }
        }

        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                _selectedTour = value;
                RaisePropertyChangedEvent();
                TourMediator.Instance.SelectedTour = _selectedTour;
            }
        }

        public TourListViewModel()
        {
            LoadDummyTours();
        }

        private void TestFunction()
        {
            Debug.WriteLine($"{SelectedTour.Name} was selected! Description: {SelectedTour.Description}");
        }

        private void LoadDummyTours()
        {
            Tours = new ObservableCollection<Tour>
            {
                new Tour("Mountain Hike", "A challenging mountain hike.", "Trailhead", "Summit", "Hiking")
                {
                    Distance = 10.5,
                    EstimatedTime = new TimeSpan(5, 0, 0),
                    RouteImage = @"C:\Images\MountainHike.png"
                },
                new Tour("City Tour", "A leisurely walk through the city.", "Main Square", "Park", "Walking")
                {
                    Distance = 3.2,
                    EstimatedTime = new TimeSpan(1, 30, 0),
                    RouteImage = @"C:\Images\CityTour.png"
                },
                new Tour("Beach Run", "A run along the beach.", "Beach Start", "Beach End", "Running")
                {
                    Distance = 5.0,
                    EstimatedTime = new TimeSpan(0, 45, 0),
                    RouteImage = @"C:\Images\BeachRun.png"
                }
            };
        }
    }
}
