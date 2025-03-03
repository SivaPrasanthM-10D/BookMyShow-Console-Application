namespace BookMyShow.Custom_Exceptions
{
    public class InvalidChoiceException : Exception
    {
        public InvalidChoiceException(string message) : base(message) { }
    }
}
