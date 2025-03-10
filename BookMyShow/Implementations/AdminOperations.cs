using BookMyShow.Custom_Exceptions;
using BookMyShow.Models;
namespace BookMyShow.Implementations
{
    public static class AdminOperations
    {
        private static List<Movie> Movies = new List<Movie>();
        private static List<Theatre> Theatres = new List<Theatre>();
        private static Dictionary<string, double> Coupons = new Dictionary<string, double>();

        private static void WriteCentered(string text)
        {
            int windowWidth = 168;
            int textLength = text.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }

        private static string ReadCentered(string prompt)
        {
            int windowWidth = 168;
            int textLength = prompt.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.Write(new string(' ', spaces) + prompt);
            return Console.ReadLine();
        }

        public static void AddMovie(string title, string genre, int duration)
        {
            try
            {
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(genre) || duration <= 0)
                {
                    throw new ArgumentException("Invalid movie details provided.");
                }
                Movies.Add(new Movie(title, genre, duration));
                Console.WriteLine();
                WriteCentered("Movie added successfully!");
            }
            catch (Exception ex)
            {
                WriteCentered($"Error: {ex.Message}");
            }
        }

        public static void AddTheatre(TheatreOwner theatreOwner, string name, string city, string street, int numScreens)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(street) || numScreens <= 0)
                {
                    throw new ArgumentException("Invalid theatre details provided.");
                }
                Theatre theatre = new Theatre(name, city, street);
                for (int i = 1; i <= numScreens; i++)
                {
                    theatre.Screens.Add(new Screen(i));
                }
                theatreOwner.OwnedTheatre = theatre;
                Console.WriteLine();
                WriteCentered("Theatre added successfully!");
            }
            catch (Exception ex)
            {
                WriteCentered($"Error: {ex.Message}");
                return;
            }
        }

        public static void RemoveTheatre(string theatreName)
        {
            try
            {
                Theatre? theatre = GetTheatres().Find(t => t.Name.Equals(theatreName, StringComparison.OrdinalIgnoreCase));
                if (theatre == null)
                {
                    throw new TheatreNotFoundException("Theatre not found.");
                }

                foreach (var screen in theatre.Screens)
                {
                    foreach (var show in screen.Shows)
                    {
                        BookingSystem.RemoveBookings(show);
                    }
                }

                GetTheatres().Remove(theatre);
                foreach (var owner in UserManagement.GetTheatreOwners())
                {
                    if (owner.OwnedTheatre == theatre)
                    {
                        owner.OwnedTheatre = null;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteCentered($"Error: {ex.Message}");
                return;
            }
        }

        public static List<Movie> GetMovies()
        {
            return Movies;
        }

        public static List<Theatre> GetTheatres()
        {
            List<Theatre> updatedTheatres = new List<Theatre>();

            foreach (var owner in UserManagement.GetTheatreOwners())
            {
                if (owner.OwnedTheatre != null)
                {
                    if (!updatedTheatres.Any(t => t.Name == owner.OwnedTheatre.Name && t.City == owner.OwnedTheatre.City))
                    {
                        updatedTheatres.Add(owner.OwnedTheatre);
                    }
                }
            }

            return updatedTheatres;
        }


        public static List<Show> GetShows()
        {
            List<Show> shows = new List<Show>();
            foreach (Theatre theatre in GetTheatres().Distinct())
            {
                foreach (Screen screen in theatre.Screens)
                {
                    shows.AddRange(screen.Shows);
                }
            }
            return shows;
        }

        public static Dictionary<string, double> GetCoupons()
        {
            return Coupons;
        }

        public static bool ShowExists(TheatreOwner theatreOwner, string theatreName, int screenNo, DateTime showTime, DateTime showDate)
        {
            try
            {
                Screen? screen = theatreOwner.OwnedTheatre.Screens.Find(s => s.ScreenNumber == screenNo);
                if (screen == null)
                {
                    throw new ScreenNotFoundException(screenNo, theatreOwner.OwnedTheatre.Name);
                }

                return screen.Shows.Any(s => s.ShowTime == showTime.ToString("hh:mm tt") && s.ShowDate == showDate.ToString("dd/MM/yyyy"));
            }
            catch (ScreenNotFoundException ex)
            {
                WriteCentered(ex.Message);
                return false;
            }
        }

        public static void AddShow(TheatreOwner theatreOwner, string theatreName, int screenNo, string movieTitle, DateTime showTime, DateTime showDate, int totalseats, List<int> availableSeats, double ticketPrice)
        {
            try
            {
                if (ShowExists(theatreOwner, theatreName, screenNo, showTime, showDate))
                {
                    throw new DuplicateShowException("A show already exists for the same date and time.");
                }

                Movie? movie = Movies.Find(m => m.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
                Theatre? theatre = GetTheatres().Find(t => t.Name.Equals(theatreName, StringComparison.OrdinalIgnoreCase));

                if (movie == null)
                {
                    throw new MovieNotFoundException("Movie not found.");
                }

                if (theatre == null)
                {
                    throw new TheatreNotFoundException("Theatre not found.");
                }

                Screen? screen = theatre.Screens.Find(s => s.ScreenNumber == screenNo);
                if (screen == null)
                {
                    throw new ScreenNotFoundException(screenNo, theatre.Name);
                }

                if (availableSeats.Count == 0 || ticketPrice < 0)
                {
                    throw new ArgumentException("Invalid show details provided.");
                }

                Show newShow = new Show(movie, showTime, showDate, totalseats, availableSeats, theatre, ticketPrice);

                foreach (var th in GetTheatres())
                {

                    Screen? TheatreScreen = th.Screens.Find(s => s.ScreenNumber == screenNo);
                    if (TheatreScreen != null)
                    {
                        bool showExists = TheatreScreen.Shows.Any(show =>
                                                show.Movie.Title.Equals(newShow.Movie.Title, StringComparison.OrdinalIgnoreCase) &&
                                                show.ShowTime == newShow.ShowTime &&
                                                show.ShowDate == newShow.ShowDate &&
                                                show.Theatre.Name.Equals(newShow.Theatre.Name, StringComparison.OrdinalIgnoreCase) &&
                                                show.TicketPrice == newShow.TicketPrice);

                        if (!showExists)
                        {
                            TheatreScreen.Shows.Add(newShow);
                        }
                    }

                }


                foreach (var owner in UserManagement.GetTheatreOwners())
                {
                    if (owner.OwnedTheatre == theatre)
                    {
                        Screen? ownerScreen = owner.OwnedTheatre.Screens.Find(s => s.ScreenNumber == screenNo);
                        if (ownerScreen != null)
                        {

                            ownerScreen.Shows.Add(newShow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteCentered($"Error: {ex.Message}");
                ReadCentered("Enter any key to exit:");
                return;
            }
        }

        public static void RemoveShow(string theatreName, int screenNumber, string movieTitle, DateTime showTime, DateTime showDate)
        {
            Theatre? theatre = GetTheatres().FirstOrDefault(t => t.Name.Equals(theatreName, StringComparison.OrdinalIgnoreCase));
            if (theatre == null)
            {
                throw new Exception("Theatre not found.");
            }

            Screen? screen = theatre.Screens.FirstOrDefault(s => s.ScreenNumber == screenNumber);
            if (screen == null)
            {
                throw new Exception("Screen not found.");
            }

            Show? show = screen.Shows.FirstOrDefault(s => s.Movie.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase) &&
                                                          s.ShowTime.Equals(showTime.ToString("hh:mm tt")) &&
                                                          s.ShowDate.Equals(showDate.ToString("dd/MM/yyyy")));
            if (show == null)
            {
                throw new Exception("Show not found.");
            }

            screen.Shows.Remove(show);

            foreach (var owner in UserManagement.GetTheatreOwners())
            {
                if (owner.OwnedTheatre == theatre)
                {
                    Screen? ownerScreen = owner.OwnedTheatre.Screens.Find(s => s.ScreenNumber == screenNumber);
                    if (ownerScreen != null)
                    {
                        ownerScreen.Shows.Remove(show);
                    }
                }
            }
        }

        public static void AddCoupon(string code, double discount)
        {
            try
            {
                if (string.IsNullOrEmpty(code) || discount <= 0 || discount > 100)
                {
                    throw new ArgumentException("Invalid coupon details provided.");
                }
                if (Coupons.ContainsKey(code))
                {
                    throw new DuplicateCouponException("Coupon already exists.");
                }
                Coupons[code] = discount;
            }
            catch (Exception ex)
            {
                WriteCentered($"Error: {ex.Message}");
            }
        }


        public static void RemoveMovie(string movieTitle)
        {
            try
            {
                Movie? movie = Movies.Find(m => m.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
                if (movie == null)
                {
                    throw new MovieNotFoundException("Movie not found.");
                }

                foreach (var theatre in Theatres)
                {
                    foreach (var screen in theatre.Screens)
                    {
                        screen.Shows.RemoveAll(s => s.Movie.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
                    }
                }

                Movies.Remove(movie);

                foreach (var owner in UserManagement.GetTheatreOwners())
                {
                    if (owner.OwnedTheatre != null)
                    {
                        foreach (var screen in owner.OwnedTheatre.Screens)
                        {
                            screen.Shows.RemoveAll(s => s.Movie.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteCentered($"Error: {ex.Message}");
            }
        }

        public static bool DisplayShows(TheatreOwner theatreOwner)
        {
            Theatre theatre = theatreOwner.OwnedTheatre;
            if (theatre != null)
            {
                bool showAvailable = false;
                foreach (Screen screen in theatre.Screens)
                {
                    WriteCentered($"Screen Number: {screen.ScreenNumber}:");
                    foreach (Show show in screen.Shows.Distinct())
                    {
                        WriteCentered($"Movie: {show.Movie.Title}, Show Time: {show.ShowTime}, Show Date: {show.ShowDate}, Total Seats : {show.TotalSeats}, Available Seats: {show.AvailableSeats.Count}, Ticket Price: Rs {show.TicketPrice}");
                        showAvailable = true;
                    }
                    Console.WriteLine();
                }
                return showAvailable;
            }
            return false;
        }
    }
}
