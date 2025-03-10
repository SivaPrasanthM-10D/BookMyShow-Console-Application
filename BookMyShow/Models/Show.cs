namespace BookMyShow.Models
{
    public class Show
    {
        public Movie Movie;
        public string ShowTime;
        public string ShowDate;
        public List<int> AvailableSeats;
        public Theatre Theatre;
        public double TicketPrice;
        public List<int> BookSeats = new List<int>();
        public int TotalSeats;

        public Show(Movie movie, DateTime showTime, DateTime showDate, int totalseats, List<int> availableSeats, Theatre theatre, double ticketPrice)
        {
            Movie = movie;
            ShowTime = showTime.ToString("hh:mm tt");
            ShowDate = showDate.ToString("dd/MM/yyyy");
            AvailableSeats = availableSeats;
            Theatre = theatre;
            TicketPrice = ticketPrice;
            TotalSeats = totalseats;
        }
    }
}
