namespace BookMyShow.Custom_Exceptions
{
    public class InvalidSeatNoException : Exception
    {
        public InvalidSeatNoException(string message) : base(message) { }
    }
}
