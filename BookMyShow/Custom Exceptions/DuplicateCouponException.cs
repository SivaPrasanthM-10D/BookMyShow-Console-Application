namespace BookMyShow.Custom_Exceptions
{
    public class DuplicateCouponException : Exception
    {
        public DuplicateCouponException() : base(String.Format("Coupon already exists!")) { }
        public DuplicateCouponException(string? message) : base(message)
        {
        }
    }
}
