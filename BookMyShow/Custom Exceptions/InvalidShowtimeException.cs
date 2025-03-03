namespace BookMyShow.Custom_Exceptions
{
    public class InvalidShowtimeException : Exception
    {
        public InvalidShowtimeException(string message) : base(message) { }
    }
}
