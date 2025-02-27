namespace BookMyShow.Custom_Exceptions
{
    class InvalidUpiException : Exception
    {
        public InvalidUpiException(string message) : base(message) { }
    }
}
