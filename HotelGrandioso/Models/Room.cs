namespace HotelGrandioso.Models;
public class Room    
{
    public int RoomNumber{get;set;}
    public int Floor{get;set;}
    public int SizeM2{get;set;}
    public int RoomTypeId{get;set;}
    public RoomType RoomType { get; set; } = null!;
}


public class RoomType
{
    public int RoomTypeId{get;set;}
    public string Name{get;set;}
    public string Description{get;set;}
    // Name (Standard/Luxury/Bridal), Description
}     



public class RoomPrice
{
    public int Id{get;set;}
    public int RoomNumber{get;set;}
    public decimal PricePerNight{get;set;}
    public DateOnly FromDate{get;set;}
    public DateOnly ? ToDate{get;set;} 
}  

public class HotelBooking    
{
    public int Id{get;set;}
    public int GuestId { get; set; }
    public Guest Guest { get; set; } = null!;
    public DateOnly CheckIn{get;set;}
    public DateOnly CheckOut{get;set;}
    public int RoomNumber { get; set; }
    public Room Room { get; set; } = null!;
}

public class RoomOverview
{
    public int RoomFloor { get; set; }
    public int RoomNumber { get; set; }
    public int SizeM2 { get; set; }
    public string RoomTypeName { get; set; } = "";
    public decimal PricePerNight { get; set; }
}

public class BookingsOverview
{
public int BookingId { get; set; }
public string GuestName	{ get; set; }
public int RoomNumber { get; set; }
public DateOnly CheckIn	 { get; set; }
public DateOnly CheckOut { get; set; }
}