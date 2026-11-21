namespace HotelGrandioso.Models;

public class SpaBooking    
{
    public int Id { get; set; }
    public int TreatmentId{get;set;}
    public Treatment Treatment {get;set;} = null!;
    public int GuestId { get; set; }
    public Guest Guest { get; set; } = null!;
    public DateOnly Date{get;set;}
}


public class Guest
{
    public int Id{get;set;}
    public string Name{get;set;}
    
}  


public class Treatment
{
    public int TreatmentId {get;set;}
    public string Name{get;set;}
    public int DurationMinutes{get;set;}
    public decimal Price{get;set;}
}