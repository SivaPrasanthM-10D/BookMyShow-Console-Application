using BookMyShow.Custom_Exceptions;
using BookMyShow.Models;

namespace BookMyShow.Implementations
{
    public static class BookingSystem
    {
        private static void WriteCentered(string text)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = text.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }

        private static string ReadCentered(string prompt)
        {
            int windowWidth = Console.WindowWidth;
            int textLength = prompt.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.Write(new string(' ', spaces) + prompt);
            return Console.ReadLine();
        }

        public static void DisplaySeats(Show show)
        {
            WriteCentered("\n        Seat Layout:");
            WriteCentered("        =====================================");
            WriteCentered("\n        _________________________________");
            WriteCentered("       /                                 \\");
            WriteCentered("");

            int totalseats = show.AvailableSeats + show.BookSeats.Count;
            int seatsPerRow = 10;
            int seatWidth = 5; // Width of each seat representation, e.g., "[001]"
            int rowWidth = seatsPerRow * seatWidth;
            int windowWidth = Console.WindowWidth;
            int spaces = (windowWidth - rowWidth) / 2;

            for (int seat = 1; seat <= totalseats; seat++)
            {
                if (seat % seatsPerRow == 1)
                {
                    Console.Write(new string(' ', spaces));
                }

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
                if (seat % seatsPerRow == 0) Console.WriteLine();
            }

            WriteCentered("        =====================================\n");
        }

        public static double ApplyDiscount(double totalprice, int seatcount)
        {
            string inp = ReadCentered("Do you have coupon code? {y/n}:").ToUpper();

            double discount = 0;
            if (seatcount >= 3)
            {
                discount += 0.10;
                WriteCentered("Bulk discount applied (10% off)!");
            }

            if (inp == "Y")
            {
                Dictionary<string, double> Coupons = AdminOperations.GetCoupons();
                string coupon = ReadCentered("Enter the coupon code:").ToUpper();

                if (coupon == "MOVIE10")
                {
                    discount += 0.10;
                    WriteCentered("Promo code MOVIE10 applied! Extra 10% Off.");
                }
                else if (Coupons.ContainsKey(coupon))
                {
                    discount += Coupons[coupon] / 100;
                    WriteCentered($"Coupon code {coupon} applied! Extra {Coupons[coupon]}% Off.");
                }
                else
                {
                    WriteCentered("Invalid coupon code.");
                }
            }

            double finprice = totalprice - discount;
            return finprice;
        }

        public static void PaymentGateway(Customer customer, Show show, List<int> seatNumbers, double totalprice)
        {
            try
            {
                WriteCentered("\nRedirecting to payment gateway..............");
                WriteCentered("\nPAYMENT GATEWAY");
                WriteCentered("Choose Payment method:");
                WriteCentered("1. Google Pay");
                WriteCentered("2. PayTm");
                WriteCentered("3. PhonePe");
                string? choice = ReadCentered("Enter your choice:");
                string? paymentmethod = choice switch
                {
                    "1" => "Google Pay",
                    "2" => "PayTm",
                    "3" => "PhonePe",
                    _ => null
                };

                if (paymentmethod == null)
                {
                    throw new InvalidPaymentMethodException();
                }

                WriteCentered("\n\nPayment Details:");
                WriteCentered($"Payment Method : {paymentmethod}");
                WriteCentered($"Amount to be paid : {totalprice}");
                WriteCentered($"Paying to : {show.Theatre.Name}, {show.Theatre.Street}");
                string paycfm = ReadCentered("Confirm Payment (pay/cancel)").Trim().ToLower();
                if (paycfm != "pay")
                {
                    WriteCentered("\n\nPayment Cancelled.");
                    return;
                }
                int retry = 3;
                while (retry > 0)
                {
                    string upipinInput = ReadCentered("Enter your 6-digit UPI Pin:");
                    if (!int.TryParse(upipinInput, out int upipin) || upipin < 100000 || upipin > 999999)
                    {
                        retry--;
                        if (retry == 0)
                            throw new PaymentFailedException("\n\nEntered wrong pin too many times. Payment failed.");
                        else
                            throw new InvalidUpiException("\n\nInvalid UPI Pin format. Enter correct pin.");
                    }
                    else if (upipin == customer.UpiPin)
                    {
                        show.AvailableSeats -= seatNumbers.Count;
                        show.BookSeats.AddRange(seatNumbers);
                        var ticket = new Ticket(show.Movie.Title, show.ShowTime, seatNumbers, show.Theatre.Name, totalprice);
                        customer.BookedTickets.Add(ticket);
                        WriteCentered("\n\nPayment Successful! Your ticket(s) have been booked.");
                        ticket.DisplayTicket();
                        return;
                    }
                    else
                    {
                        retry--;
                        if (retry == 0)
                            throw new PaymentFailedException("\n\nEntered wrong pin too many times. Payment failed.");
                        else
                            throw new InvalidUpiException("\n\nInvalid UPI Pin format. Enter correct pin.");
                    }
                }
                return;
            }
            catch (InvalidPaymentMethodException e)
            {
                WriteCentered(e.Message);
                return;
            }
            catch (InvalidUpiException e)
            {
                WriteCentered(e.Message);
            }
            catch (PaymentFailedException e)
            {
                WriteCentered(e.Message);
                return;
            }

        }
        public static void BookTicket(Customer customer, Show show, List<int> seatNumbers)
        {
            foreach (var seat in seatNumbers)
            {
                if (show.BookSeats.Contains(seat))
                {
                    WriteCentered($"Seat {seat} already booked. Please choose another seat.");
                    return;
                }
            }

            if (seatNumbers.Count == 0)
            {
                WriteCentered("No seats selected. Please select at least one seat.");
                return;
            }

            double totalPrice = show.TicketPrice * seatNumbers.Count;
            totalPrice = ApplyDiscount(totalPrice, seatNumbers.Count);
            double pricewithgst = totalPrice * 0.18;
            WriteCentered($"Ticket Price : {totalPrice} + 18% GST");
            totalPrice += pricewithgst;
            WriteCentered($"Total Price: ₹{totalPrice}");
            string confirm = ReadCentered("\nDo you want to proceed with payment? (yes/no):").Trim().ToLower();

            if (confirm != "yes" && confirm != "y")
            {
                WriteCentered("Booking cancelled.");
                return;
            }

            PaymentGateway(customer, show, seatNumbers, totalPrice);
        }

        public static bool CancelTicket(Customer customer)
        {
            if (customer.BookedTickets.Count == 0)
            {
                WriteCentered("No ticket booked to cancel.");
                return false;
            }
            WriteCentered("Your booked tickets:");
            for (int i = 0; i < customer.BookedTickets.Count; i++)
            {
                WriteCentered($"{i + 1}. {customer.BookedTickets[i].MovieName} - {customer.BookedTickets[i].ShowTime}");
            }

            string choiceInput = ReadCentered("Enter ticket number to cancel:");
            if (!int.TryParse(choiceInput, out int choice) || choice < 1 || choice > customer.BookedTickets.Count)
            {
                WriteCentered("Invalid choice.");
                return false;
            }
            char cf = ReadCentered("Please confirm to cancel the ticket (y/n):")[0];
            if (cf == 'n')
            {
                WriteCentered("Ticket cancel aborted.");
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
                WriteCentered($"Ticket cancelled. Refund amount : {refund}");
            }
            else
            {
                WriteCentered("Show already started. No refund.");
            }
            customer.BookedTickets.Remove(ticket);
            return true;
        }
    }
}
