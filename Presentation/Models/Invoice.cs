namespace Presentation.Models;

public class Invoice
{

    public string InvoiceId { get; set; } = null!;

    public string BookingId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string EventId { get; set; } = null!;
    public DateTime IssueDate { get; set; } 

    public DateTime DueDate { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

}
