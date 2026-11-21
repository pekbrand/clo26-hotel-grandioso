using Microsoft.Extensions.Configuration;
using HotelGrandioso.Models;
using HotelGrandioso.Data;






Console.Clear();
int getValidNumber(int min, int max)
{
    while (true)
    {
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Du skrev inget. Försök igen.");
            continue;
        }

        if (int.TryParse(input, out int number))
        {
            if (number >= min && number <= max)
            {
                return number;
            }
        }

        Console.WriteLine($"Ange ett heltal mellan {min} och {max}.");
    }
}

bool running = true;

while (running)
{
    Console.Clear();
    Console.WriteLine("=== Hotel Grandioso ===");
    Console.WriteLine("\n1. Boka rum");
    Console.WriteLine("2. Avboka");
    Console.WriteLine("3. Visa bokningar");
    Console.WriteLine("4. Visa lediga rum");
    Console.WriteLine("5. Boka spa");
    Console.WriteLine("6. Nyttpris");
    Console.WriteLine("7. Avsluta");

    int option = getValidNumber(1,7);

    switch (option)
    {
        case 1:
            Console.Clear();
            Console.WriteLine("1. Boka rum:.");
            break;

        case 2:
            Console.Clear();
            Console.WriteLine("2. Avboka:");
            break;
        case 3:
            Console.Clear();
            Console.WriteLine("3. Visa bokningar:");
            break;
            
        case 4:
            Console.Clear();
            Console.WriteLine("4. Visa lediga rum:");
            break;
        case 5:
            Console.Clear();
            Console.WriteLine("5. Boka spa:");
            break;
        case 6:
            Console.Clear();
            Console.WriteLine("6. Byta Pris:");

            break;
        case 7:
            Console.WriteLine("Tack för idag!");
            running = false;
            break;
            
    }

}

