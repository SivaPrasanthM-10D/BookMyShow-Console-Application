namespace BookMyShow.Custom_Exceptions
{
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException() : base(String.Format("Movie not found. Please add the movie first."))
        {
            
        }
        public MovieNotFoundException(string message) : base(message) { }
    }
}
