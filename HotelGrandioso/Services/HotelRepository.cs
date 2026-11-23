namespace HotelGrandioso.Services;

using HotelGrandioso.Data;
using HotelGrandioso.Models;

public class HotelRepository
{
    private readonly HotelContext db;

    public HotelRepository(HotelContext context)
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

    public List<Treatment> GetTreatments()
    {
        return db.Treatment.ToList();
    }

    public bool BookTreatment(
    int treatmentId,
    DateOnly date,
    string guestName)
    {
        // Skapa gästen.
        var guest = new Guest
            {
                Name = guestName
            };
        var booking = new SpaBooking
                {
                    Guest = guest,
                    TreatmentId = treatmentId,
                    Date = date
                };
        db.SpaBooking.Add(booking);
        db.SaveChanges();
        return true;
    }

    public RoomPrice? CurrentPrice(int roomNumber,DateOnly today)
    {   
        var currentPrice = db.RoomPrice.FirstOrDefault(price =>
            price.RoomNumber == roomNumber &&
            price.FromDate <= today &&
            (price.ToDate == null || today < price.ToDate));
        return currentPrice;
    }

    public void UpdatePrice(RoomPrice price)
    {
        db.RoomPrice.Add(price);
        db.SaveChanges();
    }

    public bool RoomIsAvalible(int roomNumber,DateOnly checkIn,DateOnly checkOut)
    {   bool isBooked = db.HotelBooking.Any(booking =>
            booking.RoomNumber == roomNumber &&
            booking.CheckIn < checkOut &&
            booking.CheckOut > checkIn);
        return isBooked;
    }

    public bool Savebooking(HotelBooking booking)
    {
        db.HotelBooking.Add(booking);
        db.SaveChanges();
        return true;
    }
}