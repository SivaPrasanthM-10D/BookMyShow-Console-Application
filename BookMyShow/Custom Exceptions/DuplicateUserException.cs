namespace BookMyShow.Custom_Exceptions
{
    class DuplicateUserException : Exception
    {
        public DuplicateUserException(string message) : base(message) { }
    }
}
