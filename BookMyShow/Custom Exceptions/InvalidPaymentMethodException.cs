namespace BookMyShow.Custom_Exceptions
{
    public class InvalidPaymentMethodException : Exception
    {
        public InvalidPaymentMethodException() 
            : base(String.Format("Invalid payment method. Try again")) { }
    }
}
