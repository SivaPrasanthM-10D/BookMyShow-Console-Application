namespace BookMyShow.Custom_Exceptions
{
    public class InvalidChoiceException : Exception
    {
        public InvalidChoiceException() : base(String.Format("Invalid choice.")) { }
        public InvalidChoiceException(string message) : base(message) { }
    }
}
