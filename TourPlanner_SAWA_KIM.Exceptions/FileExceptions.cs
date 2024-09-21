using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlanner_SAWA_KIM.Exceptions
{
    public class FailedToImportTourException : Exception
    {
        public FailedToImportTourException(string message) : base(message)
        {
        }
    }

    public class FailedToExportTourException : Exception
    {
        public FailedToExportTourException(string message) : base(message)
        {
        }
    }
}
