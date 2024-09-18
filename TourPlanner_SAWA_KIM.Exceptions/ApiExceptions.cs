namespace TourPlanner_SAWA_KIM.Exceptions
{
    public class FailedToRetrieveCoordinatesException : Exception
    {
        public FailedToRetrieveCoordinatesException(string message) : base(message)
        {
        }
    }

    public class FailedToRetrieveRouteException : Exception
    {
        public FailedToRetrieveRouteException(string message) : base(message)
        {
        }
    }
}
