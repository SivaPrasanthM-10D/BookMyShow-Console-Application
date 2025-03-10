using BookMyShow.Custom_Exceptions;
using BookMyShow.Models;

namespace BookMyShow.Implementations
{
    public static class BookingSystem
    {
        private static void WriteCentered(string text)
        {
            int windowWidth = 168;
            int textLength = text.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }

        private static string ReadCentered(string prompt)
        {
            int windowWidth = 168;
            int textLength = prompt.Length;
            int spaces = (windowWidth - textLength) / 2;
            Console.Write(new string(' ', spaces) + prompt);
            return Console.ReadLine();
        }

        public static void DisplaySeats(Show show)
        {
            Console.WriteLine();
            WriteCentered("        Seat Layout:");
            WriteCentered("        =====================================\n");
            WriteCentered("     _________________________________");
            WriteCentered("    /                                 \\");
            WriteCentered("");

            int seatsPerRow = 10;
            int seatWidth = 5;
            int rowWidth = seatsPerRow * seatWidth;
            int windowWidth = Console.WindowWidth;
            int spaces = (windowWidth - rowWidth) / 2;

            for (int seat = 1; seat <= show.TotalSeats; seat++)
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
                else if (Coupons.TryGetValue(coupon, out double value))
                {
                    discount += value / 100;
                    WriteCentered($"Coupon code {coupon} applied! Extra {value}% Off.");
                }
                else
                {
                    WriteCentered("Invalid coupon code.");
                }
            }

            double finprice = totalprice - (discount * totalprice);
            return finprice;
        }

        public static bool PaymentGateway(Customer customer, Show show, int screenno, List<int> seatNumbers, double totalprice)
        {
            try
            {
                Console.WriteLine("");
                WriteCentered("Redirecting to payment gateway..............");
                Thread.Sleep(1000);
                WriteCentered("");
                WriteCentered("PAYMENT GATEWAY");
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

                Console.WriteLine("\n\n");
                WriteCentered("Payment Details:");
                WriteCentered($"Payment Method : {paymentmethod}");
                switch (paymentmethod)
                {
                    case "Google Pay":
                        WriteCentered($"UPI ID: {customer.Id}@oksbi");
                        break;
                    case "PayTm":
                        WriteCentered($"Mobile No: {customer.PhoneNo}@ptsbi");
                        break;
                    case "PhonePe":
                        WriteCentered($"UPI ID: {customer.Id}@ibl");
                        break;
                }
                WriteCentered($"Amount to be paid : Rs {totalprice}");
                WriteCentered($"Paying to : {show.Theatre.Name}, {show.Theatre.Street}");
                string paycfm = ReadCentered("Confirm Payment (pay/cancel)").Trim().ToLower();
                if (paycfm != "pay")
                {
                    WriteCentered("");
                    WriteCentered("");
                    WriteCentered("Payment Cancelled.");
                    return false;
                }
                int retry = 3;
                while (retry > 0)
                {
                    string upipinInput = ReadCentered("Enter your 6-digit UPI Pin:");
                    if (!int.TryParse(upipinInput, out int upipin) || upipinInput.Length != 6)
                    {
                        retry--;
                        if (retry == 0)
                            throw new PaymentFailedException("Entered wrong pin too many times. Payment failed.\nRedirecting to BookMyShow....");
                        else
                            throw new InvalidUpiException("Invalid UPI Pin format. Enter correct pin.");
                    }
                    else if (customer.upipins.Contains(upipinInput))
                    {
                        show.AvailableSeats.RemoveAll(seat => seatNumbers.Contains(seat));
                        show.BookSeats.AddRange(seatNumbers);

                        var ticket = new Ticket(show.Movie.Title, screenno, show.ShowTime, show.ShowDate, seatNumbers, show.Theatre.Name, totalprice);
                        customer.BookedTickets.Add(ticket);
                        Console.WriteLine("\n\n");
                        WriteCentered("Payment Successful! Your ticket(s) have been booked.\n");
                        WriteCentered("Redirecting to BookMyShow....\n");
                        WriteCentered("Tickets booked successfully!\n");
                        ticket.DisplayTicket();
                        ReadCentered("Press any key to exit:");
                        return true;
                    }
                    else
                    {
                        retry--;
                        if (retry == 0)
                            throw new PaymentFailedException("Entered wrong pin too many times. Payment failed.\nRedirecting to BookMyShow....");
                        else
                            throw new InvalidUpiException("Invalid UPI Pin format. Enter correct pin.");
                    }
                }
                return true;
            }
            catch (InvalidPaymentMethodException e)
            {
                Console.WriteLine("\n\n");
                WriteCentered(e.Message);
                return false;
            }
            catch (InvalidUpiException e)
            {
                Console.WriteLine("\n\n");
                WriteCentered(e.Message);
                return false;
            }
            catch (PaymentFailedException e)
            {
                Console.WriteLine("\n\n");
                WriteCentered(e.Message);
                return false;
            }
        }

        public static bool BookTicket(Customer customer, Show show, int screenno, List<int> seatNumbers)
        {
            try
            {
                foreach (var seat in seatNumbers)
                {
                    if (show.BookSeats.Contains(seat))
                    {
                        throw new InvalidSeatNoException($"Seat {seat} already booked. Please choose another seat.");
                    }
                }

                if (seatNumbers.Count == 0)
                {
                    throw new InvalidSeatNoException("No seats selected. Please select at least one seat.");
                }

                double totalPrice = show.TicketPrice * seatNumbers.Count;
                totalPrice = ApplyDiscount(totalPrice, seatNumbers.Count);
                double pricewithgst = totalPrice * 0.18;
                totalPrice += pricewithgst;

                string confirm = ReadCentered("Do you want to proceed with payment? (yes/no):").Trim().ToLower();

                if (confirm != "yes" && confirm != "y")
                {
                    return false;
                }

                if (!PaymentGateway(customer, show, screenno, seatNumbers, totalPrice))
                {

                }

                Theatre theatre = show.Theatre;
                theatre.Screens.ForEach(screen =>
                {
                    screen.Shows.ForEach(s =>
                    {
                        if (s.Movie.Title == show.Movie.Title && s.ShowTime == show.ShowTime && s.ShowDate == show.ShowDate && s.Theatre.Screens.Any(s => show.Theatre.Screens.Any(sc => sc.ScreenNumber == s.ScreenNumber)))
                        {
                            s.AvailableSeats = new List<int>(show.AvailableSeats);
                            s.BookSeats = new List<int>(show.BookSeats);
                        }
                    });
                });
                
                return true;
            }
            catch (InvalidSeatNoException e)
            {
                WriteCentered(e.Message);
                ReadCentered("Press any key to exit:");
                return false;
            }
        }

        public static void RemoveBookings(Show show)
        {
            foreach (var customer in UserManagement.GetCustomers())
                customer.BookedTickets.RemoveAll(b => b.MovieName == show.Movie.Title && b.ShowTime == show.ShowTime && b.ShowDate == show.ShowDate && b.TheatreName == show.Theatre.Name);
        }

        public static bool CancelTicket(Customer customer)
        {
            try
            {
                WriteCentered("**Enter (EXIT) to exit**\n");

                if (customer.BookedTickets.Count == 0)
                {
                    throw new TicketNotFoundException("No ticket booked to cancel.");
                }

                WriteCentered("Your booked tickets:");
                for (int i = 0; i < customer.BookedTickets.Count; i++)
                {
                    WriteCentered($"{i + 1}. {customer.BookedTickets[i].MovieName} - {customer.BookedTickets[i].ShowDate} - {customer.BookedTickets[i].ShowTime} - Booked Seats: {string.Join(",", customer.BookedTickets[i].SeatNo)}");
                }

                string choiceInput = ReadCentered("Enter ticket number to cancel:");
                if (choiceInput.ToUpper() == "EXIT") { return false; }
                if (!int.TryParse(choiceInput, out int choice) || choice < 1 || choice > customer.BookedTickets.Count)
                {
                    throw new InvalidChoiceException("Invalid choice. Please enter a valid ticket number.");
                }

                Ticket ticket = customer.BookedTickets[choice - 1];
                ticket.DisplayTicket();

                string cf = ReadCentered("Please confirm to cancel the ticket (y/n):");
                if (string.IsNullOrEmpty(cf) || cf[0] == 'n')
                {
                    ReadCentered("Press any key for customer menu:");
                    return false;
                }

                DateTime showdatetime = DateTime.Parse(ticket.ShowTime);
                DateTime currenttime = DateTime.Now;

                double refund = 0;
                if ((showdatetime - currenttime).TotalHours > 24)
                {
                    refund = ticket.Price;
                }
                else if ((showdatetime - currenttime).TotalHours > 0)
                {
                    refund = ticket.Price * 0.5;
                }

                customer.BookedTickets.Remove(ticket);

                //WriteCentered("Checking available shows:");
                //foreach (var s in AdminOperations.GetShows())
                //{
                //    WriteCentered($"Show: {s.Movie.Title}, Date: {s.ShowDate}, Time: {s.ShowTime}, Theatre: {s.Theatre.Name}, Seats Available: {s.AvailableSeats.Count}");
                //}

                Show? show = AdminOperations.GetShows().Find(s =>
                            DateTime.ParseExact(s.ShowDate, "dd/MM/yyyy", null) == DateTime.ParseExact(ticket.ShowDate, "dd/MM/yyyy", null) &&
                            s.ShowTime.Trim() == ticket.ShowTime.Trim() &&
                            s.Theatre.Name.Contains(ticket.TheatreName, StringComparison.OrdinalIgnoreCase) &&
                            s.Theatre.Screens.Any(sn => sn.ScreenNumber == ticket.ScreenNo));

                
                if (show == null)
                {
                    throw new ShowNotFoundException("Show not found");
                }

                //WriteCentered($"Show: {show.Movie.Title}, Date: {show.ShowDate}, Time: {show.ShowTime}, Theatre: {show.Theatre.Name}, Seats Available: {show.AvailableSeats.Count}");

                show.AvailableSeats.AddRange(ticket.SeatNo);
                show.BookSeats.RemoveAll(s => ticket.SeatNo.Contains(s));

                Theatre theatre = show.Theatre;
                foreach (var screen in theatre.Screens)
                {
                    Show? matchingShow = screen.Shows.FirstOrDefault(s =>
                        s.Movie.Title == show.Movie.Title &&
                        s.ShowTime == show.ShowTime &&
                        s.ShowDate == show.ShowDate);

                    if (matchingShow != null)
                    {
                        matchingShow.AvailableSeats = new List<int>(show.AvailableSeats);
                        matchingShow.BookSeats = new List<int>(show.BookSeats);
                    }
                }

                foreach (var owner in UserManagement.GetTheatreOwners())
                {
                    if (owner.OwnedTheatre != null && owner.OwnedTheatre.Name == theatre.Name &&
                        owner.OwnedTheatre.City == theatre.City && owner.OwnedTheatre.Street == theatre.Street)
                    {
                        foreach (var screen in owner.OwnedTheatre.Screens)
                        {
                            Show? matchingShow = screen.Shows.FirstOrDefault(s =>
                                s.Movie.Title == show.Movie.Title &&
                                s.ShowTime == show.ShowTime &&
                                s.ShowDate == show.ShowDate);

                            if (matchingShow != null)
                            {
                                matchingShow.AvailableSeats = new List<int>(show.AvailableSeats);
                                matchingShow.BookSeats = new List<int>(show.BookSeats);
                            }
                        }
                    }
                }

                WriteCentered($"Ticket canceled successfully. Refund: {refund:C}");
                return true;
            }
            catch(ShowNotFoundException e)
            {
                WriteCentered(e.Message);
                ReadCentered("Press any key for customer menu:");
                return false;
            }
            catch (TicketNotFoundException e)
            {
                WriteCentered(e.Message);
                ReadCentered("Press any key for customer menu:");
                return false;
            }
            catch (InvalidChoiceException e)
            {
                WriteCentered(e.Message);
                return false;
            }
            catch (Exception e)
            {
                WriteCentered($"An unexpected error occurred: {e.Message}");
                return false;
            }
        }
    }
}