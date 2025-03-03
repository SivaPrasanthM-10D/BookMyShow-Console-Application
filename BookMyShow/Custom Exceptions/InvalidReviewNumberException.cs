namespace BookMyShow.Custom_Exceptions
{
    public class InvalidReviewNumberException : Exception
    {
        public InvalidReviewNumberException(string message) : base(message) { }
    }
}
