# InvoiceProvider

Detta är ett back end delsystem i en mikroservicestruktur för hantering av fakturor (invoices). Projektet är ett ASP.NET Core Web API och använder .NET 9 och Entity Framework Core samt Swagger för API-dokumentation.
Användargränsnittet finns i React applicationen VentixeEventManegemanetFrontEnd i tillhörande organisation "VentixeEventManagement".

## Kom igång

1. **Kloning av repo**


2. **Konfigurera databasanslutning**
   Lägg till din anslutningssträng i `appsettings.json` under `ConnectionStrings`.

3. **Kör applikationen**
   
   
## Swagger & API-dokumentation

https://js-invoiceservice-afccd2cuffeuawe5.swedencentral-01.azurewebsites.net/index.html

Swagger är aktiverat för att ge en interaktiv dokumentation av API:et. 


### Endpoints

- `POST /api/invoice/create`  
  Skapa en ny faktura.  
  **Body:**  
  
- `GET /api/invoice/getAll`  
  Hämta alla fakturor.

- `GET /api/invoice/{id}`  
  Hämta en specifik faktura.

- `PUT /api/invoice/{id}`  
  Uppdatera en faktura.

- `DELETE /api/invoice/{id}`  
  Ta bort en faktura.

Förberett för Token:
- `GET /api/invoice/getAllWithToken`  
  Hämta alla fakturor (kräver token).

## Teknologier

- .NET 9
- Entity Framework Core
- SQL Server & Databas via Azure
- Swagger (Swashbuckle)

## Utveckling & Dokumentation

- Koden är dokumenterad med [Swagger Annotations](https://github.com/domaindrivendev/Swashbuckle.AspNetCore).
- Exempel på request/response finns i Swagger UI.
- För att lägga till eller ändra endpoints, se `Presentation/Controllers/InvoiceController.cs`.

## Kontakt
Johanna Falkenmark johannafalkenmark@gmail.com
