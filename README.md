# Hotel Grandioso

Ett konsolbaserat bokningssystem byggt med C#, EF Core och SQLite.
appsettings.json ligger git eftersom det inte ligger någon hemlig-nyckel eller liknande i de
utan bara är en sträng som pekar mot databasen i projekt mappen! 

- [ ] Visa alla rum (med typ och pris) 
Tolkade detta som att jag skulle visa alla lediga rum, med tanke på menyn som var med i exemplet
på uppgiften.

## Funktioner

- Boka och avboka hotellrum.
- Visa bokningar för ett datumintervall.
- Visa tillgängliga rum med rumstyp, storlek och nattpris.
- Beräkna totalpris utifrån antal nätter och nattpris.
- Boka spabehandlingar.
- Ändra rumspriser med giltighetsperioder.

## Köra projektet

### Förutsättningar

- .NET 10 SDK
- EF Core CLI (`dotnet-ef`) version 10

Om EF-verktyget saknas:

```bash
dotnet tool install --global dotnet-ef --version "10.*"
```

Öppna terminalen i projektmappen `HotelGrandioso`, där
`HotelGrandioso.csproj` finns.

Kör:

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Projektet använder följande anslutningssträng i `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=hotel.db"
  }
}
```

Konfigurationsfilen kopieras till programmets körningsmapp vid bygge.
SQLite-filen `hotel.db` använder en relativ sökväg och hamnar i
programmets aktuella arbetsmapp.

### Startdata

Jag har lagt till information manuellt, men databasen ligger i projektet så
skall inte vara några problem att komma igång med appen.

## Kodstruktur och ansvar

### UI

`Meny` hanterar användarens menyval och inmatning.
Återanvändbara metoder används för att läsa tal, datum och datumintervall.

`RoomView` presenterar rum, bokningar och behandlingar i konsolen.
Rummen grupperas efter våning.

### Services

`HotelBookingService` hanterar rumsbokningar, avbokningar,
tillgänglighetskontroll och prisändringar.

`SpaBookingService` hämtar behandlingar och sparar spabokningar.

Databasanropen ligger i serviceklasserna och använder LINQ och
EF Core. Menyn behöver därför inte känna till hur data hämtas
eller sparas. I detta projekt har jag valt att inte lägga till
ett separat repositorylager.

### Data

`HotelContext` ärver från EF Cores `DbContext`. Klassen ger tillgång
till tabellerna genom `DbSet`, konfigurerar SQLite och beskriver
entiteternas nycklar och relationer.

`HotelContextFactory` används av EF-verktygen när exempelvis
migrationer skapas.

### Models

Modellerna beskriver rum, gäster, bokningar, rumstyper, priser och
spabehandlingar.

Valde att försöka bara ha två .cs hade kunnat göra en för varje klass men eftersom dem är väldigt små tyckte jag det var okej att 
lägga in i samma cs.

`RoomOverview` och `BookingsOverview` är resultatklasser för
information som ska visas i UI. De är inte egna databastabeller.

## Datum och tillgänglighet

En vistelse omfattar ankomstdagen fram till avresedagen.
En ny gäst kan därför checka in samma dag som en annan checkar ut.

En befintlig bokning överlappar önskade datum om:

```text
Befintlig incheckning < önskad utcheckning
OCH
Befintlig utcheckning > önskad incheckning
```

Tillgänglighetsfrågan använder `Any` för att kontrollera om det
finns en överlappande bokning för rummet. Med `!Any` väljs endast
rum utan en sådan bokning.

Service kontrollerar tillgängligheten igen innan bokningen sparas.
Det skyddar även om någon anropar bokningsmetoden utan att först
hämta listan med lediga rum.

## Prissättning

Priser lagras i `RoomPrice` för att kunna ha olika giltighetsperioder.

- `FromDate` är inkluderande.
- `ToDate` är exkluderande.
- Ett tomt (null) `ToDate` betyder att priset gäller tills vidare.

Prisändringar gäller från dagens datum. Om prisraden redan börjar
idag uppdateras beloppet. Annars avslutas den gamla perioden och
en ny prisrad skapas.

Rumslistan visar dagens giltiga pris. Totalpriset beräknas som:

```text
Antal nätter × nattpris
```

Antalet nätter beräknas som skillnaden mellan utcheckningsdatumets
och incheckningsdatumets `DayNumber`.

I nuvarande version används dagens nattpris för hela vistelsen.
Beräkningen delar inte upp vistelsen mellan olika prisperioder.
dvs att det är samma pris per natt hela bokningen även om
man uppdaterar priset undervistelsen.

## UML-klassdiagram

```mermaid
classDiagram
    direction TB

    class Room {
        int RoomNumber
        int Floor
        int SizeM2
        int RoomTypeId
        RoomType RoomType
    }

    class RoomType {
        int RoomTypeId
        string Name
        string Description
    }

    class RoomPrice {
        int Id
        int RoomNumber
        decimal PricePerNight
        DateOnly FromDate
        DateOnly? ToDate
    }

    class Guest {
        int Id
        string Name
    }

    class HotelBooking {
        int Id
        int GuestId
        Guest Guest
        DateOnly CheckIn
        DateOnly CheckOut
        int RoomNumber
        Room Room
    }

    class SpaBooking {
        int Id
        int TreatmentId
        Treatment Treatment
        int GuestId
        Guest Guest
        DateOnly Date
    }

    class Treatment {
        int TreatmentId
        string Name
        int DurationMinutes
        decimal Price
    }

    RoomType "1" --> "0*" Room : klassificerar
    Room "1" --> "0*" RoomPrice : har prisperioder
    Room "1" --> "0*" HotelBooking : bokas
    Guest "1" --> "0*" HotelBooking : har
    Guest "1" --> "0*" SpaBooking : har
    Treatment "1" --> "0..*" SpaBooking : bokas
```

## Entitetsrelationer

- En rumstyp kan användas av flera rum.
- Ett rum kan ha flera prisperioder och flera bokningar.
- Varje hotellbokning tillhör en gäst och ett rum.
- En gäst kan ha flera hotellbokningar och spabokningar.
- Varje spabokning tillhör en gäst och en behandling.
- En behandling kan förekomma i flera spabokningar.

Vid avbokning tas hotellbokningen bort. Gästen finns kvar.

## GDPR

Eftersom vi bara skall spara information som är nödvändig för syftet har jag bara valt namn.
Hade detta projektet varit verkligt hade jag även sparat e-mailadress och även telefon nummer.
för att kunna skicka ut en bekräftelse anting på telefon eller mail. samt att det kan vara nödvändigt att komma 
ikontakt med kunden både innan och efter besöket.

## Begränsningar

- Spabokningar har datum men inget klockslag eller kapacitetsskydd.
- En ny gästpost skapas vid varje bokning; befintliga gäster återanvänds inte.
    Här hade man kunnat göra en storförbättring speciellt om bokningen användes i typ en webbläsare,
    då hade man kunnat uppdatera listan för varje bokstav och klicka i en existerande kunde, men visste inte hur jag skulle göra detta
    i en consoleapp.
- Bokningens pris sparas inte som ett separat historiskt bokningsbelopp.
    akutellt pris och historiken ligger i samma tabel, men eftersom jag kikar på att priset gäller just nu borde det inte vara några större problem
    dock kan detta bli lite värre om man uppdaterar samma pris samma dag, men eftersom det nyaste priset skall ha null i sig så borde det ändå lösa sig.
- Tillgänglighetskontrollen behöver förstärkas för samtidiga bokningsförsök.

## Designmönster

Jag följde den lagerindelning vi fick i uppgiften. Tanken var att göra koden lätt att läsa och att ge varje del ett tydligt ansvar.

- **Data:** I `Dbhandeler.cs` finns `HotelContext`, som sköter kopplingen till SQLite och beskriver relationerna mellan entiteterna för EF Core.
- **Models:** Här finns entiteterna, till exempel rum, gäster och bokningar, samt modellerna som används för att visa listor i konsolen.
- **Services:** Här finns logiken för bland annat bokning och avbokning. Tjänsterna använder `HotelContext` för att läsa och spara data. Jag valde att samla databasanropen i tjänsterna eftersom projektet är litet. Det gör att menyn kan använda tjänsterna utan att behöva veta hur databasen fungerar.
- **UI:** `Meny.cs` hanterar menyval och inmatning. Jag flyttade ut flera konsolutskrifter till `RoomView` i `ConsoleLines.cs` för att göra menyn lättare att läsa och kunna återanvända utskrifterna.

I efterhand hade jag delat upp `Meny.cs` mer. Den blev några hundra rader, vilket var mer än jag tänkt mig. Med en egen metod för varje menyval hade huvudmenyn blivit kortare och det hade varit lättare att följa till exempel bokning och avbokning var för sig.

## Tekniker

- C# och .NET 10 för konsolprogrammet
- Entity Framework Core Code First med migrationer för databasens struktur
- SQLite för lagring
- LINQ för databasfrågor, till exempel för att hitta lediga rum