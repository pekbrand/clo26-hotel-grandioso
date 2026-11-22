using HotelGrandioso.Services;
using HotelGrandioso.Models;

namespace HotelGrandioso.UI;

public class Meny
{
    private readonly HotelBookingService bookingService;

    private readonly RoomView roomView = new RoomView();

    private readonly SpaBookingService spaService;

    public Meny(
        HotelBookingService service,
        SpaBookingService spaService)
    {
        bookingService = service;
        this.spaService = spaService;
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

    int getValidTreatment(List<Treatment> treatments)
    {
        while (true)
        {
            Console.Write("Vilken behandling vill du boka? Ange ID: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int treatmentNumber))
            {
               if (treatments.Any(treatment => treatment.TreatmentId == treatmentNumber))
                {
                    return treatmentNumber;
                }
            }

            Console.WriteLine("Ange ett ID som existerar i listan.");
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
                   var selectedRoomInfo = rooms.First(room =>
                        room.RoomNumber == selectedRoom);

                    int nights = dates.CheckOut.DayNumber - dates.CheckIn.DayNumber;
                    decimal totalPrice = nights * selectedRoomInfo.PricePerNight;
                    if (booked)
                    {
                        Console.WriteLine(
                            $"Tack {guestName}, ditt rum är {selectedRoom} " +
                            $"från {dates.CheckIn} till {dates.CheckOut}."+
                            $"Bokade nätter {nights}, Totalt pris: {totalPrice}");
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
                {
                    Console.Clear();
                    Console.WriteLine("2. Avboka:");
                    var dates = getBookingDates();
                    var bookings = bookingService.GetBookings(dates.CheckIn, dates.CheckOut);
                    roomView.ShowBookings(bookings);
                    Console.Write("Ange boknings-ID att avboka: ");
                    int bookingId = getValidNumber(1,99999);
                    bool cancelled = bookingService.CancelBooking(bookingId);
                    break;
                }
                case 3:
                {
                    Console.Clear();
                    Console.WriteLine("3. Visa bokningar:");
                    var dates = getBookingDates();
                    var bookings = bookingService.GetBookings(dates.CheckIn, dates.CheckOut);
                    roomView.ShowBookings(bookings);
                    break;
                }
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
                {
                    Console.Clear();
                    Console.WriteLine("5. Boka spa:");
                    var treatments = spaService.GetTreatments();
                    roomView.ShowTreatments(treatments);
                    int selectedTreatment = getValidTreatment(treatments);
                    DateOnly spaDate = getValidDate("Ange Datum:");
                    Console.WriteLine("Ange Namn:");
                    string guestName = Console.ReadLine();
                    bool booked = spaService.BookTreatment(
                        selectedTreatment,
                        spaDate,
                        guestName);

                    if (booked)
                    {
                        Console.WriteLine($"Tack {guestName}! Din spabehandling är bokad {spaDate:yyyy-MM-dd}.");
                    }
                    else
                    {
                        Console.WriteLine("Bokningen misslyckades.");
}
                    Console.WriteLine("Tryck på Enter för att fortsätta.");
                    Console.ReadLine();
                    break;
                }
                case 6:
                {
                    Console.Clear();
                    Console.WriteLine("6. Byta Pris:");
                    Console.Write("Rumsnummer: ");
                    int roomNumber = getValidNumber(1, 99999);

                    decimal newPrice;

                    while (true)
                    {
                        Console.Write("Nytt pris per natt: ");

                        if (decimal.TryParse(Console.ReadLine(), out newPrice)
                            && newPrice > 0)
                        {
                            break;
                        }

                        Console.WriteLine("Ange ett giltigt pris större än 0.");
                    }

                    bool changed = bookingService.ChangeRoomPrice(
                        roomNumber, newPrice);

                    if (changed)
                    {
                        Console.WriteLine("Priset är uppdaterat och gäller från idag.");
                    }
                    else
                    {
                        Console.WriteLine(
                            "Priset kunde inte ändras. Rummet saknar ett aktuellt pris eller finns inte.");
                    }

                    Console.WriteLine("Tryck Enter för att fortsätta.");
                    Console.ReadLine();
                    break;
                }
                case 7:
                    Console.WriteLine("Tack för idag!");
                    running = false;
                    break;
                    
            }
        }
    }

}