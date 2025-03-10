using System.Text;
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
            while (true)
            {
            welcome:
                Thread.Sleep(1000);
                Console.Clear();
                WriteCentered("Welcome to BookMyShow Console Application!\n");
                WriteCentered("1. Login");
                WriteCentered("2. Sign Up");
                WriteCentered("3. Exit");
                string choice = ReadCentered("Enter your choice:");
                switch (choice)
                {
                    case "1":
                    login:
                        try
                        {
                            Console.Clear();
                            WriteCentered("**Press (EXIT) to exit**\n");
                            WriteCentered("Login Page");
                            WriteCentered("");
                            string id = ReadCentered("Enter User ID:").Trim();
                            if (string.IsNullOrEmpty(id))
                            {
                                throw new InvalidUserException("User ID cannot be empty.");
                            }
                            if (id == "EXIT")
                            {
                                break;
                            }
                            string password = ReadCentered("Enter Password:").Trim();
                            if (string.IsNullOrEmpty(password))
                            {
                                throw new InvalidPasswordException("Password cannot be empty.");
                            }
                            if (password == "EXIT")
                            {
                                break;
                            }

                            User user = UserManagement.Login(id, password);

                            if (user is Admin admin)
                            {
                                AdminMenu(admin);
                            }
                            else if (user is TheatreOwner theatreowner)
                            {
                                TheatreOwnerMenu(theatreowner);
                            }
                            else if (user is Customer customer)
                            {
                                CustomerMenu(customer);
                            }
                            else
                            {
                                WriteCentered("Invalid login credentials.");
                                ReadCentered("Press any key to retry:");
                                goto login;
                            }
                        }
                        catch (InvalidPasswordException e) { WriteCentered(e.Message); goto welcome; }
                        catch (InvalidUserException e) { WriteCentered(e.Message); goto welcome; }
                        goto welcome;
                    case "2":
                    signup:
                        Thread.Sleep(500);
                        Console.Clear();
                        WriteCentered("");
                        WriteCentered("**Enter (EXIT) to exit**");
                        WriteCentered("1. Register Customer");
                        WriteCentered("2. Register Theatre Owner");
                        choice = ReadCentered("Enter your choice:");
                        if (choice == "EXIT")
                        {
                            goto welcome;
                        }
                        else
                        {
                            string nname, nid, npass, ncpass, phoneno;
                            switch (choice)
                            {
                                case "1":
                                    Thread.Sleep(500);
                                    Console.Clear();

                                    WriteCentered("Register new Customer!\n");
                                    WriteCentered("**Enter (EXIT) to exit**\n");
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
                                    if (nname == "EXIT") { goto signup; }
                                    while (true)
                                    {
                                        nid = ReadCentered("Your User ID:").Trim();
                                        if (string.IsNullOrEmpty(nid))
                                        {
                                            WriteCentered("User ID cannot be empty.");
                                        }
                                        else if (nid == "EXIT") { goto signup; }
                                        else if (UserManagement.GetCustomers().Any(c => c.Id.Equals(nid)))
                                        {
                                            WriteCentered("User ID already exists!");
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
                                            else if (npass == "EXIT") { goto signup; }
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
                                            ncpass = ReadCentered("Confirm your password:");
                                            if (!npass.Equals(ncpass) && ncpass != "EXIT")
                                            {
                                                throw new PasswordNotMatchException();
                                            }
                                            else if (ncpass == "EXIT") { goto signup; }
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
                                            if (string.IsNullOrEmpty(phoneno) || !UserManagement.IsValidPhoneNumber(phoneno) && phoneno != "EXIT")
                                            {
                                                throw new InvalidPhoneNoException("Invalid Phone No.");
                                            }
                                            else if (UserManagement.GetCustomers().Any(c => c.PhoneNo.Equals(phoneno)))
                                            {
                                                WriteCentered($"{phoneno} is already linked to another account.");
                                            }
                                            else if (phoneno == "EXIT") { goto signup; }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        catch (InvalidPhoneNoException e) { WriteCentered(e.Message); }
                                    }
                                    UserManagement.CustomerSignup(nid, npass, nname, phoneno);
                                    Console.WriteLine();
                                    WriteCentered("Customer Registration successful!");
                                    ReadCentered("Press any key to exit:");
                                    goto signup;
                                case "2":
                                    Thread.Sleep(500);
                                    Console.Clear();
                                    WriteCentered("Register new Theatre Owner!\n");
                                    WriteCentered("**Enter (EXIT) to exit**\n");
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
                                    if (nname == "EXIT") { goto signup; }
                                    while (true)
                                    {
                                        nid = ReadCentered("Your User ID:").Trim();
                                        if (string.IsNullOrEmpty(nid))
                                        {
                                            WriteCentered("User ID cannot be empty.");
                                        }
                                        else if (nid == "EXIT") { goto signup; }
                                        else if (UserManagement.GetTheatreOwners().Any(th => th.Id.Equals(nid)))
                                        {
                                            WriteCentered("User ID already exists!");
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
                                            else if (npass == "EXIT") { goto signup; }
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
                                            ncpass = ReadCentered("Confirm your password:");
                                            if (!npass.Equals(ncpass) && ncpass != "EXIT")
                                            {
                                                throw new PasswordNotMatchException();
                                            }
                                            else if (ncpass == "EXIT") { goto signup; }
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
                                            if (string.IsNullOrEmpty(phoneno) || !UserManagement.IsValidPhoneNumber(phoneno) && phoneno != "EXIT")
                                            {
                                                throw new InvalidPhoneNoException("Invalid Phone No.");
                                            }
                                            else if (UserManagement.GetTheatreOwners().Any(c => c.PhoneNo.Equals(phoneno)))
                                            {
                                                WriteCentered($"{phoneno} is already linked to another account.");
                                            }
                                            else if (phoneno == "EXIT") { goto signup; }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        catch (InvalidPhoneNoException e) { WriteCentered(e.Message); }
                                    }
                                    UserManagement.TheatreOwnerSignup(nid, npass, nname, phoneno);
                                    Console.WriteLine();
                                    WriteCentered("Theatre Owner Registration successful!");
                                    ReadCentered("Press any key to exit:");
                                    goto signup;
                                default:
                                    WriteCentered("Invalid choice, try again.");
                                    goto signup;
                            }
                        }
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
                WriteCentered("1. Add Movie");
                WriteCentered("2. Remove Movie");
                WriteCentered("3. View Reviews");
                WriteCentered("4. Remove Review");
                WriteCentered("5. Add Coupon");
                WriteCentered("6. View Profile");
                WriteCentered("7. View User Profiles");
                WriteCentered("8. Logout");
                string choice = ReadCentered("Enter your choice:");

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Clear();
                            WriteCentered("**Press (EXIT) to exit**\n");
                            WriteCentered("Enter the details to add a movie:-\n");
                            string title = ReadCentered("Enter movie title:");
                            if (title == "EXIT" || string.IsNullOrEmpty(title))
                            {
                                break;
                            }
                            if (AdminOperations.GetMovies().Any(m => m.Title.Equals(title, StringComparison.OrdinalIgnoreCase)))
                            {
                                throw new DuplicateMovieException("Movie already exists.");
                            }
                            string genre = ReadCentered("Enter genre:");
                            if (genre == "EXIT" || string.IsNullOrEmpty(genre))
                            {
                                break;
                            }
                            string dur = ReadCentered("Enter duration (minutes):");
                            if (dur == "EXIT" || string.IsNullOrEmpty(dur))
                            {
                                break;
                            }
                            int duration = int.Parse(dur);
                            AdminOperations.AddMovie(title, genre, duration);

                            ReadCentered("Press any key to exit:");
                            break;
                        }
                        catch(DuplicateMovieException e)
                        {
                            WriteCentered(e.Message);
                            ReadCentered("Press any key to exit:");
                            break;
                        }
                    case "2":
                        try
                        {
                            Console.Clear();
                            List<Movie> movielist = AdminOperations.GetMovies();
                            if (movielist.Count == 0)
                            {
                                throw new MovieNotFoundException("No movies found");
                            }
                            else
                            {
                                WriteCentered("**Press (EXIT) to exit**\n");
                                WriteCentered("Movies:\n");
                                foreach (var mv in movielist)
                                {
                                    WriteCentered($"{movielist.IndexOf(mv) + 1}. {mv.Title}");
                                }
                                Console.WriteLine();
                                string movieTitle = ReadCentered("Enter movie title to remove:");
                                if (movieTitle.Equals("EXIT"))
                                {
                                    break;
                                }
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
                                        Console.WriteLine();
                                        WriteCentered($"Movie removed successfully!");
                                        ReadCentered("Press any key to exit:");
                                    }
                                    else
                                    {
                                        WriteCentered("Movie removal cancelled.");
                                        ReadCentered("Press any key to exit:");
                                        break;
                                    }
                                }
                                else
                                {
                                    throw new MovieNotFoundException("Movie not found");
                                }
                            }
                            break;
                        }
                        catch(MovieNotFoundException e)
                        {
                            WriteCentered(e.Message);
                            ReadCentered("Press any key to exit:");
                            break;
                        }
                    case "3":
                        Console.Clear();
                        ReviewSystem.ViewReview();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "4":
                        Console.Clear();
                        ReviewSystem.RemoveReview();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "5":
                        try
                        {
                            Console.Clear();
                            WriteCentered("**Press (EXIT) to exit**\n");
                            string code = ReadCentered("Enter coupon code:").Trim();
                            if (string.IsNullOrEmpty(code))
                            {
                                throw new InvalidCouponException("Coupon code can't be empty.");
                            }
                            if (code == "EXIT")
                            {
                                break;
                            }
                            string dis = ReadCentered("Enter discount percentage:");
                            if (dis == "EXIT")
                            {
                                break;
                            }
                            if (!double.TryParse(dis, out double discount) || discount <= 0 || discount > 100)
                            {
                                throw new InvalidCouponException("Invalid coupon code. Try again.");
                            }

                            AdminOperations.AddCoupon(code, discount);
                            WriteCentered("Coupon added successfully!");
                            ReadCentered("Press any key to exit:");
                        }
                        catch (InvalidCouponException e)
                        {
                            WriteCentered(e.Message);
                            WriteCentered("Enter (EXIT) to exit:");
                            break;
                        }
                        break;
                    case "6":
                        Console.Clear();
                        WriteCentered("");
                        WriteCentered(".......Profile.......\n");
                        admin.DisplayUserInfo();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "7":
                    goback1:
                        Console.Clear();
                        WriteCentered("**Enter (EXIT) to exit**");
                        WriteCentered("1. View Customers");
                        WriteCentered("2. View Theatre Owners");
                        string choice2 = ReadCentered("Enter your choice:");
                        if (choice2 == "EXIT") { break; }
                        switch (choice2)
                        {
                            case "1":
                            rmore1:
                                List<Customer> customers = UserManagement.GetCustomers();
                                if (customers.Count == 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine();
                                    WriteCentered("No customers found.");
                                    ReadCentered("Press any key to exit:");
                                    goto goback1;
                                }
                                else
                                {
                                    Console.Clear();
                                    WriteCentered("List of Customers:\n");
                                    foreach (var customer in customers)
                                    {
                                        WriteCentered($"{customers.IndexOf(customer) + 1}. Name: {customer.Name} | ID: {customer.Id} | Phone No: {customer.PhoneNo}");
                                    }
                                    WriteCentered("");
                                rmore3:
                                    WriteCentered("1. Remove user");
                                    WriteCentered("2. Exit");
                                    string choice3 = ReadCentered("Enter your choice:");
                                    switch (choice3)
                                    {
                                        case "1":
                                            Console.WriteLine();
                                            string id = ReadCentered("Enter user ID to remove:");
                                            if (id == "EXIT") { goto rmore3; }
                                            if (!customers.Any(c => c.Id == id))
                                            {
                                                WriteCentered("Invalid user ID.");
                                                ReadCentered("Press any key to retry:");
                                                goto rmore1;
                                            }
                                            UserManagement.RemoveCustomer(id);
                                            WriteCentered("Customer removed successfully!");
                                            Thread.Sleep(1000);
                                            goto rmore1;
                                        case "2":
                                            Thread.Sleep(1000);
                                            goto goback1;
                                        default:
                                            Console.WriteLine();
                                            WriteCentered("Invalid choice, try again.");
                                            Thread.Sleep(1000);
                                            goto rmore1;
                                    }
                                }
                            case "2":
                            rmore2:
                                List<TheatreOwner> theatreOwners = UserManagement.GetTheatreOwners();
                                if (theatreOwners.Count == 0)
                                {
                                    Console.Clear();
                                    Console.WriteLine();
                                    WriteCentered("No theatre owners found.");
                                    ReadCentered("Press any key to exit:");
                                    goto goback1;
                                }
                                else
                                {
                                    Console.Clear();
                                    WriteCentered("List of Theatre Owners:");
                                    foreach (var theatreOwner in theatreOwners)
                                    {
                                        WriteCentered($"{theatreOwners.IndexOf(theatreOwner) + 1}. Name: {theatreOwner.Name} | ID: {theatreOwner.Id} | Phone No: {theatreOwner.PhoneNo}");
                                        if (theatreOwner.OwnedTheatre != null)
                                        {
                                            WriteCentered($"Owned Theatre: {theatreOwner.OwnedTheatre?.Name}, {theatreOwner.OwnedTheatre.Street}, {theatreOwner.OwnedTheatre.City}");
                                        }
                                        else
                                        {
                                            WriteCentered("No theatre is owned.");
                                        }
                                        WriteCentered($"");
                                    }
                                    WriteCentered("");
                                rmore4:
                                    WriteCentered("1. Remove theatre owner");
                                    WriteCentered("2. Exit");
                                    string choice4 = ReadCentered("Enter your choice:");
                                    switch (choice4)
                                    {
                                        case "1":
                                            Console.WriteLine();
                                            string id = ReadCentered("Enter user ID to remove:");
                                            if(id == "EXIT") { goto rmore4; }
                                            if (!theatreOwners.Any(to => to.Id.Equals(id)))
                                            {
                                                WriteCentered("Invalid user ID.");
                                                ReadCentered("Press any key to retry:");
                                                goto rmore2;
                                            }
                                            UserManagement.RemoveTheatreOwner(id);
                                            WriteCentered("Theatre owner and associated theatre removed successfully!");
                                            Thread.Sleep(1000);
                                            goto rmore2;
                                        case "2":
                                            Thread.Sleep(1000);
                                            goto goback1;
                                        default:
                                            Console.WriteLine();
                                            WriteCentered("Invalid choice, try again.");
                                            Thread.Sleep(1000);
                                            goto rmore2;
                                    }
                                }
                            default:
                                WriteCentered("Invalid choice, try again.");
                                Thread.Sleep(1000);
                                goto goback1;
                        }
                    case "8":
                        WriteCentered("Logging out!");
                        return;
                    default:
                        try { throw new InvalidChoiceException("Invalid choice, try again."); }
                        catch (InvalidChoiceException e) { WriteCentered(e.Message); Thread.Sleep(1000); break; }
                }
            }
        }

        public void TheatreOwnerMenu(TheatreOwner theatreOwner)
        {
            while (true)
            {
            theatreownermenu:
                Console.Clear();
                WriteCentered($"Welcome, {theatreOwner.Name}!");
                WriteCentered("");
                WriteCentered("Theatre Owner Menu");
                WriteCentered("");
                WriteCentered("1. Add Theatre");
                WriteCentered("2. Remove Theatre");
                WriteCentered("3. Add Show");
                WriteCentered("4. Remove Show");
                WriteCentered("5. View Reviews");
                WriteCentered("6. View Profile");
                WriteCentered("7. Logout");
                string choice = ReadCentered("Enter your choice:");
                string confirm;
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        try
                        {
                            WriteCentered("**Enter (EXIT) to exit**\n");
                            if (theatreOwner.OwnedTheatre != null) { throw new DuplicateMovieException("You already own a theatre!"); }
                            string tname = ReadCentered("Enter theatre name:");
                            if (tname == "EXIT" || string.IsNullOrEmpty(tname)) { break; }
                            string street = ReadCentered("Enter street:");
                            if (street == "EXIT" || string.IsNullOrEmpty(street)) { break; }
                            string city = ReadCentered("Enter city:");
                            if (city == "EXIT" || string.IsNullOrEmpty(city)) { break; }
                            string sn = ReadCentered("Enter number of screens:");
                            if (sn == "EXIT" || string.IsNullOrEmpty(sn)) { break; }
                            int seno = int.Parse(sn);
                            AdminOperations.AddTheatre(theatreOwner, tname, city, street, seno);
                            ReadCentered("Press any key to exit:");

                        }
                        catch (DuplicateMovieException e)
                        {
                            WriteCentered(e.Message);
                            Console.WriteLine();
                            WriteCentered("Your theatre details:");
                            WriteCentered($"Theatre Name: {theatreOwner.OwnedTheatre.Name}");
                            WriteCentered($"Street: {theatreOwner.OwnedTheatre.Street}");
                            WriteCentered($"City: {theatreOwner.OwnedTheatre.City}");
                            WriteCentered($"Number of Screens: {theatreOwner.OwnedTheatre.Screens.Count}\n");
                            ReadCentered("Press any key to exit:");
                            break;
                        }
                        break;
                    case "2":
                        Console.Clear();
                        if (theatreOwner.OwnedTheatre == null)
                        {
                            WriteCentered("No theatre owned.");
                            ReadCentered("Press any key to exit:");
                            break;
                        }
                        else
                        {
                            WriteCentered($"Theatre Name: {theatreOwner.OwnedTheatre.Name}");
                            WriteCentered($"Street: {theatreOwner.OwnedTheatre.Street}");
                            WriteCentered($"City: {theatreOwner.OwnedTheatre.City}");
                            WriteCentered($"Number of Screens: {theatreOwner.OwnedTheatre.Screens.Count}\n");
                            confirm = ReadCentered("Are you sure you want to remove this theatre? (yes/no):");
                            if (confirm.ToLower() == "yes")
                            {
                                AdminOperations.RemoveTheatre(theatreOwner.OwnedTheatre.Name);
                                theatreOwner.OwnedTheatre = null;
                                Console.WriteLine();
                                WriteCentered($"Theatre is successfully removed!");
                                ReadCentered("Press any key to exit:");
                            }
                            else
                            {
                                WriteCentered("Theatre removal cancelled.");
                                ReadCentered("Press any key to exit:");
                            }
                        }
                        break;
                    case "3":
                        try
                        {
                            Console.Clear();
                            if (theatreOwner.OwnedTheatre == null)
                            {
                                WriteCentered("No theatre owned.");
                                ReadCentered("Press any key to exit:");
                                break;
                            }

                            WriteCentered("**Enter (EXIT) to exit**\n");
                            WriteCentered($"{theatreOwner.OwnedTheatre.Name} - {theatreOwner.OwnedTheatre.Screens.Count} screens");

                            WriteCentered("Available Movies:");
                            foreach (var mv in AdminOperations.GetMovies())
                            {
                                WriteCentered($"{mv.Title}");
                            }

                            string stname = theatreOwner.OwnedTheatre.Name;
                            string sn = ReadCentered("Enter screen number:");
                            if (sn == "EXIT" || string.IsNullOrEmpty(sn)) { goto theatreownermenu; }
                            int ssno = int.Parse(sn);
                            string stitle = ReadCentered("Enter movie title:");
                            if (stitle == "EXIT" || string.IsNullOrEmpty(stitle)) { goto theatreownermenu; }
                            string startDateStr = ReadCentered("Enter start date (DD/MM/YYYY):");
                            if (startDateStr == "EXIT" || string.IsNullOrEmpty(startDateStr)) { goto theatreownermenu; }
                            string endDateStr = ReadCentered("Enter end date (DD/MM/YYYY):");
                            if (endDateStr == "EXIT" || string.IsNullOrEmpty(endDateStr)) { goto theatreownermenu; }
                            DateTime.TryParseExact(startDateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime startDate);
                            DateTime.TryParseExact(endDateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime endDate);
                            string st;
                            DateTime stime;
                            List<string> showtimings = new List<string> { "10:30 AM", "02:30 PM", "06:30 PM", "10:30 PM" };
                            StringBuilder stimings = new StringBuilder();

                            foreach (var time in showtimings)
                            {
                                DateTime parsedTime;
                                if (DateTime.TryParseExact(time, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out parsedTime)) { }
                                bool isUnavailable = false;
                                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                                {
                                    if (AdminOperations.ShowExists(theatreOwner, stname, ssno, parsedTime, date))
                                    {
                                        isUnavailable = true;
                                        break;
                                    }
                                }
                                if (isUnavailable)
                                {
                                    stimings.Append($"{time} (Unavailable) | ");
                                }
                                else
                                {
                                    stimings.Append($"{time} | ");
                                }
                            }

                            while (true)
                            {
                                WriteCentered(stimings.ToString().TrimEnd(' ', '|'));
                                st = ReadCentered("Enter show time (HH:MM AM/PM):");
                                if (st == "EXIT" || string.IsNullOrEmpty(st)) { goto theatreownermenu; }
                                if (!DateTime.TryParseExact(st, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out stime) ||
                                    !showtimings.Contains(st))
                                {
                                    throw new InvalidShowtimeException("Invalid show time. Allowed show times are 10:30 AM, 02:30 PM, 06:30 PM, and 10:30 PM.");
                                }

                                bool isShowAvailable = true;
                                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                                {
                                    if (AdminOperations.ShowExists(theatreOwner, stname, ssno, stime, date))
                                    {
                                        isShowAvailable = false;
                                        break;
                                    }
                                }

                                if (!isShowAvailable)
                                {
                                    WriteCentered("Could not add show. Show time is unavailable for the selected dates.");
                                    ReadCentered("Press any key to exit:");
                                    goto theatreownermenu;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            string seatstr;
                            int savlseats;
                            while (true)
                            {
                                seatstr = ReadCentered("Enter available seats (100 - 160):");
                                if (seatstr == "EXIT" || string.IsNullOrEmpty(seatstr)) { goto theatreownermenu; }
                                if (!int.TryParse(seatstr, out savlseats) || savlseats > 160 || savlseats < 100)
                                {
                                    throw new InvalidSeatNoException("Invalid input! Valid total seat number between 100 and 160.");
                                }
                                else
                                {
                                    break;
                                }
                            }
                            List<int> savailableseats = [];
                            for (int i = 1; i <= savlseats; i++)
                            {
                                savailableseats.Add(i);
                            }
                            string tkprstr = ReadCentered("Enter ticket price:");
                            if (tkprstr == "EXIT" || string.IsNullOrEmpty(tkprstr)) { goto theatreownermenu; }
                            double tktprice = double.Parse(tkprstr);

                            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                            {
                                if (AdminOperations.ShowExists(theatreOwner, stname, ssno, stime, date))
                                {
                                    WriteCentered($"Show already exists on {date:dd/MM/yyyy} at {stime:hh:mm tt}");
                                    ReadCentered("Press any key to skip adding this show and continue with the next date:");
                                    continue;
                                }
                                else
                                {
                                    AdminOperations.AddShow(theatreOwner, stname, ssno, stitle, stime, date, savlseats, savailableseats, tktprice);
                                }
                            }
                            WriteCentered($"Show successfully added: {stitle} at {stname}, Screen {ssno} - Show Time : {stime:hh:mm tt}({startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy})");
                            ReadCentered("Press any key to exit:");
                        }
                        catch (InvalidSeatNoException e) { WriteCentered($"{e.Message}"); }
                        catch (DuplicateShowException e) { WriteCentered($"{e.Message}"); }
                        catch (InvalidShowtimeException e) { WriteCentered($"{e.Message}"); }
                        break;
                    case "4":
                        Console.Clear();
                        if (theatreOwner.OwnedTheatre == null)
                        {
                            WriteCentered("No theatre owned.");
                            ReadCentered("Press any key to exit:");
                            break;
                        }
                        if (!AdminOperations.DisplayShows(theatreOwner))
                        {
                            WriteCentered("No shows found.");
                            ReadCentered("Press any key to exit:");
                            break;
                        }
                        WriteCentered("**Enter (EXIT) to exit**\n");
                        string sno = ReadCentered("Enter screen number:");
                        if (sno == "EXIT" || string.IsNullOrEmpty(sno)) { break; }
                        int rScreenNo = int.Parse(sno);
                        string rMovieTitle = ReadCentered("Enter movie title:");
                        if (rMovieTitle == "EXIT" || string.IsNullOrEmpty(rMovieTitle)) { break; }
                        string rSD = ReadCentered("Enter show date (DD/MM/YYYY):");
                        if (rSD == "EXIT" || string.IsNullOrEmpty(rSD)) { break; }
                        DateTime.TryParseExact(rSD, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime rShowDate);
                        string rST = ReadCentered("Enter show time (HH:MM AM/PM):");
                        if (rST == "EXIT" || string.IsNullOrEmpty(rST)) { break; }
                        DateTime.TryParseExact(rST, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out DateTime rShowTime);
                        Show? show = AdminOperations.GetTheatres()
                                    .Find(t => t.Name.Equals(theatreOwner.OwnedTheatre.Name, StringComparison.OrdinalIgnoreCase))?
                                    .Screens.Find(s => s.ScreenNumber == rScreenNo)?
                                    .Shows.Find(s => s.Movie.Title.Equals(rMovieTitle, StringComparison.OrdinalIgnoreCase) && s.ShowTime == rShowTime.ToString("hh:mm tt") && s.ShowDate == rShowDate.ToString("dd/MM/yyyy"));
                        if (show != null)
                        {
                            var displayedShows = new HashSet<string>();
                            string showKey = show.Movie.Title + show.ShowTime + show.ShowDate;
                            if (!displayedShows.Contains(showKey))
                            {
                                WriteCentered($"Movie Title: {show.Movie.Title}");
                                WriteCentered($"Show Time: {show.ShowTime}");
                                WriteCentered($"Show Date: {show.ShowDate}");
                                WriteCentered($"Total Seats: {show.TotalSeats}");
                                WriteCentered($"Available Seats: {show.AvailableSeats.Count}");
                                WriteCentered($"Ticket Price: ₹{show.TicketPrice}");
                                displayedShows.Add(showKey);
                            }
                        }
                        else
                        {
                            WriteCentered("Show not available to remove.");
                            ReadCentered("Press any key to exit:");
                            goto theatreownermenu;
                        }
                        confirm = ReadCentered("Are you sure you want to remove this show? (yes/no):");
                        if (confirm == "yes")
                        {
                            AdminOperations.RemoveShow(theatreOwner.OwnedTheatre.Name, rScreenNo, rMovieTitle, rShowTime, rShowDate);
                            if (!AdminOperations.DisplayShows(theatreOwner))
                            {
                                WriteCentered("No shows found.");
                                ReadCentered("Press any key to exit:");
                                break;
                            }
                            ReadCentered("Press any key to exit:");
                        }
                        else
                        {
                            WriteCentered("Show removal cancelled.");
                            ReadCentered("Press any key to exit:");
                        }
                        break;
                    case "5":
                        Console.Clear();
                        ReviewSystem.ViewReview();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "6":
                        Console.Clear();
                        WriteCentered("");
                        WriteCentered(".......Profile.......\n");
                        theatreOwner.DisplayUserInfo();
                        ReadCentered("Press any key to exit:");
                        break;
                    case "7":
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
                CustomerMenu:
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
                                WriteCentered("**Enter (EXIT) to exit**\n");
                                string searchcity = ReadCentered("Enter city:");
                                if (searchcity == "EXIT" || string.IsNullOrEmpty(searchcity)) { goto CustomerMenu; }
                                List<Theatre> theatresincity = AdminOperations.GetTheatres().FindAll(t => t.City.Equals(searchcity, StringComparison.OrdinalIgnoreCase));
                                if (theatresincity.Count == 0)
                                {
                                    throw new TheatreNotFoundException($"No theatres found in {searchcity}");
                                }


                                Console.WriteLine();
                                string chosenshowdate = ReadCentered("Enter show date (DD/MM/YYYY):");
                                if (chosenshowdate == "EXIT" || string.IsNullOrEmpty(chosenshowdate)) { goto CustomerMenu; }

                                if (!DateTime.TryParseExact(chosenshowdate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime showDate))
                                {
                                    throw new InvalidShowtimeException("Invalid date format. Use DD/MM/YYYY for date.");
                                }

                                var showsOnDate = theatresincity.SelectMany(t => t.Screens)
                                                                .SelectMany(s => s.Shows)
                                                                .Where(sh => sh.ShowDate.Equals(showDate.ToString("dd/MM/yyyy"))).Distinct()
                                                                .ToList();

                                if (showsOnDate.Count == 0)
                                {
                                    WriteCentered($"No shows found on {showDate:dd/MM/yyyy}");
                                    ReadCentered("Press any key to exit:");
                                    goto CustomerMenu;
                                }
                                else
                                {
                                        var showsGroupedByTheatre = showsOnDate.GroupBy(show => show.Theatre.Name);
                                        foreach (var theatreGroup in showsGroupedByTheatre)
                                        {
                                            WriteCentered($"Theatre: {theatreGroup.Key}");
                                            foreach (var show in theatreGroup)
                                            {
                                                WriteCentered($"  Movie: {show.Movie.Title} | Screen: {show.Theatre.Screens.First(s => s.Shows.Contains(show)).ScreenNumber} | Show Date: {show.ShowDate:dd/MM/yyyy} | Show Time: {show.ShowTime:hh:mm tt} | Available Seats: {show.AvailableSeats.Count} | Ticket Price: Rs {show.TicketPrice}");
                                            }
                                            WriteCentered("");
                                        }
                                }

                                Console.WriteLine();
                                string selectedtheatre = ReadCentered("Enter theatre name:");
                                if (selectedtheatre == "EXIT" || string.IsNullOrEmpty(selectedtheatre)) { goto CustomerMenu; }
                                Theatre? chosentheatre = theatresincity.Find(t => t.Name.Equals(selectedtheatre, StringComparison.OrdinalIgnoreCase));

                                if (chosentheatre == null)
                                {
                                    throw new InvalidTheatreNameException("Invalid theatre name.");
                                }

                                string chosenmovie = ReadCentered("Enter movie name:");
                                if (chosenmovie == "EXIT" || string.IsNullOrEmpty(chosenmovie)) { goto CustomerMenu; }
                                string scno = ReadCentered("Enter screen number:");
                                if(scno == "EXIT" || string.IsNullOrEmpty(scno)) { goto CustomerMenu; }
                                int screenno = int.Parse(scno);
                                string chosenshowtime = ReadCentered("Enter show time (HH:MM AM/PM):");
                                if (chosenshowtime == "EXIT" || string.IsNullOrEmpty(chosenshowtime)) { goto CustomerMenu; }

                                if (!DateTime.TryParseExact(chosenshowtime, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out DateTime _))
                                {
                                    throw new InvalidShowtimeException("Invalid time. Use HH:MM AM/PM for time.");
                                }
                                Show? chosenshow = chosentheatre.Screens.SelectMany(s => s.Shows).FirstOrDefault(sh => sh.Movie.Title.Equals(chosenmovie, StringComparison.OrdinalIgnoreCase) &&
                                                                                     sh.ShowTime.Equals(chosenshowtime, StringComparison.OrdinalIgnoreCase) &&
                                                                                     sh.ShowDate.Equals(showDate.ToString("dd/MM/yyyy")) &&
                                                                                     sh.Theatre.Screens.Any(s => s.ScreenNumber == screenno));

                                if (chosenshow == null)
                                {
                                    throw new InvalidShowtimeException("Invalid movie, show time, or show date selection.");
                                }

                                BookingSystem.DisplaySeats(chosenshow);
                                string noseats = ReadCentered("Enter number of seats to book:");
                                if (noseats == "EXIT" || string.IsNullOrEmpty(noseats)) { goto CustomerMenu; }
                                if (!int.TryParse(noseats, out int nos) || nos <= 0)
                                {
                                    throw new SeatNotAvailableException("Invalid Input. Please enter a valid number.");
                                }
                                List<int> seatnumbers = new List<int>();
                                string seatnos;
                                for (int i = 0; i < nos; i++)
                                {
                                    seatnos = ReadCentered($"Enter seat number {i + 1}:");
                                    if (seatnos == "EXIT" || string.IsNullOrEmpty(seatnos)) { seatnumbers.Clear(); goto CustomerMenu; }
                                    if (!int.TryParse(seatnos, out int seat) || seat <= 0)
                                    {
                                        i--;
                                        throw new InvalidSeatNoException("Invalid seat number. Please try again.");
                                    }
                                    else
                                    {
                                        seatnumbers.Add(seat);
                                    }
                                }
                                if(BookingSystem.BookTicket(customer, chosenshow,screenno, seatnumbers))
                                {
                                    BookingSystem.DisplaySeats(chosenshow);
                                    ReadCentered("Press any key for customer menu:");
                                }
                                else
                                {
                                    BookingSystem.DisplaySeats(chosenshow);
                                    goto CustomerMenu;
                                }
                            }
                            catch (InvalidTheatreNameException e) { WriteCentered(e.Message); ReadCentered("Press any key to exit:"); break; }
                            catch (InvalidShowtimeException e) { WriteCentered(e.Message); ReadCentered("Press any key to exit:"); break; }
                            catch (InvalidSeatNoException e) { WriteCentered(e.Message); }
                            catch (SeatNotAvailableException e) { WriteCentered(e.Message); ReadCentered("Press any key to exit:"); break; }
                            catch (TheatreNotFoundException e) { WriteCentered(e.Message); ReadCentered("Press any key to exit:"); break; }

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
                                    ReadCentered("Press any key for customer menu:");
                                }
                            }
                            catch (TicketNotFoundException e) { WriteCentered(e.Message); ReadCentered("Press any key to exit:"); goto CustomerMenu; }
                            break;
                        case "3":
                            Console.Clear();
                            if (!BookingSystem.CancelTicket(customer))
                            {
                                goto CustomerMenu;
                            }
                            WriteCentered("Ticket cancelled successfully!");
                            ReadCentered("Press any key for customer menu:");
                            break;
                        case "4":
                            Console.Clear();
                            ReviewSystem.AddReview(customer);
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