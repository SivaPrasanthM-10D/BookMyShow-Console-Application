namespace BookMyShow.Custom_Exceptions
{
    class InvalidCouponException : Exception
    {
        public InvalidCouponException(string message) : base(message) { }
    }
}
