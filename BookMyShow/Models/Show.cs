
namespace BookMyShow.Models
{
    public class Show
    {
        public Movie Movie;
        public string ShowTime;
        public int AvailableSeats;
        public Theatre Theatre;
        public double TicketPrice;
        public List<int> BookSeats = new List<int>();

        public Show(Movie movie, DateTime showTime, int availableSeats, Theatre theatre, double ticketPrice)
        {
            Movie = movie;
            ShowTime = showTime.ToString("hh:mm tt");
            AvailableSeats = availableSeats;
            Theatre = theatre;
            TicketPrice = ticketPrice;
            //if (!DateTime.TryParseExact(showTime, "hh:mm tt", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
            //{
            //    throw new ArgumentException("Invalid show time format. Use HH:MM AM/PM format.");
            //}
        }
    }
}
