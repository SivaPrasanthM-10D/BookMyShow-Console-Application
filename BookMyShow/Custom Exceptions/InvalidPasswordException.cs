namespace BookMyShow.Custom_Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() : base(String.Format("The password should contain alteast 8 characters, a uppercase, a lowercase, a digit and a special character.")) { }
        public InvalidPasswordException(string message) : base(message) { }
    }
}
