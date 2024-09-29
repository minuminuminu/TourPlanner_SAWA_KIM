using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner_SAWA_KIM.Mediators;

namespace TourPlanner_SAWA_KIM.ViewModels
{
    public class SearchBarViewModel : ViewModelBase
    {
        private string _content;
        public ICommand SearchCommand { get; }
        private IMediator _mediator;

        public string Content
        {
            get { return _content; }
            set
            {
                if (_content != value)
                {
                    _content = value;
                    RaisePropertyChangedEvent(nameof(Content));
                }
            }
        }

        public SearchBarViewModel()
        {
            SearchCommand = new RelayCommand(() => Search());
        }

        private void Search()
        {
            _mediator?.Notify(this, "Search");
        }

        public void SetMediator(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}
