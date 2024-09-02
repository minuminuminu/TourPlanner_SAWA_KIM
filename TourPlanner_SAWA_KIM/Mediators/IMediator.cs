using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner_SAWA_KIM.Mediators
{
    public interface IMediator
    {
        // https://refactoring.guru/design-patterns/mediator/csharp/example
        void Notify(object sender, string eventName);
    }
}
