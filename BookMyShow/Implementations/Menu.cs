using BookMyShow.Custom_Exceptions;
using BookMyShow.Interfaces;
using BookMyShow.Models;

namespace BookMyShow.Implementations
{
    public class Menu : IMenu
    {
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

        public void Simulate()
        {
            WriteCentered("Welcome to BookMyShow Console Application!");

            while (true)
            {
                WriteCentered("1. Login");
                WriteCentered("2. Sign Up");
                WriteCentered("3. Exit");
                string choice = ReadCentered("Enter your choice:");
                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Clear();
                            WriteCentered("");
                            WriteCentered("Login Page");
                            WriteCentered("");
                            string id = ReadCentered("Enter User ID:").Trim();
                            if (string.IsNullOrEmpty(id))
                            {
                                throw new InvalidUserException("User ID cannot be empty.");
                            }
                            string password = ReadCentered("Enter Password:").Trim();
                            if (string.IsNullOrEmpty(password))
                            {
                                throw new InvalidPasswordException("Password cannot be empty.");
                            }

                            User user = UserManagement.Login(id, password);

                            if (user is Admin admin)
                            {
                                AdminMenu(admin);
                            }
                            else if (user is Customer customer)
                            {
                                CustomerMenu(customer);
                            }
                            else
                            {
                                WriteCentered("Invalid login credentials.");
                                ReadCentered("Press any key to retry:");
                            }
                        }
                        catch (InvalidPasswordException e) { WriteCentered(e.Message); break; }
                        catch (InvalidUserException e) { WriteCentered(e.Message); break; }
                        break;
                    case "2":
                        Console.Clear();
                        WriteCentered("");
                        WriteCentered("Register Now!");
                        WriteCentered("");
                        string nname, nid, npass, upiid, upipin, phoneno;
                        while (true)
                        {
                            nname = ReadCentered("Your username:").Trim();
                            if (string.IsNullOrEmpty(nname))
                            {
                                WriteCentered("Username cannot be empty.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        while (true)
                        {
                            nid = ReadCentered("Your User ID:").Trim();
                            if (string.IsNullOrEmpty(nid))
                            {
                                WriteCentered("User ID cannot be empty.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        while (true)
                        {
                            try
                            {
                                npass = ReadCentered("Enter new password:").Trim();
                                if (string.IsNullOrEmpty(npass))
                                {
                                    throw new InvalidPasswordException("Password cannot be empty.");
                                }
                                else if (!UserManagement.IsValidPassword(npass))
                                {
                                    throw new InvalidPasswordException();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (InvalidPasswordException e) { WriteCentered(e.Message); }
                        }
                        while (true)
                        {
                            try
                            {
                                if (!npass.Equals(ReadCentered("Confirm your password:")))
                                {
                                    throw new PasswordNotMatchException();
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (PasswordNotMatchException e) { WriteCentered(e.Message); }
                        }
                        while (true)
                        {
                            try
                            {
                                phoneno = ReadCentered("Enter your Phone Number:");
                                if (string.IsNullOrEmpty(phoneno) || !UserManagement.IsValidPhoneNumber(phoneno))
                                {
                                    throw new InvalidPhoneNoException("Invalid Phone No.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (InvalidPhoneNoException e) { WriteCentered(e.Message); }
                        }
                        while (true)
                        {
                            try
                            {
                                upiid = ReadCentered("Enter your UPI ID:");
                                if (string.IsNullOrEmpty(upiid) || !UserManagement.IsValidUpiId(upiid))
                                {
                                    throw new InvalidUpiException("Invalid UPI ID.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch (InvalidUpiException e) { WriteCentered(e.Message); }
                        }

                        while (true)
                        {
                            upipin = ReadCentered("Enter your UPI Pin:");
                            if (string.IsNullOrEmpty(upipin))
                            {
                                WriteCentered("UPI PIN cannot be empty.");
                            }
                            else
                            {
                                break;
                            }
                        }


                        Console.Clear();
                        UserManagement.Signup(nid, npass, nname, phoneno, upiid, upipin);
                        WriteCentered("Registration successful!");
                        ReadCentered("Press any key to exit:");
                        break;
                    case "3":

                        Console.Clear();
                        WriteCentered("Thank you! Exiting...");
                        return;
                    default:
                        WriteCentered("Invalid choice, try again.");
                        break;
                }
            }
        }

        public void AdminMenu(Admin admin)
        {
            while (true)
            {

                Console.Clear();
                WriteCentered("Welcome, Admin!");
                WriteCentered("");
                WriteCentered("Admin Menu");
                WriteCentered("");
                WriteCentered("1. Add Theatre");
                WriteCentered("2. Remove Theatre");
                WriteCentered("3. Add Movie");
                WriteCentered("4. Remove Movie");
                WriteCentered("5. Add Show");
                WriteCentered("6. Remove Show");
                WriteCentered("7. View Reviews");
                WriteCentered("8. Remove Review");
                WriteCentered("9. Add Coupon");
                WriteCentered("10. View Profile");
                WriteCentered("11. Logout");
                string choice = ReadCentered("Enter your choice:");

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        string tname = ReadCentered("Enter theatre name:");
                        string city = ReadCentered("Enter city:");
                        string street = ReadCentered("Enter street:");
                        int sno = int.Parse(ReadCentered("Enter number of screens:"));
                        AdminOperations.AddTheatre(tname, city, street, sno);
                        WriteCentered("Theatre added successfully!");
                        ReadCentered("Press any key to exit:");
                        break;
                    case "2":
                        Console.Clear();
                        string theatreName = ReadCentered("Enter theatre name to remove:");
                        Theatre? theatre = AdminOperations.GetTheatres().Find(t => t.Name.Equals(theatreName, StringComparison.OrdinalIgnoreCase));
                        if (theatre != null)
                        {
                            WriteCentered($"Theatre Name: {theatre.Name}");
                            WriteCentered($"City: {theatre.City}");
                            WriteCentered($"Street: {theatre.Street}");
                            WriteCentered($"Number of Screens: {theatre.Screens.Count}");
                            string confirm = ReadCentered("Are you sure you want to remove this theatre? (yes/no):");
                            if (confirm.ToLower() == "yes")
                            {
                                AdminOperations.RemoveTheatre(theatreName);
                                WriteCentered("Theatre removed successfully!");
                                ReadCentered("Press any key to exit:");
                            }
                            else
                            {
                                WriteCentered("Theatre removal cancelled.");
                                ReadCentered("Press any key to exit:");
                            }
                        }
                        else
                        {
                            WriteCentered("Theatre not found.");
                            ReadCentered("Press any key to exit:");
                        }
                        break;
                    case "3":
                        Console.Clear();
                        string title = ReadCentered("Enter movie title:");
                        string genre = ReadCentered("Enter genre:");
                        int duration = int.Parse(ReadCentered("Enter duration (minutes):"));
                        AdminOperations.AddMovie(title, genre, duration);
                        WriteCentered("Movie added successfully!");
                        ReadCentered("Press any key to exit:");
                        break;
                    case "4":
                        Console.Clear();
                        string movieTitle = ReadCentered("Enter movie title to remove:");
                        Movie? movie = AdminOperations.GetMovies().Find(m => m.Title.Equals(movieTitle, StringComparison.OrdinalIgnoreCase));
                        if (movie != null)
                        {
                            WriteCentered($"Movie Title: {movie.Title}");
                            WriteCentered($"Genre: {movie.Genre}");
                            WriteCentered($"Duration: {movie.Duration} minutes");
                            string confirm = ReadCentered("Are you sure you want to remove this movie? (yes/no):");
                            if (confirm.ToLower() == "yes")
                            {
                                AdminOperations.RemoveMovie(movieTitle);
                                WriteCentered("Movie removed successfully!");
                                ReadCentered("Press any key to exit:");
                            }
                            else
                            {
                                WriteCentered("Movie removal cancelled.");
                                ReadCentered("Press any key to exit:");
                            }
                        }
                        else
                        {
                            WriteCentered("Movie not found.");
                            ReadCentered("Press any key to exit:");
                        }
                        break;
                    case "5":
                        try
                        {
                            Console.Clear();
                            WriteCentered("Available Theatres:");
                            foreach (var th in AdminOperations.GetTheatres())
                            {
                                WriteCentered($"{th.Name} - {th.Screens.Count} screens");
                            }
                            WriteCentered("Available Movies:");
                            foreach (var mv in AdminOperations.GetMovies())
                            {
                                WriteCentered($"{mv.Title}");
                            }
                            string stname = ReadCentered("Enter theatre name:");
                            int ssno = int.Parse(ReadCentered("Enter screen number:"));
                            string stitle = ReadCentered("Enter movie title:");
                            string sd = ReadCentered("Enter show date (DD/MM/YYYY):");
                            DateTime.TryParseExact(sd, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime sdate);
                            string st;
                            DateTime stime;
                            List<string> showtimings = new List<string> { "10:30 AM", "02:30 PM", "06:30 PM", "10:30 PM" };
                            List<Show> shows = AdminOperations.GetTheatres()
                                .Find(t => t.Name.Equals(stname, StringComparison.OrdinalIgnoreCase))?
                                .Screens.Find(s => s.ScreenNumber == ssno)?
                                .Shows.FindAll(sh => sh.ShowDate.Equals(sdate.ToString("dd/MM/yyyy")) && sh.ShowTime != null);
                            String stimings = "";
                            if (shows.Count > 0)
                            {
                                foreach (string sts in showtimings)
                                {
                                    foreach (var s in shows)
                                    {
                                        if (!sts.Equals(s.ShowTime.ToString()))
                                        {
                                            stimings += "| ";
                                            stimings += sts;
                                            stimings += " | ";
                                        }
                                    }

                                }
                            }
                            else
                            {
                                foreach (var s in showtimings)
                                {
                                    stimings += "| ";
                                    stimings += s;
                                    stimings += " | ";
                                }
                            }

                            while (true)
                            {
                                WriteCentered(stimings);
                                st = ReadCentered("Enter show time (HH:MM AM/PM):");
                                if (!DateTime.TryParseExact(st, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out stime) ||
                                    !showtimings.Contains(st))
                                {
                                    throw new InvalidShowtimeException("Invalid show time. Allowed show times are 10:30 AM, 02:30 PM, 06:30 PM, and 10:30 PM.");
                                }
                                if (AdminOperations.ShowExists(stname, ssno, stime, sdate))
                                {
                                    throw new DuplicateShowException("A show already exists for the same date and time.");
                                }
                                else
                                {
                                    break;
                                }
                            }

                            int savlseats;
                            while (true)
                            {
                                if (!int.TryParse(ReadCentered("Enter available seats (100 - 160):"), out savlseats) || savlseats > 160 || savlseats < 100)
                                {
                                    throw new InvalidSeatNoException("Invalid input! Valid total seat number between 100 and 160.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            double tktprice = double.Parse(ReadCentered("Enter ticket price:"));

                            AdminOperations.AddShow(stname, ssno, stitle, stime, sdate, savlseats, tktprice);
                            WriteCentered("Show added successfully!");
                            ReadCentered("Press any key to exit:");
                        }
                        catch (InvalidSeatNoException e) { WriteCentered($"{e.Message}"); }
                        catch (DuplicateShowException e) { WriteCentered($"{e.Message}"); }
                        catch (InvalidShowtimeException e) { WriteCentered($"{e.Message}"); }
                        break;
                    case "6":
                        Console.Clear();
                        string rTheatreName = ReadCentered("Enter theatre name:");
                        int rScreenNo = int.Parse(ReadCentered("Enter screen number:"));
                        string rMovieTitle = ReadCentered("Enter movie title:");
                        string rShowTime = ReadCentered("Enter show time (HH:MM AM/PM):");
                        DateTime.TryParseExact(rShowTime, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out DateTime rShowDateTime);
                        Show? show = AdminOperations.GetTheatres()
                            .Find(t => t.Name.Equals(rTheatreName, StringComparison.OrdinalIgnoreCase))?
                            .Screens.Find(s => s.ScreenNumber == rScreenNo)?
                            .Shows.Find(s => s.Movie.Title.Equals(rMovieTitle, StringComparison.OrdinalIgnoreCase) && s.ShowTime == rShowDateTime.ToString("hh:mm tt"));
                        if (show != null)
                        {
                            WriteCentered($"Movie Title: {show.Movie.Title}");
                            WriteCentered($"Show Time: {show.ShowTime}");
                            WriteCentered($"Show Date: {show.ShowDate}");
                            WriteCentered($"Available Seats: {show.AvailableSeats}");
                            WriteCentered($"Ticket Price: ₹{show.TicketPrice}");
                            string confirm = ReadCentered("Are you sure you want to remove this show? (yes/no):");
                            if (confirm.ToLower() == "yes")
                            {
                                AdminOperations.RemoveShow(rTheatreName, rScreenNo, rMovieTitle, rShowDateTime);
                                WriteCentered("Show removed successfully!");
                                ReadCentered("Press any key to exit:");
                            }
                            else
                            {
                                WriteCentered("Show removal cancelled.");
                                ReadCentered("Press any key to exit:");
                            }
                        }
                        else
                        {
                            WriteCentered("Show not found.");
                            ReadCentered("Press any key to exit:");
                        }
                        break;
                    case "7":
                        Console.Clear();
                        ReviewSystem.ViewReview();
                        break;
                    case "8":
                        Console.Clear();
                        ReviewSystem.RemoveReview();
                        WriteCentered("Review removed successfully!");
                        ReadCentered("Press any key to exit:");
                        break;
                    case "9":
                        try
                        {
                            Console.Clear();
                            string code = ReadCentered("Enter coupon code:").Trim();
                            if (string.IsNullOrEmpty(code))
                            {
                                throw new InvalidCouponException("Coupon code can't be empty.");
                            }
                            if (!double.TryParse(ReadCentered("Enter discount percentage:"), out double discount) || discount <= 0 || discount > 100)
                            {
                                throw new InvalidCouponException("Invalid coupon code. Try again.");
                            }
                            AdminOperations.AddCoupon(code, discount);
                            WriteCentered("Coupon added successfully!");
                            ReadCentered("Press any key to exit:");
                        }
                        catch (InvalidCouponException e) { WriteCentered(e.Message); break; }
                        break;
                    case "10":

                        Console.Clear();
                        WriteCentered("");
                        WriteCentered(".......Profile.......\n");
                        admin.DisplayUserInfo();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "11":
                        WriteCentered("Logging out!");
                        return;
                    default:
                        try { throw new InvalidChoiceException("Invalid choice, try again."); }
                        catch (InvalidChoiceException e) { WriteCentered(e.Message); break; }
                }
            }
        }

        public void CustomerMenu(Customer customer)
        {
            while (true)
            {
                try
                {

                    Console.Clear();
                    WriteCentered($"Welcome, {customer.Name}!");
                    ReviewSystem.DisplayMovies();
                    WriteCentered("");
                    WriteCentered("Customer Menu");
                    WriteCentered("");
                    WriteCentered("1. Book Tickets");
                    WriteCentered("2. View Booked Tickets");
                    WriteCentered("3. Cancel Ticket");
                    WriteCentered("4. Add Review");
                    WriteCentered("5. View Review");
                    WriteCentered("6. Update Review");
                    WriteCentered("7. Delete Review");
                    WriteCentered("8. View Profile");
                    WriteCentered("9. Logout");
                    string choice = ReadCentered("Enter your choice:");

                    switch (choice)
                    {
                        case "1":
                            try
                            {
                                Console.Clear();
                                string searchcity = ReadCentered("Enter city:");
                                List<Theatre> theatresincity = AdminOperations.GetTheatres().FindAll(t => t.City.Equals(searchcity, StringComparison.OrdinalIgnoreCase));
                                if (theatresincity.Count == 0)
                                {
                                    throw new TheatreNotFoundException($"No theatres found in {searchcity}");
                                }

                                foreach (var theatre in theatresincity)
                                {
                                    WriteCentered($"Theatre : {theatre.Name}, {theatre.Street}");
                                    foreach (var screen in theatre.Screens)
                                    {
                                        foreach (var show in screen.Shows)
                                        {
                                            WriteCentered($"Movie : {show.Movie.Title} | Show Time : {show.ShowTime:hh:mm tt} | Available Seats : {show.AvailableSeats} | Ticket Price : ₹{show.TicketPrice}");
                                        }
                                    }
                                    WriteCentered("");
                                }

                                string selectedtheatre = ReadCentered("Enter theatre name:");
                                Theatre? chosentheatre = theatresincity.Find(t => t.Name.Equals(selectedtheatre, StringComparison.OrdinalIgnoreCase));

                                if (chosentheatre == null)
                                {
                                    throw new InvalidTheatreNameException("Invalid theatre name.");
                                }

                                string chosenmovie = ReadCentered("Enter movie name:");
                                string chosenshowtime = ReadCentered("Enter show time (HH:MM AM/PM):");

                                if (!DateTime.TryParseExact(chosenshowtime, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out DateTime _))
                                {
                                    throw new InvalidShowtimeException("Invalid time format. Use HH:MM AM/PM.");
                                }
                                Show? chosenshow = chosentheatre.Screens.SelectMany(s => s.Shows).FirstOrDefault(sh => sh.Movie.Title.Equals(chosenmovie, StringComparison.OrdinalIgnoreCase) &&
                                                                                 sh.ShowTime.Equals(chosenshowtime, StringComparison.OrdinalIgnoreCase));

                                if (chosenshow == null)
                                {
                                    throw new InvalidShowtimeException("Invalid movie or show time selection.");
                                }

                                BookingSystem.DisplaySeats(chosenshow);
                                if (!int.TryParse(ReadCentered("Enter number of seats to book:"), out int nos) || nos <= 0)
                                {
                                    throw new SeatNotAvailableException("Invalid Input. Please enter a valid number.");
                                }
                                List<int> seatnumbers = [];
                                for (int i = 0; i < nos; i++)
                                {
                                    if (!int.TryParse(ReadCentered($"Enter seat number {i + 1}:"), out int seat) || seat <= 0)
                                    {
                                        i--;
                                        throw new InvalidSeatNoException("Invalid seat number. Please try again.");
                                    }
                                    else
                                    {
                                        seatnumbers.Add(seat);
                                    }
                                }
                                BookingSystem.BookTicket(customer, chosenshow, seatnumbers);
                                BookingSystem.DisplaySeats(chosenshow);
                                WriteCentered("Tickets booked successfully!");
                                ReadCentered("Press any key for customer menu:");
                            }
                            catch (InvalidTheatreNameException e) { WriteCentered(e.Message); break; }
                            catch (InvalidShowtimeException e) { WriteCentered(e.Message); break; }
                            catch (InvalidSeatNoException e) { WriteCentered(e.Message); }
                            catch (SeatNotAvailableException e) { WriteCentered(e.Message); break; }
                            catch (TheatreNotFoundException e) { WriteCentered(e.Message); break; }

                            break;
                        case "2":
                            try
                            {

                                Console.Clear();
                                if (customer.BookedTickets.Count == 0)
                                {
                                    throw new TicketNotFoundException("No booked tickets found.");
                                }
                                else
                                {
                                    foreach (var ticket in customer.BookedTickets)
                                    {
                                        ticket.DisplayTicket();
                                    }
                                }
                            }
                            catch (TicketNotFoundException e) { WriteCentered(e.Message); ReadCentered("Press any key to exit:"); }
                            break;
                        case "3":

                            Console.Clear();
                            if (!BookingSystem.CancelTicket(customer))
                            {
                                ReviewSystem.DisplayMovies();
                            }
                            WriteCentered("Ticket cancelled successfully!");
                            ReadCentered("Press any key for customer menu:");
                            break;
                        case "4":

                            Console.Clear();
                            ReviewSystem.AddReview(customer);
                            WriteCentered("Review added successfully!");
                            ReadCentered("Press any key for customer menu:");
                            break;
                        case "5":
                            Console.Clear();
                            ReviewSystem.ViewReview();
                            ReadCentered("Press any key for customer menu:");
                            break;
                        case "6":
                            Console.Clear();
                            ReviewSystem.UpdateReview(customer);
                            ReadCentered("Press any key for customer menu:");
                            break;
                        case "7":
                            Console.Clear();
                            ReviewSystem.RemoveReview();
                            ReadCentered("Press any key for customer menu:");
                            break;
                        case "8":
                            Console.Clear();
                            WriteCentered("");
                            WriteCentered(".......Profile.......\n");
                            customer.DisplayUserInfo();
                            ReadCentered("Press any key for customer menu:");
                            break;
                        case "9":
                            WriteCentered("Logging out!");
                            return;
                        default:
                            throw new InvalidChoiceException("Invalid choice.");
                    }
                }
                catch (InvalidChoiceException e) { WriteCentered(e.Message); ReadCentered("Press any key to try again:"); break; }
            }
        }
    }
}