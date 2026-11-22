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
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        var result = 
        (
            from Room in db.Rooms
            join RoomPrice in db.RoomPrice
                on Room.RoomNumber equals RoomPrice.RoomNumber
            where RoomPrice.FromDate <= today &&
            (RoomPrice.ToDate == null || today < RoomPrice.ToDate) &&
            !db.HotelBooking.Any(booking =>
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

    public List<BookingsOverview> GetBookings(
        DateOnly checkIn,
        DateOnly checkOut)
    {
        var result = (
            from booking in db.HotelBooking
            join guest in db.Guests
                on booking.GuestId equals guest.Id
            where booking.CheckIn < checkOut &&
                booking.CheckOut > checkIn
            select new BookingsOverview
            {
                BookingId = booking.Id,
                GuestName = guest.Name,
                RoomNumber = booking.RoomNumber,
                CheckIn = booking.CheckIn,
                CheckOut = booking.CheckOut
            }
        ).ToList();

        return result;
    }

    public bool ChangeRoomPrice(int roomNumber, decimal newPrice)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);

        var currentPrice = db.RoomPrice.FirstOrDefault(price =>
            price.RoomNumber == roomNumber &&
            price.FromDate <= today &&
            (price.ToDate == null || today < price.ToDate));

        if (newPrice <= 0)
        {
            return false;
        }
        if (currentPrice is null)
        {
            return false;
        }

        if (currentPrice.FromDate == today)
        {
            currentPrice.PricePerNight = newPrice;
        }
        else
        {
            currentPrice.ToDate = today;

            var price = new RoomPrice
            {
                RoomNumber = roomNumber,
                PricePerNight = newPrice,
                FromDate = today,
                ToDate = null
            };

            db.RoomPrice.Add(price);
        }
        db.SaveChanges();
        return true;
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

    public bool CancelBooking(int bookingId)
    {
        var booking = db.HotelBooking
            .FirstOrDefault(booking => booking.Id == bookingId);

        if (booking is null)
        {
            return false;
        }

        db.HotelBooking.Remove(booking);
        db.SaveChanges();

        return true;
    }


}