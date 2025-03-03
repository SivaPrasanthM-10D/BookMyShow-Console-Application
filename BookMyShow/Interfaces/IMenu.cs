using BookMyShow.Models;
namespace BookMyShow.Interfaces
{
    interface IMenu
    {
        public void AdminMenu(Admin admin);
        public void CustomerMenu(Customer customer);
    }
}