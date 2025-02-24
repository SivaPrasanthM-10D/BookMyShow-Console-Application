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

        public Customer(string id, string password, string name,string upiid, string upipin) : base(id, password, name)
        {
            if(!int.TryParse(upipin, out int pin) || (pin < 100000 || pin > 999999))
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
            Console.WriteLine("\nTicket Details");
            Console.WriteLine($"Movie : {MovieName}");
            Console.WriteLine($"Show Time : {ShowTime}");
            Console.WriteLine($"Seat : {string.Join(",",SeatNo)}");
            Console.WriteLine($"Theatre : {TheatreName}");
            Console.WriteLine($"Price : ₹{Price}");
        }
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
    public static class BookingSystem
    {
        public static bool BookTicket(Customer customer, Show show, int seatNumber)
        {
            if(show.AvailableSeats > 0 && seatNumber > 0)
            {
                Console.WriteLine($"Initiating payment from UPI ID: {customer.UpiId}");
                Console.Write("Enter your 6-digit UPI Pin:");

                if(!int.TryParse(Console.ReadLine(),out int upipin) || upipin < 100000 || upipin > 999999)
                {
                    Console.WriteLine("Invalid UPI Pin format. Payment failed.");
                    return false;
                }

                if(upipin == customer.UpiPin)
                {
                    show.AvailableSeats--;
                    var ticket = new Ticket(show.Movie.Title, show.ShowTime, seatNumber, show.Theatre.Name, show.TicketPrice);
                    customer.BookedTickets.Add(ticket);
                    Console.WriteLine("Payment Successful! Your ticket has been book!");
                    ticket.DisplayTicket();
                    return true;
                }
                else
                {
                    Console.WriteLine("Incorrect UPI PIN. Payment failed.");
                    return false;
                }
            }
            return false;
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
            for(int i = 1; i <= numscreens; i++)
            {
                theatre.Screens.Add(new Screen(i));
            }
            Theatres.Add(theatre);
        }

        public static List<Theatre> GetTheatres()
        {
            return Theatres;
        }

        public static void AddShow(string theatrename, int screenno,string movietitle, string showtime, int availableseats, double tktprice)
        {
            Movie movie = Movies.Find(m => m.Title.Equals(movietitle));
            Theatre theatre = Theatres.Find(t => t.Name == theatrename);

            if(movie != null && theatre != null)
            {
                Screen screen = theatre.Screens.Find(s => s.ScreenNumber == screenno);
                if(screen != null)
                {
                    screen.Shows.Add(new Show(movie, showtime, availableseats, theatre, tktprice));
                }
                else
                {
                    Console.WriteLine($"Screen number not found in {theatre.Name}");
                } 
            }
            else
            {
                Console.WriteLine("Invalid theatre or movie selection.");
            }
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
            foreach(var u in Users)
            {
                if(u.Id == id && u.Password == password)
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
            Console.WriteLine("Welcome to BookMyShow Console Application!");

            while (true)
            {
                Console.WriteLine("1.Login");
                Console.WriteLine("2.Exit");
                Console.Write("Enter your choice:");
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter User ID:");
                        string id = Console.ReadLine();
                        Console.Write("Enter Password:");
                        string password = Console.ReadLine();

                        User user = UserManagement.Login(id, password);

                        if (user is Admin)
                        {
                            Console.WriteLine("Welcome, Admin!");
                            AdminMenu();
                        }
                        else if (user is Customer customer)
                        {
                            Console.WriteLine($"Welcome, {customer.Name}!");
                            CustomerMenu(customer);
                        }
                        else
                        {
                            Console.WriteLine("Invalid login credentials.");
                        }
                        break;
                    case "2":
                        Console.WriteLine("Thank you! Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid choice, try again.");
                        break;
                }
                
            }
        }

        static void AdminMenu()
        {
            while (true)
            {
                Console.WriteLine("\nAdmin Menu");
                Console.WriteLine("1. Add Movie");
                Console.WriteLine("2. Add Theatre");
                Console.WriteLine("3. Add Show");
                Console.WriteLine("4. Logout");
                Console.Write("Enter your choice:");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter movie title:");
                        string title = Console.ReadLine();
                        Console.Write("Enter genere:");
                        string genere = Console.ReadLine();
                        Console.Write("Enter duration (minutes):");
                        int duration = int.Parse(Console.ReadLine());
                        AdminOperations.AddMovie(title, genere, duration);
                        break;
                    case "2":
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
                        int tktprice = int.Parse(Console.ReadLine());
                        AdminOperations.AddShow(stname, ssno, stname, stime, savlseats, tktprice);
                        break;
                    case "4":
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
                Console.WriteLine("3. Logout");
                Console.Write("Enter your choice:");

                switch (Console.ReadLine())
                {
                    case "1":
                        Console.Write("Enter theatre name:");
                        string tname = Console.ReadLine();
                        Console.Write("Enter show time:");
                        string stime = Console.ReadLine();
                        Console.Write("Enter seat no:");
                        int seatno = int.Parse(Console.ReadLine());

                        Show selectedshow = null;
                        foreach(var theatre in AdminOperations.GetTheatres())
                        {
                            if(theatre.Name == tname)
                            {
                                foreach(var screen in theatre.Screens)
                                {
                                    selectedshow = screen.Shows.Find(s => s.ShowTime == stime);
                                    if (selectedshow != null)
                                        break;
                                }
                            }
                            if (selectedshow != null)
                                break;
                        }
                        if(selectedshow != null)
                        {
                            BookingSystem.BookTicket(customer, selectedshow, seatno);
                        }
                        else
                        {
                            Console.WriteLine("Show not found. Please check the details and try again.");
                        }
                            break;
                    case "2":
                        if(customer.BookedTickets.Count == 0)
                        {
                            Console.WriteLine("No booked tickets found.");
                        }
                        else
                        {
                            foreach(var ticket in customer.BookedTickets)
                            {
                                ticket.DisplayTicket();
                            }
                        }
                        break;
                    case "3":
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