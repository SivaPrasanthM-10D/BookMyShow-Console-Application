using static BookMyShow.Ticket;

namespace BookMyShow.Implementations
{
    public static class BookingSystem
    {
        public static void DisplaySeats(Show show)
        {
            Console.WriteLine("\n        Seat Layout:");
            Console.WriteLine("        =====================================");
            Console.WriteLine("\n        _________________________________");
            Console.WriteLine("       /                                 \\");
            Console.WriteLine();

            int totalseats = show.AvailableSeats + show.BookSeats.Count;
            for (int seat = 1; seat <= totalseats; seat++)
            {
                string seatNumber = seat.ToString();
                seatNumber = seat < 10 ? "00" + seat : (seat < 100 ? "0" + seat : seat.ToString());
                if (show.BookSeats.Contains(seat))
                {
                    Console.Write("[ X ]");
                }
                else
                {
                    Console.Write($"[{seatNumber}]");
                }
                if (seat % 10 == 0) Console.WriteLine();
            }

            Console.WriteLine("        =====================================\n");
        }

        public static double ApplyDiscount(double totalprice, int seatcount)
        {
            Console.Write("Do you have coupon code? {y/n}:");
            string inp = Console.ReadLine().ToUpper();

            double discount = 0;
            if (seatcount >= 3)
            {
                discount += 0.10;
                Console.WriteLine("Bulk discount applied (10% off)!");
            }

            if (inp == "Y")
            {
                Dictionary<string, double> Coupons = AdminOperations.GetCoupons();
                Console.Write("Enter the coupon code:");
                string coupon = Console.ReadLine().ToUpper();

                if (coupon == "MOVIE10")
                {
                    discount += 0.10;
                    Console.WriteLine("Promo code MOVIE10 applied! Extra 10% Off.");
                }
                else if (Coupons.ContainsKey(coupon))
                {
                    discount += Coupons[coupon] / 100;
                    Console.WriteLine($"Coupon code {coupon} applied! Extra {Coupons[coupon]}% Off.");
                }
                else
                {
                    Console.WriteLine("Invalid coupon code.");
                }
            }

            double finprice = totalprice - discount;
            return finprice;
        }

        public static bool PaymentGateway(Customer customer, Show show, List<int> seatNumbers, double totalprice)
        {
            int retry = 3;
        x:
            Console.WriteLine("\t\t\t\t\t\t\t\t PAYMENT GATEWAY");
            Console.WriteLine($"\n\nInitiating payment from UPI ID: {customer.UpiId}");
            Console.Write("Enter your 6-digit UPI Pin:");

            if (!int.TryParse(Console.ReadLine(), out int upipin) || upipin < 100000 || upipin > 999999)
            {

                retry--;
                if (retry == 0)
                {
                    Console.WriteLine("\n\nEntered wrong pin too many times. Payment failed.");
                    return false;
                }
                else
                {
                    Console.WriteLine("\n\nInvalid UPI Pin format. Enter correct PIN.");
                    goto x;
                }
            }
            if (upipin == customer.UpiPin)
            {
                show.AvailableSeats -= seatNumbers.Count;
                show.BookSeats.AddRange(seatNumbers);
                var ticket = new Ticket(show.Movie.Title, show.ShowTime, seatNumbers, show.Theatre.Name, totalprice);
                customer.BookedTickets.Add(ticket);
                Console.WriteLine("\n\nPayment Successful! Your ticket(s) have been booked.");
                ticket.DisplayTicket();
                return true;
            }
            else
            {
                Console.WriteLine("\n\nIncorrect UPI PIN! Enter correct PIN.");
                retry--;
                if (retry == 0)
                {
                    Console.WriteLine("\n\nEntered wrong pin too many times. Payment failed!");
                    return false;
                }
                else goto x;
            }
        }

        public static bool BookTicket(Customer customer, Show show, List<int> seatNumbers)
        {
            foreach (var seat in seatNumbers)
            {
                if (show.BookSeats.Contains(seat))
                {
                    Console.WriteLine($"Seat {seat} already booked. Please choose another seat.");
                    return false;
                }
            }

            double totalPrice = show.TicketPrice * seatNumbers.Count;
            totalPrice = ApplyDiscount(totalPrice, seatNumbers.Count);
            double pricewithgst = totalPrice * 0.18;
            Console.WriteLine($"Ticket Price : {totalPrice} + 18% GST");
            totalPrice += pricewithgst;
            Console.WriteLine($"Total Price: ₹{totalPrice}");
            Console.Write("\nDo you want to proceed with payment? (yes/no): ");
            string confirm = Console.ReadLine().Trim().ToLower();

            if (confirm != "yes" || confirm == "no" || confirm == "n")
            {
                Console.WriteLine("Booking cancelled.");
                return false;
            }

            return PaymentGateway(customer, show, seatNumbers, totalPrice); ;
        }

        public static bool CancelTicket(Customer customer)
        {
            if (customer.BookedTickets.Count == 0)
            {
                Console.WriteLine("No ticket booked to cancel.");
                return false;
            }
            Console.WriteLine("Your booked tickets:");
            for (int i = 0; i < customer.BookedTickets.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {customer.BookedTickets[i].MovieName} - {customer.BookedTickets[i].ShowTime}");
            }

            Console.Write("Enter ticket number to cancel:");
            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > customer.BookedTickets.Count)
            {
                Console.WriteLine("Invalid choice.");
                return false;
            }
            Console.Write("Please confirm to cancel the ticket (y/n):");
            char cf = Console.ReadLine()[0];
            if(cf == 'n')
            {
                Console.WriteLine("Ticket cancel aborted.");
                return false;
            }
            Ticket ticket = customer.BookedTickets[choice - 1];
            DateTime showdatetime = DateTime.Parse(ticket.ShowTime);
            DateTime currenttime = DateTime.Now;

            double refund = 0;
            if ((currenttime - showdatetime).TotalHours > 24)
            {
                refund = ticket.Price;
            }
            else if ((currenttime - showdatetime).TotalHours > 0)
            {
                refund = ticket.Price * 0.5;
            }
            if (refund > 0)
            {
                Console.WriteLine($"Ticket cancelled. Refund amount : {refund}");
            }
            else
            {
                Console.WriteLine("Show already started. No refund.");
            }
            customer.BookedTickets.Remove(ticket);
            return true;
        }
    }
}
