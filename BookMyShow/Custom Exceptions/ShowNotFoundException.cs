namespace BookMyShow.Custom_Exceptions
{
    public class ShowNotFoundException : Exception
    {
        public ShowNotFoundException(string message) : base(message) { }
    }
}