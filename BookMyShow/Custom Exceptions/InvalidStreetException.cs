
namespace BookMyShow.Implementations
{
    public class InvalidStreetException : Exception
    {
        public InvalidStreetException(string? message) : base(message)
        {
        }
    }
}