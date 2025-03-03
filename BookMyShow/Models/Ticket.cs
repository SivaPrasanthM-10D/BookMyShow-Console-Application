using BookMyShow.Implementations;
using BookMyShow.Interfaces;

namespace BookMyShow.Models
{
    public class Ticket : ITicket
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
            SeatNo = seatNo ?? new List<int>();
            TheatreName = theatreName;
            Price = price;
        }

        private static void WriteCentered(string text)
        {
            int windowWidth = 168;
            int textLength = text.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }

        public void DisplayTicket()
        {
            Theatre theatre = AdminOperations.GetTheatres().Find(t => t.Name.Equals(TheatreName, StringComparison.OrdinalIgnoreCase));
            WriteCentered("");
            WriteCentered("****************** Ticket Details ******************");
            WriteCentered($"  Movie     : {MovieName}");
            WriteCentered($"  Show Time : {ShowTime}");
            WriteCentered($"  Seat(s)   : {string.Join(",", SeatNo)}");
            WriteCentered($"  Theatre   : {TheatreName}, {theatre.Street}");
            WriteCentered($"  Price     : ₹{Price} (Includes GST)");
            WriteCentered("***************************************************");
        }
    }
}
