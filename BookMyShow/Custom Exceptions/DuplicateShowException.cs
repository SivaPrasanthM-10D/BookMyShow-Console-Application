namespace BookMyShow.Custom_Exceptions
{
    public class DuplicateShowException : Exception
    {
        public DuplicateShowException(string message) : base(message) { }
    }
}
