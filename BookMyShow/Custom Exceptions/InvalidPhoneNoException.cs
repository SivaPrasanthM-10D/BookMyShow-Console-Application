namespace BookMyShow.Custom_Exceptions
{
    class InvalidPhoneNoException : Exception
    {
        public InvalidPhoneNoException(string message) : base(message) { }
    }
}
