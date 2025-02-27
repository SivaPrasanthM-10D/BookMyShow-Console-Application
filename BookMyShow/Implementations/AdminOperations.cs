using static BookMyShow.Ticket;
namespace BookMyShow.Implementations
{
    public static class AdminOperations
    {
        private static List<Movie> Movies = new List<Movie>();
        private static List<Screen> Screens = new List<Screen>();
        private static List<Theatre> Theatres = new List<Theatre>();
        private static Dictionary<string, double> Coupons = new Dictionary<string, double>();

        public static void AddMovie(string title, string genere, int duration)
        {
            Movies.Add(new Movie(title, genere, duration));
            Console.WriteLine("Movie Added Successfully.");
        }

        public static void AddScreen(int screennumber)
        {
            Screens.Add(new Screen(screennumber));
        }

        public static void AddTheatre(string name, string city, string street, int numscreens)
        {
            //Theatres.Add(new Theatre(name, location));
            Theatre theatre = new Theatre(name, city, street);
            for (int i = 1; i <= numscreens; i++)
            {
                theatre.Screens.Add(new Screen(i));
            }
            Theatres.Add(theatre);
        }

        public static List<Theatre> GetTheatres()
        {
            return Theatres;
        }
        public static Dictionary<string, double> GetCoupons()
        {
            return Coupons;
        }

        public static void AddShow(string theatrename, int screenno, string movietitle, DateTime showtime, int availableseats, double tktprice)
        {
            Movie? movie = Movies.Find(m => m.Title.Equals(movietitle, StringComparison.OrdinalIgnoreCase));
            Theatre? theatre = Theatres.Find(t => t.Name.Equals(theatrename, StringComparison.OrdinalIgnoreCase));

            if (movie == null)
            {
                Console.WriteLine("Movie not found. Please add the movie first.");
                return;
            }

            if (theatre == null)
            {
                Console.WriteLine("Theatre not found. Please add the theatre first.");
                return;
            }

            Screen? screen = theatre.Screens.Find(s => s.ScreenNumber == screenno);
            if (screen == null)
            {
                Console.WriteLine($"Screen number {screenno} does not exist in {theatre.Name}");
                return;
            }
            screen.Shows.Add(new Show(movie, showtime, availableseats, theatre, tktprice));
            Console.WriteLine($"Show successfully added: {movie.Title} at {theatre.Name}, Screen {screenno}");
        }

        public static void AddCoupon(string code, double discount)
        {
            if (Coupons.ContainsKey(code))
            {
                Console.WriteLine("Coupon already exists!");
                return;
            }
            Coupons[code] = discount;
            Console.WriteLine("Coupon added successfully!");
        }
    }
}
