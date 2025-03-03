namespace BookMyShow.Custom_Exceptions
{
    public class ReviewNotFoundException : Exception
    {
        public ReviewNotFoundException(string message) : base(message) { }
    }
}
