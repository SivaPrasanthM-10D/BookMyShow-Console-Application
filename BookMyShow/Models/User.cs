namespace BookMyShow.Models
{
    public abstract class User
    {
        public string Id;
        public string Password;
        public string Name;
        public string PhoneNo;

        protected User(string id, string password, string name, string phoneNo)
        {
            Id = id;
            Password = password;
            Name = name;
            PhoneNo = phoneNo;
        }
        public static void WriteCentered(string text)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = text.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }
        public abstract void DisplayUserInfo();
    }

    public class Admin : User
    {
        public Admin(string id, string password, string name, string phoneNo) : base(id, password, name, phoneNo)
        {
        }

        public override void DisplayUserInfo()
        {
            WriteCentered($"Admin Info:");
            WriteCentered($"ID: {Id}");
            WriteCentered($"Name: {Name}");
            WriteCentered($"Phone No: {PhoneNo}");
        }
    }

    public class Customer : User
    {
        public List<Ticket> BookedTickets = new List<Ticket>();
        public string UpiId;
        public int UpiPin;

        public Customer(string id, string password, string name, string phoneNo, string upiid, string upipin) : base(id, password, name, phoneNo)
        {
            if (!int.TryParse(upipin, out int pin) || (pin < 100000 || pin > 999999))
            {
                throw new ArgumentException("UPI Pin must be a 6 digit number.");
            }
            UpiId = upiid;
            UpiPin = pin;
        }

        public override void DisplayUserInfo()
        {
            WriteCentered($"Customer Info:");
            WriteCentered($"ID: {Id}");
            WriteCentered($"Name: {Name}");
            WriteCentered($"Phone No: {PhoneNo}");
            WriteCentered($"UPI ID: {UpiId}");
            WriteCentered("Booked Tickets:");
            foreach (var ticket in BookedTickets)
            {
                ticket.DisplayTicket();
            }
        }
    }
}
