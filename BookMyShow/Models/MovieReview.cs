
namespace BookMyShow.Models
{
    public class MovieReview
    {
        public string CustomerName;
        public string MovieTitle;
        public double Rating;
        public string Review;

        public MovieReview(string name, string movieTitle, double rating, string review)
        {
            CustomerName = name;
            MovieTitle = movieTitle;
            Rating = rating;
            Review = review;
        }
    }
}
