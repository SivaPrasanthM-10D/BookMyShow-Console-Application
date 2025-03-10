using System.Text.RegularExpressions;
using BookMyShow.Models;

namespace BookMyShow.Implementations
{
    public static class UserManagement
    {
        private static List<User> Users = new List<User>()
            {
                new Admin("admin","admin123","Admin","9500913678"),
                new Customer("c1","c123","Customer1","9965168135"),
                new Customer("c2","c123","Customer2","9965168135"),
                new TheatreOwner("siva","Siva@123","Siva","9123456789")
            };

        public static bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.])[A-Za-z\d@$!%*?&.]{8,15}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(password);
        }
        public static bool IsValidUpiId(string email)
        {
            string pattern = @"^[a-z0-9._-]+@[a-z]+$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }

        public static bool IsValidPhoneNumber(string phoneno)
        {
            string pattern = @"^[6-9]\d{9}$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(phoneno);
        }

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

        public static void CustomerSignup(string id, string password, string name, string phoneno)
        {
            Users.Add(new Customer(id, password, name, phoneno));
        }

        public static void TheatreOwnerSignup(string id, string password, string name, string phoneno)
        {
            Users.Add(new TheatreOwner(id, password, name, phoneno));
        }

        public static List<Customer> GetCustomers()
        {
            return Users.OfType<Customer>().ToList();
        }

        public static List<TheatreOwner> GetTheatreOwners()
        {
            return Users.OfType<TheatreOwner>().ToList();
        }

        public static void RemoveCustomer(string id)
        {
            User? user = Users.Find(u => u.Id == id);
            if (user is Customer customer)
            {
                foreach (var ticket in customer.BookedTickets)
                {
                    Show? show = AdminOperations.GetShows().Find(s => s.Movie.Title.Equals(ticket.MovieName, StringComparison.OrdinalIgnoreCase) &&
                                                                      s.ShowTime == ticket.ShowTime &&
                                                                      s.ShowDate == ticket.ShowDate &&
                                                                      s.Theatre.Name.Equals(ticket.TheatreName));
                    if (show != null)
                    {
                        show.AvailableSeats.AddRange(ticket.SeatNo); // Add seats back to available seats
                        show.BookSeats.RemoveAll(s => ticket.SeatNo.Contains(s));
                    }
                }
            }
            if (user != null)
            {
                Users.Remove(user);
            }
        }

        public static void RemoveTheatreOwner(string id)
        {
            TheatreOwner owner = Users.OfType<TheatreOwner>().FirstOrDefault(to => to.Id == id);
            if (owner != null)
            {
                if (owner.OwnedTheatre != null)
                {
                    foreach (var screen in owner.OwnedTheatre.Screens)
                    {
                        foreach (var show in screen.Shows)
                        {
                            BookingSystem.RemoveBookings(show);
                        }
                    }
                    AdminOperations.RemoveTheatre(owner.OwnedTheatre.Name);
                }
                Users.Remove(owner);
            }
        }

    }
}
