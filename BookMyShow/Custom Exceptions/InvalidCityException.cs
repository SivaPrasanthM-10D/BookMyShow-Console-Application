
namespace BookMyShow.Implementations
{
    public class InvalidCityException : Exception
    {
        public InvalidCityException(string? message) : base(message)
        {
        }
    }
}