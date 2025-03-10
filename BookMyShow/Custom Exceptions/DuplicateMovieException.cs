
namespace BookMyShow.Implementations
{
    public class DuplicateMovieException : Exception
    {
        public DuplicateMovieException(string? message) : base(message)
        {
        }
    }
}