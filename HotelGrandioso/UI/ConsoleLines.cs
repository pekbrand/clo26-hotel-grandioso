using HotelGrandioso.Models;

namespace HotelGrandioso.UI;

public class RoomView
{
    public void ShowRooms(List<RoomOverview> rooms)
    {
    var floors = rooms
                        .GroupBy(room => room.RoomFloor)
                        .OrderBy(floor => floor.Key);

                    foreach (var floor in floors)
                    {
                        Console.WriteLine($"\n=== Våning {floor.Key} ===");

                        foreach (var room in floor.OrderBy(room => room.RoomNumber))
                        {
                            Console.WriteLine(
                                $"Våning: {room.RoomFloor} | " +
                                $"Rum: {room.RoomNumber} | " +
                                $"Storlek: {room.SizeM2} m² | " +
                                $"Typ: {room.RoomTypeName} | " +
                                $"Pris: {room.PricePerNight:0.##} kr/natt");
                        }
                    }

    }
    public void ShowBookings(List<BookingsOverview> bookings)
    {
        if (bookings.Count == 0)
        {
            Console.WriteLine("Inga bokningar hittades för perioden.");
        }
        else
        {
            foreach (var booking in bookings.OrderBy(b => b.CheckIn))
            {
                Console.WriteLine(
                    $"Bokning: {booking.BookingId} | " +
                    $"Gäst: {booking.GuestName} | " +
                    $"Rum: {booking.RoomNumber} | " +
                    $"Ankomst: {booking.CheckIn:yyyy-MM-dd} | " +
                    $"Avresa: {booking.CheckOut:yyyy-MM-dd}");
            }
        }

        Console.WriteLine("\nTryck Enter för att fortsätta.");
        Console.ReadLine();
    }

  
    public void ShowTreatments(List<Treatment> treatments)
    {
        Console.WriteLine($"Antal behandlingar: {treatments.Count}");
        foreach (var treatment in treatments)
        {
            Console.WriteLine(
            $"{treatment.TreatmentId}: {treatment.Name}-{treatment.Price}- {treatment.DurationMinutes}");
        }
    }
}