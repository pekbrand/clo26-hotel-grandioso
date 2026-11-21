using Microsoft.Extensions.Configuration;
using HotelGrandioso.Models;
using HotelGrandioso.Data;
using HotelGrandioso.UI;
using HotelGrandioso.Services;

var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = config.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string saknas.");

using var db = new HotelContext(connectionString);
var service = new HotelBookingService(db);
var program = new Meny(service);

program.choices();