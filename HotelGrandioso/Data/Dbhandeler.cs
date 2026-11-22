using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using HotelGrandioso.Models;

namespace HotelGrandioso.Data;

public class HotelContextFactory
    : IDesignTimeDbContextFactory<HotelContext>
{
    public HotelContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Hittar ingen connection string 'Default' i appsettings.json.");

        return new HotelContext(connectionString);
    }
}


public class HotelContext : DbContext
{
    public string connectionString { get; private set; }
    
    

    public HotelContext(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<RoomType> RoomTypes => Set<RoomType>();
    public DbSet<RoomPrice> RoomPrice => Set<RoomPrice>();
    public DbSet<HotelBooking> HotelBooking => Set<HotelBooking>();
    public DbSet<SpaBooking> SpaBooking => Set<SpaBooking>();
    public DbSet<Treatment> Treatment => Set<Treatment>();


    




    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
            .HasKey(room => room.RoomNumber);
        
        modelBuilder.Entity<HotelBooking>()
            .HasOne(booking => booking.Room)
            .WithMany()
            .HasForeignKey(booking => booking.RoomNumber);
        
        modelBuilder.Entity<HotelBooking>()
            .HasOne(booking => booking.Guest)
            .WithMany()
            .HasForeignKey(booking => booking.GuestId);

        modelBuilder.Entity<RoomType>()
            .HasKey(RoomType => RoomType.RoomTypeId);

        modelBuilder.Entity<Guest>()
            .HasKey(Guest => Guest.Id);

        modelBuilder.Entity<Treatment>()
            .HasKey(Treatment => Treatment.TreatmentId);

        modelBuilder.Entity<SpaBooking>()
            .HasOne(SpaBooking => SpaBooking.Guest)
            .WithMany()
            .HasForeignKey(SpaBooking => SpaBooking.GuestId);

        modelBuilder.Entity<SpaBooking>()
            .HasOne(spaBooking => spaBooking.Treatment)
            .WithMany()
            .HasForeignKey(spaBooking => spaBooking.TreatmentId);

        modelBuilder.Entity<RoomPrice>()
            .HasOne<Room>()
            .WithMany()
            .HasForeignKey(RoomPrice => RoomPrice.RoomNumber);

        modelBuilder.Entity<Room>()
            .HasOne(room => room.RoomType)
            .WithMany()
            .HasForeignKey(room => room.RoomTypeId);

    }

}

