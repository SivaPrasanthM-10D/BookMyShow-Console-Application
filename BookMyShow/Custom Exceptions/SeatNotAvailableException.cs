namespace BookMyShow.Custom_Exceptions
{
    public class SeatNotAvailableException : Exception
    {
        public SeatNotAvailableException(string message) : base(message) { }
    }
}
