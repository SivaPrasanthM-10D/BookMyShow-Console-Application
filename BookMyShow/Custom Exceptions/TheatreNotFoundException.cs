namespace BookMyShow.Custom_Exceptions
{
    public class TheatreNotFoundException : Exception
    {
        public TheatreNotFoundException() : base(String.Format("Theatre not found. Please add the theatre first.")) { }
        public TheatreNotFoundException(string? message) : base(message)
        {
        }
    }
}
