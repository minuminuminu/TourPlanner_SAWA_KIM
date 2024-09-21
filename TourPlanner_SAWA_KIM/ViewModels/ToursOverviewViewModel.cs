using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner_SAWA_KIM.Mediators;
using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class ToursOverviewViewModel : ViewModelBase
    {
        private Tour _selectedTour;
        private IMediator _mediator;
        private int _attributePopularity;
        private int _childFriendliness;

        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    RaisePropertyChangedEvent(nameof(SelectedTour));
                }
            }
        }

        public int AttributePopularity
        {
            get { return _attributePopularity; }
            set
            {
                if (_attributePopularity != value)
                {
                    _attributePopularity = value;
                    RaisePropertyChangedEvent(nameof(AttributePopularity));
                }
            }
        }

        public string AttributeChildFriendliness
        {
            get
            {
                return ChildFriendliness switch
                {
                    1 => "Absolutely not recommended for children",
                    2 => "Not very child friendly",
                    3 => "Moderately child friendly",
                    4 => "Child friendly",
                    5 => "Very child friendly",
                    _ => "Unknown"
                };
            }
        }

        public int ChildFriendliness
        {
            get { return _childFriendliness; }
            set
            {
                if (_childFriendliness != value)
                {
                    _childFriendliness = Math.Clamp(value, 0, 5);
                    RaisePropertyChangedEvent(nameof(ChildFriendliness));
                    RaisePropertyChangedEvent(nameof(AttributeChildFriendliness));
                }
            }
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void UpdateTourDetails(Tour tour)
        {
            SelectedTour = tour;
        }

        public void ClearTourDetails()
        {
            SelectedTour = null;
        }

        public void ComputeAttributes(ObservableCollection<TourLog> tourLogs)
        {
            ComputePopularity(tourLogs);
            ComputeChildFriendliness(tourLogs);
        }

        private void ComputePopularity(ObservableCollection<TourLog> tourLogs)
        {
            if (tourLogs != null)
            {
                int numberOfLogs = tourLogs.Count;
                AttributePopularity = MapPopularity(numberOfLogs);
            }
            else
            {
                AttributePopularity = 1;
            }
        }

        private int MapPopularity(int numberOfLogs)
        {
            if (numberOfLogs >= 9)
                return 5;
            else if (numberOfLogs >= 7)
                return 4;
            else if (numberOfLogs >= 5)
                return 3;
            else if (numberOfLogs >= 3)
                return 2;
            else if (numberOfLogs > 0)
                return 1;
            else
                return 0; // No logs, not popular
        }

        private void ComputeChildFriendliness(ObservableCollection<TourLog> tourLogs)
        {
            if (tourLogs != null && tourLogs.Any())
            {
                // difficulty levels into collection of scores 5, 3, 1
                var difficultyScores = tourLogs.Select(log => MapDifficultyToScore(log.Difficulty));
                double averageDifficultyScore = difficultyScores.Average();

                // calculate average duration in hours
                double averageDuration = tourLogs.Average(log => log.Duration.TotalHours);
                double durationScore = MapDurationToScore(averageDuration);

                // Calculate average distance
                double averageDistance = tourLogs.Average(log => log.Distance);
                double distanceScore = MapDistanceToScore(averageDistance);

                // Combine scores to compute overall child-friendliness
                double totalScore = (averageDifficultyScore + durationScore + distanceScore) / 3.0;

                ChildFriendliness = (int)Math.Round(totalScore);
            }
            else
            {
                ChildFriendliness = 0;
            }
        }

        private int MapDifficultyToScore(string difficulty)
        {
            return difficulty.ToLower() switch
            {
                "easy" => 5,
                "medium" => 3,
                "hard" => 1,
                _ => 3 // should never happen anyway
            };
        }

        private double MapDurationToScore(double averageDuration)
        {
            // shorter -> more child friendly
            if (averageDuration <= 1)
                return 5;
            else if (averageDuration <= 2)
                return 4;
            else if (averageDuration <= 3)
                return 3;
            else if (averageDuration <= 4)
                return 2;
            else
                return 1;
        }

        private double MapDistanceToScore(double averageDistance)
        {
            if (averageDistance <= 2) // in km
                return 5;
            else if (averageDistance <= 5)
                return 4;
            else if (averageDistance <= 10)
                return 3;
            else if (averageDistance <= 15)
                return 2;
            else
                return 1;
        }
    }
}
