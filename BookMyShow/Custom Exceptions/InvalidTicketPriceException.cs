
namespace BookMyShow.Implementations
{
    public class InvalidTicketPriceException : Exception
    {
        public InvalidTicketPriceException(string? message) : base(message)
        {
        }
    }
}