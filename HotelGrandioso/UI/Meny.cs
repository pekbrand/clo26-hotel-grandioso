using HotelGrandioso.Services;
using HotelGrandioso.Models;

namespace HotelGrandioso.UI;

public class Meny
{
    private readonly HotelBookingService bookingService;

    private readonly RoomView roomView = new RoomView();

    public Meny(HotelBookingService service)
    {
        bookingService = service;
    }

    (DateOnly CheckIn, DateOnly CheckOut) getBookingDates()
    {
        while (true)
        {
            DateOnly checkIn = getValidDate("Ankomst");
            DateOnly checkOut = getValidDate("Avresa");

            if (checkOut > checkIn)
            {
                return (checkIn, checkOut);
            }

            Console.WriteLine(
                "Avresa måste vara efter ankomst. Ange båda datumen igen.");
        }
    }

    DateOnly getValidDate(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt} (ÅÅÅÅ-MM-DD): ");
            string? input = Console.ReadLine();

            if (DateOnly.TryParseExact(
                input,
                "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out DateOnly date))
            {
                return date;
            }

            Console.WriteLine("Ogiltigt datum. Försök igen.");
        }
    }
    
    int getValidNumber(int min, int max)
    {
        while (true)
        {
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Du skrev inget. Försök igen.");
                continue;
            }

            if (int.TryParse(input, out int number))
            {
                if (number >= min && number <= max)
                {
                    return number;
                }
            }

            Console.WriteLine($"Ange ett heltal mellan {min} och {max}.");
        }
    }

    int getAvailableRoom(List<RoomOverview> rooms)
    {
        while (true)
        {
            Console.Write("Vilket rum vill du boka? ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int roomNumber))
            {
                if (rooms.Any(room => room.RoomNumber == roomNumber))
                {
                    return roomNumber;
                }
            }

            Console.WriteLine("Ange rumsnumret för ett av de lediga rummen.");
        }
    }

    public void choices()
    {
        bool running = true;

        while (running)
        {
            Console.Clear();
            Console.WriteLine("=== Hotel Grandioso ===");
            Console.WriteLine("\n1. Boka rum");
            Console.WriteLine("2. Avboka");
            Console.WriteLine("3. Visa bokningar");
            Console.WriteLine("4. Visa lediga rum");
            Console.WriteLine("5. Boka spa");
            Console.WriteLine("6. Nyttpris");
            Console.WriteLine("7. Avsluta");

            int option = getValidNumber(1,7);

            switch (option)
            {
                case 1:
                    {
                    Console.Clear();
                    Console.WriteLine("1. Boka rum:.");
                    var dates = getBookingDates();
                    var rooms = bookingService.GetRooms(dates.CheckIn, dates.CheckOut);
                    roomView.ShowRooms(rooms);
                    int selectedRoom = getAvailableRoom(rooms);
                    Console.Clear();
                    Console.Write("Gästens namn: ");
                    string guestName = Console.ReadLine() ?? "";
                    bool booked = bookingService.BookRoom(
                    selectedRoom,
                    dates.CheckIn,
                    dates.CheckOut,
                    guestName);
                    if (booked)
                    {
                        Console.WriteLine(
                            $"Tack {guestName}, ditt rum är {selectedRoom} " +
                            $"från {dates.CheckIn} till {dates.CheckOut}.");
                    }
                    else
                    {
                        Console.WriteLine("Din bokning misslyckades, vänligen försök igen!");
                    }
                    Console.WriteLine("Tryck Enter för att fortsätta.");
                    Console.ReadLine();
                    break;
                    }

                case 2:
                    Console.Clear();
                    Console.WriteLine("2. Avboka:");
                    break;
                case 3:
                    Console.Clear();
                    Console.WriteLine("3. Visa bokningar:");
                    break;
                    
                case 4:
                {
                    Console.Clear();
                    Console.WriteLine("4. Visa lediga rum:");
                    var dates = getBookingDates();
                    var rooms = bookingService.GetRooms(dates.CheckIn, dates.CheckOut);
                    roomView.ShowRooms(rooms);
                    Console.WriteLine("Tryck på Enter för att fortsätta.");
                    Console.ReadLine();
                    break;
                }
                case 5:
                    Console.Clear();
                    Console.WriteLine("5. Boka spa:");
                    break;
                case 6:
                    Console.Clear();
                    Console.WriteLine("6. Byta Pris:");

                    break;
                case 7:
                    Console.WriteLine("Tack för idag!");
                    running = false;
                    break;
                    
            }
        }
    }

}