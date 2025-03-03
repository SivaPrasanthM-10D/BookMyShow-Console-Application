namespace BookMyShow.Custom_Exceptions
{
    class PasswordNotMatchException : Exception
    {
        public PasswordNotMatchException() : base(String.Format("Password does not match.")) { }
        public PasswordNotMatchException(string? message) : base(message)
        {
        }
    }
}
