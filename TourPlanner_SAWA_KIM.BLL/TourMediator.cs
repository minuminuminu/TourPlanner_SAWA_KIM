using TourPlanner_SAWA_KIM.Models;

namespace TourPlanner_SAWA_KIM.BLL
{
    public class TourMediator
    {
        private static TourMediator _instance;

        private Tour _selectedTour;
        public event Action<Tour> TourSelected;

        private TourMediator() { }

        public static TourMediator Instance => _instance ??= new TourMediator();

        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    TourSelected?.Invoke(_selectedTour);
                }
            }
        }
    }
}
