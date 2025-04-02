
namespace BookMyShow.Implementations
{
    public class InvalidScreenNumberException : Exception
    {
        public InvalidScreenNumberException(string? message) : base(message)
        {
        }
    }
}