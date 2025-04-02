
namespace BookMyShow.Implementations
{
    public class DuplicateSeatException : Exception
    {
        public DuplicateSeatException(string? message) : base(message)
        {
        }
    }
}