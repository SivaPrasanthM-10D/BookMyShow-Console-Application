namespace BookMyShow.Custom_Exceptions
{
    public class InvalidTheatreNameException : Exception
    {
        public InvalidTheatreNameException(string message) : base(message) { }
    }
}
