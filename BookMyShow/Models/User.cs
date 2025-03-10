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
            int windowWidth = 168;
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

    public class TheatreOwner : User
    {
        public Theatre? OwnedTheatre;
        public TheatreOwner(string id, string password, string name, string phoneNo) : base(id, password, name, phoneNo)
        {
        }
        public override void DisplayUserInfo()
        {
            WriteCentered($"Theatre Owner Info:");
            WriteCentered($"ID: {Id}");
            WriteCentered($"Name: {Name}");
            WriteCentered($"Phone No: {PhoneNo}");
            WriteCentered("Owned Theatre:");
            if (OwnedTheatre != null)
            {
                WriteCentered($"Name : {OwnedTheatre.Name} | Address : {OwnedTheatre.Street}, {OwnedTheatre.City}");
            }
            else
            {
                WriteCentered("No theatre is owned!\n");
            }
        }
    }


    public class Customer : User
    {
        public List<Ticket> BookedTickets = new List<Ticket>();
        public List<string> upipins = ["876543", "123456", "567890", "987654"];
        public Customer(string id, string password, string name, string phoneNo) : base(id, password, name, phoneNo)
        {
        }

        public override void DisplayUserInfo()
        {
            WriteCentered($"Customer Info:");
            WriteCentered($"ID: {Id}");
            WriteCentered($"Name: {Name}");
            WriteCentered($"Phone No: {PhoneNo}");
            WriteCentered("Booked Tickets:");
            foreach (var ticket in BookedTickets)
            {
                ticket.DisplayTicket();
            }
        }
    }
}
