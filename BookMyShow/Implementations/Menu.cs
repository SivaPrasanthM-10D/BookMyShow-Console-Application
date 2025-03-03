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
                                Console.Clear();
                                WriteCentered("Welcome, Admin!");
                                AdminMenu(admin);
                            }
                            else if (user is Customer customer)
                            {
                                Console.Clear();
                                WriteCentered($"Welcome, {customer.Name}!");
                                CustomerMenu(customer);
                            }
                            else
                            {
                                WriteCentered("Invalid login credentials.");
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
                WriteCentered("");
                WriteCentered("Admin Menu");
                WriteCentered("");
                WriteCentered("1. Add Theatre");
                WriteCentered("2. Add Movie");
                WriteCentered("3. Add Show");
                WriteCentered("4. View Reviews");
                WriteCentered("5. Remove Reviews");
                WriteCentered("6. Add Coupon");
                WriteCentered("7. View Profile");
                WriteCentered("8. Logout");
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
                        break;
                    case "2":
                        Console.Clear();
                        string title = ReadCentered("Enter movie title:");
                        string genre = ReadCentered("Enter genre:");
                        int duration = int.Parse(ReadCentered("Enter duration (minutes):"));
                        AdminOperations.AddMovie(title, genre, duration);
                        WriteCentered("Movie added successfully!");
                        break;
                    case "3":
                        try
                        {
                            Console.Clear();
                            string stname = ReadCentered("Enter theatre name:");
                            int ssno = int.Parse(ReadCentered("Enter screen number:"));
                            string stitle = ReadCentered("Enter movie title:");
                            string st = ReadCentered("Enter show time (HH:MM AM/PM):");
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

                            DateTime.TryParseExact(st, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out DateTime stime);
                            AdminOperations.AddShow(stname, ssno, stitle, stime, savlseats, tktprice);
                            WriteCentered("Show added successfully!");
                        }
                        catch (InvalidSeatNoException e) { WriteCentered($"{e.Message}"); }
                        break;
                    case "4":
                        Console.Clear();
                        ReviewSystem.ViewReview();
                        break;
                    case "5":
                        Console.Clear();
                        ReviewSystem.RemoveReview();
                        WriteCentered("Review removed successfully!");
                        break;
                    case "6":
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
                        }
                        catch (InvalidCouponException e) { WriteCentered(e.Message); break; }
                        break;
                    case "7":
                        Console.Clear();
                        WriteCentered("");
                        WriteCentered(".......Profile.......\n");
                        admin.DisplayUserInfo();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "8":
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
                    ReviewSystem.DisplayMovies();
                    WriteCentered("");
                    WriteCentered("Customer Menu");
                    WriteCentered("");
                    WriteCentered("1. Book Tickets");
                    WriteCentered("2. View Booked Tickets");
                    WriteCentered("3. Cancel Ticket");
                    WriteCentered("4. Add Review");
                    WriteCentered("5. View Review");
                    WriteCentered("6. View Profile");
                    WriteCentered("7. Logout");
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
                            catch (TicketNotFoundException e) { WriteCentered(e.Message); }
                            break;
                        case "3":
                            Console.Clear();
                            if (!BookingSystem.CancelTicket(customer))
                            {
                                ReviewSystem.DisplayMovies();
                            }
                            WriteCentered("Ticket cancelled successfully!");
                            break;
                        case "4":
                            Console.Clear();
                            ReviewSystem.AddReview(customer);
                            WriteCentered("Review added successfully!");
                            break;
                        case "5":
                            Console.Clear();
                            ReviewSystem.ViewReview();
                            break;
                        case "6":
                            Console.Clear();
                            WriteCentered("");
                            WriteCentered(".......Profile.......\n");
                            customer.DisplayUserInfo();
                            ReadCentered("Press any key to exit:");
                            break;
                        case "7":
                            WriteCentered("Logging out!");
                            return;
                        default:
                            throw new InvalidChoiceException("Invalid choice, try again.");
                    }
                }
                catch (InvalidChoiceException e) { WriteCentered(e.Message); break; }
            }
        }
    }
}