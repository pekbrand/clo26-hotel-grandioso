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
}