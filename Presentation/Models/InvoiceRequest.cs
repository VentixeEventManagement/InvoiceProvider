namespace Presentation.Models;

public class InvoiceRequest
{
    //Ta emot data från Booking - när en bokning gjorts. Detta ska föras över från Fabrices modell.

    public string BookingId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string EventId { get; set; } = null!;

    public decimal TotalAmount { get; set; }

}
