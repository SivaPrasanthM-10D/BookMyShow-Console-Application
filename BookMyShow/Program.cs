namespace BookMyShow
{
    public abstract class User
    {
        public string Id;
        public string Password;
        public string Name;

        protected User(string id, string password, string name)
        {
            Id = id;
            Password = password;
            Name = name;
        }
    }

    public class Admin : User
    {
        public Admin(string id, string password, string name) : base(id, password, name)
        {
        }
    }

    public class Customer : User
    {
        public List<Ticket> BookedTickets = new List<Ticket>();
        public string UpiId;
        public int UpiPin;

        public Customer(string id, string password, string name, string upiid, string upipin) : base(id, password, name)
        {
            if (!int.TryParse(upipin, out int pin) || (pin < 100000 || pin > 999999))
            {
                throw new ArgumentException("UPI Pin must be a 6 digit number.");
            }
            UpiId = upiid;
            UpiPin = pin;
        }
    }

    public class Ticket
    {
        public string MovieName;
        public string ShowTime;
        public List<int> SeatNo;
        public string TheatreName;
        public double Price;

        public Ticket(string movieName, string showTime, List<int> seatNo, string theatreName, double price)
        {
            MovieName = movieName;
            ShowTime = showTime;
            SeatNo = seatNo;
            TheatreName = theatreName;
            Price = price;
        }

        public void DisplayTicket()
        {
            Console.WriteLine("\n****************** Ticket Details ******************");
            Console.WriteLine($"* Movie     : {MovieName}");
            Console.WriteLine($"* Show Time : {ShowTime}");
            Console.WriteLine($"* Seat(s)   : {string.Join(",", SeatNo)}");
            Console.WriteLine($"* Theatre   : {TheatreName}");
            Console.WriteLine($"* Price     : ₹{Price}");
            Console.WriteLine("***************************************************");

        }

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

        public class Show
        {
            public Movie Movie;
            public string ShowTime;
            public int AvailableSeats;
            public Theatre Theatre;
            public double TicketPrice;
            public List<int> BookSeats = new List<int>();

            public Show(Movie movie, string showTime, int availableSeats, Theatre theatre, double ticketPrice)
            {
                Movie = movie;
                ShowTime = showTime;
                AvailableSeats = availableSeats;
                Theatre = theatre;
                TicketPrice = ticketPrice;
            }
        }

        public class Screen
        {
            public int ScreenNumber;
            public List<Show> Shows = new List<Show>();

            public Screen(int screenNumber)
            {
                ScreenNumber = screenNumber;
            }
        }

        public class Theatre
        {
            public string Name;
            public string City;
            public string Street;
            public List<Screen> Screens = new List<Screen>();

            public Theatre(string name, string city, string street)
            {
                Name = name;
                City = city;
                Street = street;
            }
        }

        public class MovieReview
        {
            public string MovieTitle;
            public int Rating;
            public string Review;

            public MovieReview(string movieTitle, int rating, string review)
            {
                MovieTitle = movieTitle;
                Rating = rating;
                Review = review;
            }
        }

        public static class ReviewSystem
        {
            private static List<MovieReview> Reviews = new List<MovieReview>();

            public static void AddReview()
            {
                Console.Write("Enter movie name:");
                string moviename = Console.ReadLine();
                Console.Write("Rate the movie (1-5):");
                if (!int.TryParse(Console.ReadLine(), out int rating) || rating < 1 || rating > 5)
                {
                    Console.WriteLine("Invalid rating. Must be between 1 and 5.");
                    return;
                }
                Console.Write("Enter your review:");
                string review = Console.ReadLine();
                Reviews.Add(new MovieReview(moviename, rating, review));
                Console.WriteLine("Thank you for your review!");
            }

            public static void ViewReview()
            {
                if (Reviews.Count == 0)
                {
                    Console.WriteLine("No reviews found.");
                    return;
                }
                foreach (var review in Reviews)
                {
                    Console.WriteLine($"\n{review.MovieTitle} - {review.Rating}/5");
                    Console.WriteLine($"{review.Review}");
                }
            }

            public static void RemoveReview()
            {
                if (Reviews.Count == 0)
                {
                    Console.WriteLine("No reviews found.");
                    return;
                }
                for(int i = 0; i < Reviews.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {Reviews[i].MovieTitle} - {Reviews[i].Rating}/5\nReview: {Reviews[i].Review}\n");
                }
                Console.Write("Enter the review number to remove:");
                if(!int.TryParse(Console.ReadLine(), out int revno) || revno < 1 || revno > Reviews.Count)
                {
                    Console.WriteLine("Invalid review number. Try again.");
                    return;
                }
                Reviews.RemoveAt(revno-1);
                Console.WriteLine("Review removed successfully!");
            }
        }

        public static class BookingSystem
        {
            public static void DisplaySeats(Show show)
            {
                Console.WriteLine("\nSeat Layout:");
                Console.WriteLine("=====================================");
                Console.WriteLine("\n        _________________________________");
                Console.WriteLine("       /                                 \\");
                Console.WriteLine();

                int totalseats = show.AvailableSeats + show.BookSeats.Count;
                for (int i = 1; i <= totalseats; i++)
                {
                    if (show.BookSeats.Contains(i))
                    {
                        Console.Write("[X]");
                    }
                    else
                    {
                        Console.Write($"[{i}]");
                    }
                    if (i % 10 == 0)
                    {
                        Console.WriteLine();
                    }
                }
                Console.WriteLine("=====================================\n");
            }

            public static double ApplyDiscount(double totalprice, int seatcount)
            {
                Console.Write("Do you have coupon code? {y/n}:");
                string inp = Console.ReadLine().ToUpper();

                double discount = 0;
                if (seatcount >= 3)
                {
                    discount += 0.10;
                    Console.WriteLine("Bulk discount applied (10% off)!");
                }

                if (inp == "Y")
                {
                    Console.Write("Enter the coupon code:");
                    string coupon = Console.ReadLine().ToUpper();

                    if (coupon == "MOVIE10")
                    {
                        discount += 0.10;
                        Console.WriteLine("Promo code MOVIE10 applied! Extra 10% Off.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid coupon code.");
                    }
                }

                double finprice = totalprice - discount;
                return finprice;
            }


            public static bool BookTicket(Customer customer, Show show, List<int> seatNumbers)
            {
                foreach (var seat in seatNumbers)
                {
                    if (show.BookSeats.Contains(seat))
                    {
                        Console.WriteLine($"Seat {seat} already booked. Please choose another seat.");
                        return false;
                    }
                }

                double totalPrice = show.TicketPrice * seatNumbers.Count;
                totalPrice = ApplyDiscount(totalPrice, seatNumbers.Count);
                Console.WriteLine($"Total Price: ₹{totalPrice}");
                Console.Write("Do you want to proceed with payment? (yes/no): ");
                string confirm = Console.ReadLine().Trim().ToLower();

                if (confirm != "yes")
                {
                    Console.WriteLine("Booking cancelled.");
                    return false;
                }
                Console.WriteLine($"Initiating payment from UPI ID: {customer.UpiId}");
                Console.Write("Enter your 6-digit UPI Pin:");

                if (!int.TryParse(Console.ReadLine(), out int upipin) || upipin < 100000 || upipin > 999999)
                {
                    Console.WriteLine("Invalid UPI Pin format. Payment failed.");
                    return false;
                }

                if (upipin == customer.UpiPin)
                {
                    show.AvailableSeats -= seatNumbers.Count;
                    show.BookSeats.AddRange(seatNumbers);
                    double totalprice = show.TicketPrice * seatNumbers.Count;
                    var ticket = new Ticket(show.Movie.Title, show.ShowTime, seatNumbers, show.Theatre.Name, totalprice);
                    customer.BookedTickets.Add(ticket);
                    Console.WriteLine("Payment Successful! Your ticket(s) have been booked.");
                    ticket.DisplayTicket();
                    return true;
                }
                else
                {
                    Console.WriteLine("Incorrect UPI PIN. Payment failed.");
                    return false;
                }
            }

            public static void CancelTicket(Customer customer)
            {
                if (customer.BookedTickets.Count == 0)
                {
                    Console.WriteLine("No ticket booked to cancel.");
                    return;
                }
                Console.WriteLine("Your booked tickets:");
                for (int i = 0; i <= customer.BookedTickets.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {customer.BookedTickets[i].MovieName} - {customer.BookedTickets[i].ShowTime}");
                }

                Console.Write("Enter ticket number to cancel:");
                if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > customer.BookedTickets.Count)
                {
                    Console.WriteLine("Invalid choice.");
                    return;
                }

                Ticket ticket = customer.BookedTickets[choice - 1];
                DateTime showdatetime = DateTime.Parse(ticket.ShowTime);
                DateTime currenttime = DateTime.Now;

                double refund = 0;
                if ((currenttime - showdatetime).TotalHours > 24)
                {
                    refund = ticket.Price;
                }
                else if ((currenttime - showdatetime).TotalHours > 0)
                {
                    refund = ticket.Price * 0.5;
                }
                if (refund > 0)
                {
                    Console.WriteLine($"Ticket cancelled. Refund amount : {refund}");
                }
                else
                {
                    Console.WriteLine("Show already started. No refund.");
                }
                customer.BookedTickets.Remove(ticket);
            }
        }

        public static class AdminOperations
        {
            private static List<Movie> Movies = new List<Movie>();
            private static List<Screen> Screens = new List<Screen>();
            private static List<Theatre> Theatres = new List<Theatre>();

            public static void AddMovie(string title, string genere, int duration)
            {
                Movies.Add(new Movie(title, genere, duration));
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

            public static void AddShow(string theatrename, int screenno, string movietitle, string showtime, int availableseats, double tktprice)
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
        }

        public static class UserManagement
        {
            private static List<User> Users = new List<User>()
            {
                new Admin("admin","admin123","Admin"),
                new Customer("c1","c123","Customer1","c1@gmail.com","876543")
            };

            public static User Login(string id, string password)
            {
                foreach (var u in Users)
                {
                    if (u.Id == id && u.Password == password)
                    {
                        return u;
                    }
                }
                return null;
            }

            public static void RegisterCustomer(string id, string password, string name, string upiid, string upipin)
            {
                Users.Add(new Customer(id, password, name, upiid, upipin));
            }
        }

        class Program
        {
            static void Main()
            {
                Console.WriteLine("\t\t\t\t\t\t\tWelcome to BookMyShow Console Application!");

                while (true)
                {
                    Console.WriteLine("\n\t\t\t\t\t\t\t\t\t1.Login");
                    Console.WriteLine("\t\t\t\t\t\t\t\t\t2.Exit");
                    Console.Write("\t\t\t\t\t\t\t\t  Enter your choice:");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.Clear();
                            Console.WriteLine("\t\t\t\t\t\t\t\t\tLogin Page");
                            Console.Write("\n\t\t\t\t\t\t\t\t  Enter User ID:");
                            string id = Console.ReadLine().Trim();
                            if (string.IsNullOrEmpty(id))
                            {
                                Console.WriteLine("User ID cannot be empty.");
                                break;
                            }
                            Console.Write("\t\t\t\t\t\t\t\t  Enter Password:");
                            string password = Console.ReadLine().Trim();
                            if (string.IsNullOrEmpty(password))
                            {
                                Console.WriteLine("Password cannot be empty.");
                                break;
                            }

                            User user = UserManagement.Login(id, password);

                            if (user is Admin)
                            {
                                Console.Clear();
                                Console.WriteLine("\n\t\t\t\t\t\t\t\t\tWelcome, Admin!");
                                AdminMenu();
                            }
                            else if (user is Customer customer)
                            {
                                Console.Clear();
                                Console.WriteLine($"\n\t\t\t\t\t\t\t\t\tWelcome, {customer.Name}!");
                                CustomerMenu(customer);
                            }
                            else
                            {
                                Console.WriteLine("\n\t\t\t\t\t\t\t\tInvalid login credentials.");
                            }
                            break;
                        case "2":
                            Console.Clear();
                            Console.WriteLine("\n\t\t\t\t\t\t\t\tThank you! Exiting...");
                            return;
                        default:
                            Console.WriteLine("\n\t\t\t\t\t\t\t\tInvalid choice, try again.");
                            break;
                    }
                }
            }

            static void AdminMenu()
            {
                while (true)
                {
                    Console.WriteLine("\n\t\t\t\t\t\t\t\t\t  Admin Menu");
                    Console.WriteLine("\t\t\t\t\t\t\t\t    1. Add Theatre");
                    Console.WriteLine("\t\t\t\t\t\t\t\t    2. Add Movie");
                    Console.WriteLine("\t\t\t\t\t\t\t\t    3. Add Show");
                    Console.WriteLine("\t\t\t\t\t\t\t\t    4. View Reviews");
                    Console.WriteLine("\t\t\t\t\t\t\t\t    5. Remove Reviews");
                    Console.WriteLine("\t\t\t\t\t\t\t\t    6. Logout");
                    Console.Write("\t\t\t\t\t\t\t\t    Enter your choice:");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.Write("Enter theatre name:");
                            string tname = Console.ReadLine();
                            Console.Write("Enter city:");
                            string city = Console.ReadLine();
                            Console.Write("Enter street:");
                            string street = Console.ReadLine();
                            Console.Write("Enter number of screens:");
                            int sno = int.Parse(Console.ReadLine());
                            AdminOperations.AddTheatre(tname, city, street, sno);
                            break;
                        case "2":
                            Console.Write("Enter movie title:");
                            string title = Console.ReadLine();
                            Console.Write("Enter genere:");
                            string genere = Console.ReadLine();
                            Console.Write("Enter duration (minutes):");
                            int duration = int.Parse(Console.ReadLine());
                            AdminOperations.AddMovie(title, genere, duration);
                            break;
                        case "3":
                            Console.Write("Enter theatre name:");
                            string stname = Console.ReadLine();
                            Console.Write("Enter screen number:");
                            int ssno = int.Parse(Console.ReadLine());
                            Console.Write("Enter movie title:");
                            string stitle = Console.ReadLine();
                            Console.Write("Enter show time:");
                            string stime = Console.ReadLine();
                            Console.Write("Enter available seats:");
                            int savlseats = int.Parse(Console.ReadLine());
                            Console.Write("Enter ticket price:");
                            double tktprice = double.Parse(Console.ReadLine());
                            AdminOperations.AddShow(stname, ssno, stitle, stime, savlseats, tktprice);
                            break;
                        case "4":
                            ReviewSystem.ViewReview();
                            break;
                        case "5":
                            ReviewSystem.RemoveReview();
                            break;
                        case "6":
                            Console.WriteLine("Logging out!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice, try again.");
                            break;
                    }
                }
            }

            static void CustomerMenu(Customer customer)
            {
                while (true)
                {
                    Console.WriteLine("\nCustomer Menu");
                    Console.WriteLine("1. Book Tickets");
                    Console.WriteLine("2. View Booked Tickets");
                    Console.WriteLine("3. Add Review");
                    Console.WriteLine("4. View Review");
                    Console.WriteLine("5. Logout");
                    Console.Write("Enter your choice:");

                    switch (Console.ReadLine())
                    {
                        case "1":
                            Console.Write("Enter city:");
                            string searchcity = Console.ReadLine();
                            List<Theatre> theatresincity = AdminOperations.GetTheatres().FindAll(t => t.City.Equals(searchcity, StringComparison.OrdinalIgnoreCase));
                            if (theatresincity.Count == 0)
                            {
                                Console.WriteLine($"No theatres found in the {searchcity}");
                                break;
                            }

                            foreach (var theatre in theatresincity)
                            {
                                Console.WriteLine($"Theatre : {theatre.Name}");
                                foreach (var screen in theatre.Screens)
                                {
                                    foreach (var show in screen.Shows)
                                    {
                                        Console.WriteLine($"Movie : {show.Movie.Title} | Show Time : {show.ShowTime} | Available Seats : {show.AvailableSeats} | Ticket Price : ₹{show.TicketPrice}");
                                    }
                                }
                                Console.WriteLine();
                            }

                            Console.Write("Enter theatre name:");
                            string selectedtheatre = Console.ReadLine();
                            Theatre? chosentheatre = theatresincity.Find(t => t.Name.Equals(selectedtheatre, StringComparison.OrdinalIgnoreCase));

                            if (chosentheatre == null)
                            {
                                Console.WriteLine("Invalid theatre name.");
                                break;
                            }

                            Console.Write("Enter movie name:");
                            string chosenmovie = Console.ReadLine();
                            Console.Write("Enter show time:");
                            string chosenshowtime = Console.ReadLine();
                            Show? chosenshow = chosentheatre.Screens.SelectMany(s => s.Shows).FirstOrDefault(sh => sh.Movie.Title.Equals(chosenmovie, StringComparison.OrdinalIgnoreCase) &&
                                                                             sh.ShowTime.Equals(chosenshowtime, StringComparison.OrdinalIgnoreCase));

                            if (chosenshow == null)
                            {
                                Console.WriteLine("Invalid movie or show time selection.");
                                break;
                            }

                            BookingSystem.DisplaySeats(chosenshow);
                            Console.Write("Enter number of seats to book:");
                            if (!int.TryParse(Console.ReadLine(), out int nos) || nos <= 0)
                            {
                                Console.WriteLine("Invalid Input. Please enter a valid number.");
                                break;
                            }
                            List<int> seatnumbers = new List<int>();
                            for (int i = 0; i < nos; i++)
                            {
                                Console.Write($"Enter seat number {i + 1}:");
                                if (!int.TryParse(Console.ReadLine(), out int seat) || seat <= 0)
                                {
                                    Console.WriteLine("Invalid seat number. Please try again.");
                                    i--;
                                }
                                else
                                {
                                    seatnumbers.Add(seat);
                                }
                            }

                            BookingSystem.BookTicket(customer, chosenshow, seatnumbers);
                            break;

                        case "2":
                            if (customer.BookedTickets.Count == 0)
                            {
                                Console.WriteLine("No booked tickets found.");
                            }
                            else
                            {
                                foreach (var ticket in customer.BookedTickets)
                                {
                                    ticket.DisplayTicket();
                                }
                            }
                            break;

                        case "3":
                            ReviewSystem.AddReview();
                            break;
                        
                        case "4":
                            ReviewSystem.ViewReview();
                            break;
                        case "5":
                            Console.WriteLine("Logging out!");
                            return;

                        default:
                            Console.WriteLine("Invalid choice, try again.");
                            break;
                    }
                }
            }
        }
    }
}
