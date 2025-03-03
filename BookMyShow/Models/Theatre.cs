
namespace BookMyShow.Models
{
    public class Theatre
    {
        public string Name;
        public string City;
        public string Street;
        public List<Screen> Screens = new List<Screen>();

        public Theatre(string name, string city, string street)
        {
            Name = name;
            City = city;
            Street = street;
        }
    }
}
