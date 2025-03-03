namespace BookMyShow.Custom_Exceptions
{
    public class InvalidMovieException : Exception
    {
        public InvalidMovieException(string message) : base(message) { }
    }
}
