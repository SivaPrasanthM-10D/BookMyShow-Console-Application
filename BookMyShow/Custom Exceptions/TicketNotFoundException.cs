namespace BookMyShow.Custom_Exceptions
{
    public class TicketNotFoundException : Exception
    {
        public TicketNotFoundException(string message) : base(message) { }
    }
}
