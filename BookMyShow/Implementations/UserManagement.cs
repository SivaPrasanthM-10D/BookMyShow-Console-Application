using System.Text.RegularExpressions;
using BookMyShow.Models;

namespace BookMyShow.Implementations
{
    public static class UserManagement
    {
        private static void WriteCentered(string text)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = text.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }

        private static List<User> Users = new List<User>()
            {
                new Admin("admin","admin123","Admin","9500913678"),
                new Customer("c1","c123","Customer1","9965168135","c1@gmail.com","876543")
            };

        public static bool IsValidUpiId(string email)
        {
            string pattern = @"^[a-z0-9._%+-]+@[a-z]+$";
            Regex regex = new Regex(pattern);

            return regex.IsMatch(email);
        }

        public static bool IsValidPhoneNumber(string phoneno)
        {
            string pattern = @"(^[6-9]\d{9}$)|(^\+[0-9]{2}\s+[0-9]{2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)";
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

        public static void Signup(string id, string password, string name, string phoneno, string upiid, string upipin)
        {
            Users.Add(new Customer(id, password, name, phoneno, upiid, upipin));
            WriteCentered("User registered successfully.");
        }
    }
}
