
namespace BookMyShow.Implementations
{
    public class InvalidShowDateException : Exception
    {
        public InvalidShowDateException(string? message) : base(message)
        {
        }
    }
}