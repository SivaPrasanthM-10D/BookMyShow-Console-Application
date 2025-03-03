namespace BookMyShow.Custom_Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string message) : base(message) { }
    }
}
