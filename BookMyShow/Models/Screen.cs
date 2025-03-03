namespace BookMyShow.Models
{
    public class Screen
    {
        public int ScreenNumber;
        public List<Show> Shows = new List<Show>();

        public Screen(int screenNumber)
        {
            ScreenNumber = screenNumber;
        }
    }
}
