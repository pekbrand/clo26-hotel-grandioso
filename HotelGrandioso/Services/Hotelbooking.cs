namespace HotelGrandioso.Services;

using HotelGrandioso.Data;
using HotelGrandioso.Models;
using HotelGrandioso.Services;

public class HotelBookingService
{
    private readonly HotelRepository HotelRepository;

    public HotelBookingService(HotelRepository hotelRepository)
    {
        HotelRepository = hotelRepository;
    }
    public bool ChangeRoomPrice(int roomNumber, decimal newPrice)
    {
        DateOnly today = DateOnly.FromDateTime(DateTime.Today);
        var currentPrice = HotelRepository.CurrentPrice(roomNumber,today);

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

            HotelRepository.UpdatePrice(price);
        }
        
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
        bool isBooked = HotelRepository.RoomIsAvalible(roomNumber,checkIn,checkOut);

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
            HotelRepository.Savebooking(booking);
            return true;
        }
    }

    


}