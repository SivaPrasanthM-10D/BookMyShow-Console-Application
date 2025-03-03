using BookMyShow.Custom_Exceptions;
using BookMyShow.Interfaces;
using BookMyShow.Models;

namespace BookMyShow.Implementations
{
    public class Menu : IMenu
    {
        private void WriteCentered(string text)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = text.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }

        private string ReadCentered(string prompt)
        {
            int windowWidth = Console.WindowWidth;
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
                        Console.Clear();
                        WriteCentered("Login Page");
                        string id = ReadCentered("Enter User ID:").Trim();
                        if (string.IsNullOrEmpty(id))
                        {
                            WriteCentered("User ID cannot be empty.");
                            break;
                        }
                        string password = ReadCentered("Enter Password:").Trim();
                        if (string.IsNullOrEmpty(password))
                        {
                            WriteCentered("Password cannot be empty.");
                            break;
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
                        break;
                    case "2":
                        Console.Clear();
                        WriteCentered("Register Now!");
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
                            npass = ReadCentered("Enter new password:").Trim();
                            if (string.IsNullOrEmpty(npass))
                            {
                                WriteCentered("Password cannot be empty.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        while (true)
                        {
                            if (!npass.Equals(ReadCentered("Confirm your password:")))
                            {
                                WriteCentered("Password does not match.");
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

                        UserManagement.Signup(nid, npass, nname, phoneno, upiid, upipin);
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
                        string tname = ReadCentered("Enter theatre name:");
                        string city = ReadCentered("Enter city:");
                        string street = ReadCentered("Enter street:");
                        int sno = int.Parse(ReadCentered("Enter number of screens:"));
                        AdminOperations.AddTheatre(tname, city, street, sno);
                        break;
                    case "2":
                        string title = ReadCentered("Enter movie title:");
                        string genre = ReadCentered("Enter genre:");
                        int duration = int.Parse(ReadCentered("Enter duration (minutes):"));
                        AdminOperations.AddMovie(title, genre, duration);
                        break;
                    case "3":
                        string stname = ReadCentered("Enter theatre name:");
                        int ssno = int.Parse(ReadCentered("Enter screen number:"));
                        string stitle = ReadCentered("Enter movie title:");
                        string st = ReadCentered("Enter show time (HH:MM AM/PM):");
                        int savlseats;
                        while (true)
                        {
                            if (!int.TryParse(ReadCentered("Enter available seats (100 - 160):"), out savlseats) || savlseats > 160 || savlseats < 100)
                            {
                                WriteCentered("Invalid input! Valid total seat number between 100 and 160.");
                            }
                            else
                            {
                                break;
                            }
                        }
                        double tktprice = double.Parse(ReadCentered("Enter ticket price:"));

                        DateTime.TryParseExact(st, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out DateTime stime);
                        AdminOperations.AddShow(stname, ssno, stitle, stime, savlseats, tktprice);
                        break;
                    case "4":
                        ReviewSystem.ViewReview();
                        break;
                    case "5":
                        ReviewSystem.RemoveReview();
                        break;
                    case "6":
                        string code = ReadCentered("Enter coupon code:").Trim();
                        if (string.IsNullOrEmpty(code))
                        {
                            WriteCentered("Coupon code can't be empty.");
                            break;
                        }
                        if (!double.TryParse(ReadCentered("Enter discount percentage:"), out double discount) || discount <= 0 || discount > 100)
                        {
                            WriteCentered("Invalid coupon code. Try again.");
                            break;
                        }
                        AdminOperations.AddCoupon(code, discount);
                        break;
                    case "7":
                        WriteCentered("");
                        WriteCentered(".......Profile.......\n");
                        admin.DisplayUserInfo();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "8":
                        WriteCentered("Logging out!");
                        return;
                    default:
                        WriteCentered("Invalid choice, try again.");
                        break;
                }
            }
        }

        public void CustomerMenu(Customer customer)
        {
            ReviewSystem rs = new ReviewSystem();
            while (true)
            {
                rs.DisplayMovies();
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
                        string searchcity = ReadCentered("Enter city:");
                        List<Theatre> theatresincity = AdminOperations.GetTheatres().FindAll(t => t.City.Equals(searchcity, StringComparison.OrdinalIgnoreCase));
                        if (theatresincity.Count == 0)
                        {
                            WriteCentered($"No theatres found in {searchcity}");
                            break;
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
                            WriteCentered("Invalid theatre name.");
                            break;
                        }

                        string chosenmovie = ReadCentered("Enter movie name:");
                        string chosenshowtime = ReadCentered("Enter show time (HH:MM AM/PM):");

                        if (!DateTime.TryParseExact(chosenshowtime, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out DateTime _))
                        {
                            WriteCentered("Invalid time format. Use HH:MM AM/PM.");
                            break;
                        }
                        Show? chosenshow = chosentheatre.Screens.SelectMany(s => s.Shows).FirstOrDefault(sh => sh.Movie.Title.Equals(chosenmovie, StringComparison.OrdinalIgnoreCase) &&
                                                                         sh.ShowTime.Equals(chosenshowtime, StringComparison.OrdinalIgnoreCase));

                        if (chosenshow == null)
                        {
                            WriteCentered("Invalid movie or show time selection.");
                            break;
                        }

                        BookingSystem.DisplaySeats(chosenshow);
                        if (!int.TryParse(ReadCentered("Enter number of seats to book:"), out int nos) || nos <= 0)
                        {
                            WriteCentered("Invalid Input. Please enter a valid number.");
                            break;
                        }
                        List<int> seatnumbers = new List<int>();
                        for (int i = 0; i < nos; i++)
                        {
                            if (!int.TryParse(ReadCentered($"Enter seat number {i + 1}:"), out int seat) || seat <= 0)
                            {
                                WriteCentered("Invalid seat number. Please try again.");
                                i--;
                            }
                            else
                            {
                                seatnumbers.Add(seat);
                            }
                        }
                        BookingSystem.BookTicket(customer, chosenshow, seatnumbers);
                        BookingSystem.DisplaySeats(chosenshow);
                        break;
                    case "2":
                        if (customer.BookedTickets.Count == 0)
                        {
                            WriteCentered("No booked tickets found.");
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
                        if (!BookingSystem.CancelTicket(customer))
                        {
                            rs.DisplayMovies();
                        }
                        break;
                    case "4":
                        ReviewSystem.AddReview(customer);
                        break;
                    case "5":
                        ReviewSystem.ViewReview();
                        break;
                    case "6":
                        WriteCentered("");
                        WriteCentered(".......Profile.......\n");
                        customer.DisplayUserInfo();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "7":
                        WriteCentered("Logging out!");
                        return;
                    default:
                        WriteCentered("Invalid choice, try again.");
                        break;
                }
            }
        }
    }
}