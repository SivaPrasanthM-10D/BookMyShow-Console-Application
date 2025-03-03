using BookMyShow.Models;
using BookMyShow.Custom_Exceptions;
namespace BookMyShow.Implementations
{
    public class ReviewSystem
    {
        private static List<MovieReview> Reviews = new List<MovieReview>();
        private static Dictionary<string, double> moviewithrating = new Dictionary<string, double>();

        private static void WriteCentered(string text)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = text.Length;
            int spaces = Math.Max((windowWidth - textLength) / 2, 0);
            Console.WriteLine(new string(' ', spaces) + text);
        }

        private static string ReadCentered(string prompt)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = prompt.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.Write(new string(' ', spaces) + prompt);
            return Console.ReadLine();
        }

        public void DisplayMovies()
        {
            try
            {
                var movies = AdminOperations.GetMovies();
                var availableMovies = movies.Select(movie => movie.Title).ToList();

                if (availableMovies.Count == 0)
                {
                    throw new MovieNotFoundException("No movies available.");
                }

                moviewithrating.Clear();
                foreach (var review in Reviews)
                {
                    if (!moviewithrating.ContainsKey(review.MovieTitle))
                    {
                        moviewithrating[review.MovieTitle] = 0;
                    }
                    moviewithrating[review.MovieTitle] += review.Rating;
                }

                var movieReviewCounts = Reviews.GroupBy(r => r.MovieTitle)
                                               .ToDictionary(g => g.Key, g => g.Count());

                int consoleWidth = Console.WindowWidth;
                int maxTitleLength = availableMovies.Any() ? availableMovies.Max(title => title.Length) : 10;
                string format = "{0,-30}{1,-30}{2,-30}{3,-30}{4,-30}";
                string separator = new string('-', Math.Max(consoleWidth, 150)); // Ensure separator length is non-negative

                WriteCentered(separator);
                WriteCentered(string.Format(format, "Movie 1", "Movie 2", "Movie 3", "Movie 4", "Movie 5"));
                WriteCentered(separator);

                for (int i = 0; i < availableMovies.Count; i += 5)
                {
                    var rowMovies = availableMovies.Skip(i).Take(5).ToList();
                    WriteCentered(string.Format(format,
                        rowMovies.ElementAtOrDefault(0) ?? "",
                        rowMovies.ElementAtOrDefault(1) ?? "",
                        rowMovies.ElementAtOrDefault(2) ?? "",
                        rowMovies.ElementAtOrDefault(3) ?? "",
                        rowMovies.ElementAtOrDefault(4) ?? ""));
                    WriteCentered(string.Format(format,
                        rowMovies.ElementAtOrDefault(0) != null && moviewithrating.TryGetValue(rowMovies.ElementAtOrDefault(0), out double r1) ? (r1 / movieReviewCounts[rowMovies.ElementAtOrDefault(0)]).ToString("0.0") + "/5" : "",
                        rowMovies.ElementAtOrDefault(1) != null && moviewithrating.TryGetValue(rowMovies.ElementAtOrDefault(1), out double r2) ? (r2 / movieReviewCounts[rowMovies.ElementAtOrDefault(1)]).ToString("0.0") + "/5" : "",
                        rowMovies.ElementAtOrDefault(2) != null && moviewithrating.TryGetValue(rowMovies.ElementAtOrDefault(2), out double r3) ? (r3 / movieReviewCounts[rowMovies.ElementAtOrDefault(2)]).ToString("0.0") + "/5" : "",
                        rowMovies.ElementAtOrDefault(3) != null && moviewithrating.TryGetValue(rowMovies.ElementAtOrDefault(3), out double r4) ? (r4 / movieReviewCounts[rowMovies.ElementAtOrDefault(3)]).ToString("0.0") + "/5" : "",
                        rowMovies.ElementAtOrDefault(4) != null && moviewithrating.TryGetValue(rowMovies.ElementAtOrDefault(4), out double r5) ? (r5 / movieReviewCounts[rowMovies.ElementAtOrDefault(4)]).ToString("0.0") + "/5" : ""));
                    WriteCentered(separator);
                }
            }
            catch (MovieNotFoundException e)
            {
                WriteCentered(e.Message);
            }
            catch (Exception e)
            {
                WriteCentered("An unexpected error occurred: " + e.Message);
            }
        }

        public static void AddReview(Customer customer)
        {
            try
            {
                var movies = AdminOperations.GetMovies();
                var availableMovies = movies.Select(movie => movie.Title).ToList();

                string moviename = ReadCentered("Enter movie name: ");

                if (!availableMovies.Contains(moviename))
                {
                    throw new InvalidMovieException("The movie is not available for review.");
                }

                if (Reviews.Any(r => r.CustomerName == customer.Name && r.MovieTitle == moviename))
                {
                    WriteCentered("You have already reviewed this movie. Please update your review instead.");
                    return;
                }

                string ratingInput = ReadCentered("Rate the movie (1-5): ");
                if (!double.TryParse(ratingInput, out double rating) || rating < 1 || rating > 5)
                {
                    throw new InvalidRatingException("Invalid rating. Must be between 1 and 5.");
                }

                string review = ReadCentered("Enter your review: ");
                Reviews.Add(new MovieReview(customer.Name, moviename, rating, review));
                WriteCentered("Thank you for your review!");
            }
            catch (InvalidMovieException e)
            {
                WriteCentered(e.Message);
            }
            catch (InvalidRatingException e)
            {
                WriteCentered(e.Message);
            }
            catch (Exception e)
            {
                WriteCentered("An unexpected error occurred: " + e.Message);
            }
        }

        public static void UpdateReview(Customer customer)
        {
            try
            {
                if (Reviews.Count == 0)
                {
                    throw new ReviewNotFoundException("No reviews found.");
                }

                string moviename = ReadCentered("Enter movie name to update review: ");
                var existingReview = Reviews.FirstOrDefault(r => r.CustomerName == customer.Name && r.MovieTitle == moviename);

                if (existingReview == null)
                {
                    WriteCentered("No existing review found for this movie.");
                    return;
                }

                string ratingInput = ReadCentered("Update your rating (1-5): ");
                if (!double.TryParse(ratingInput, out double rating) || rating < 1 || rating > 5)
                {
                    throw new InvalidRatingException("Invalid rating. Must be between 1 and 5.");
                }

                string review = ReadCentered("Update your review: ");
                existingReview.Rating = rating;
                existingReview.Review = review;
                WriteCentered("Your review has been updated successfully!");
            }
            catch (ReviewNotFoundException e)
            {
                WriteCentered(e.Message);
            }
            catch (InvalidRatingException e)
            {
                WriteCentered(e.Message);
            }
            catch (Exception e)
            {
                WriteCentered("An unexpected error occurred: " + e.Message);
            }
        }

        public static void RemoveReview()
        {
            try
            {
                if (Reviews.Count == 0)
                {
                    throw new ReviewNotFoundException("No reviews found.");
                }
                for (int i = 0; i < Reviews.Count; i++)
                {
                    WriteCentered($"{i + 1}. {Reviews[i].CustomerName}\n{Reviews[i].MovieTitle} - {Reviews[i].Rating}/5\n{Reviews[i].Review}\n");
                }
                string revnoInput = ReadCentered("Enter the review number to remove:");
                if (!int.TryParse(revnoInput, out int revno) || revno < 1 || revno > Reviews.Count)
                {
                    WriteCentered("Invalid review number. Try again.");
                    return;
                }
                Reviews.RemoveAt(revno - 1);
                WriteCentered("Review removed successfully!");
            }
            catch (ReviewNotFoundException e)
            {
                WriteCentered(e.Message);
                return;
            }
            catch (InvalidReviewNumberException e)
            {
                WriteCentered(e.Message);
                return;
            }

        }

        public static void ViewReview()
        {
            try
            {
                if (Reviews.Count == 0)
                {
                    throw new ReviewNotFoundException("No reviews found.");
                }
                foreach (var review in Reviews)
                {
                    WriteCentered($"\n{review.CustomerName}:");
                    WriteCentered($"\n{review.MovieTitle} - {review.Rating}/5");
                    WriteCentered($"{review.Review}");
                }
            }
            catch (ReviewNotFoundException e)
            {
                WriteCentered(e.Message);
                return;
            }

        }
    }
}