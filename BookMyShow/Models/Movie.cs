namespace BookMyShow.Models
{
    public class Movie
    {
        public string Title;
        public string Genre;
        public int Duration;

        public Movie(string title, string genre, int duration)
        {
            Title = title;
            Genre = genre;
            Duration = duration;
        }
    }
}
