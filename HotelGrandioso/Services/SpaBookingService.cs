using HotelGrandioso.Data;
using HotelGrandioso.Models;

namespace HotelGrandioso.Services;

public class SpaBookingService
{
    private readonly HotelContext db;

    public SpaBookingService(HotelContext context)
    {
        db = context;
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
}