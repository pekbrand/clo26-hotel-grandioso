namespace HotelGrandioso.Services;

using HotelGrandioso.Data;
using HotelGrandioso.Models;

public class HotelBookingService
{
    private readonly HotelContext db;

    public HotelBookingService(HotelContext context)
    {
        db = context;
    }

    public List<RoomOverview> GetRooms(DateOnly checkIn, DateOnly checkOut)
    {
        var result = 
        (
            from Room in db.Rooms
            join RoomPrice in db.RoomPrice
                on Room.RoomNumber equals RoomPrice.RoomNumber
            where !db.HotelBooking.Any(booking =>
            booking.RoomNumber == Room.RoomNumber &&
            booking.CheckIn < checkOut &&
            booking.CheckOut > checkIn)
            select new RoomOverview
            {
                RoomNumber = Room.RoomNumber,
                RoomFloor = Room.Floor,
                SizeM2 = Room.SizeM2,
                RoomTypeName = Room.RoomType.Name,
                PricePerNight = RoomPrice.PricePerNight
            }
        ).ToList();
        return result;
    }

    public bool BookRoom(
        int roomNumber,
        DateOnly checkIn,
        DateOnly checkOut,
        string guestName)
    {
        if (checkOut <= checkIn)
        {
            return false;
        }
        bool isBooked = db.HotelBooking.Any(booking =>
            booking.RoomNumber == roomNumber &&
            booking.CheckIn < checkOut &&
            booking.CheckOut > checkIn);

        if (isBooked)
        {
            return false;
        }
        else
        {
            var guest = new Guest
            {
                Name = guestName
            };

            var booking = new HotelBooking
            {
                Guest = guest,
                RoomNumber = roomNumber,
                CheckIn = checkIn,
                CheckOut = checkOut
                
            };
            db.HotelBooking.Add(booking);
            db.SaveChanges();
            return true;
        }
    }
}