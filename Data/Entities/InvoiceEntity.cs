
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class InvoiceEntity
{

    [Key]
    public string InvoiceId { get; set; } = Guid.NewGuid().ToString();

    public string BookingId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string EventId { get; set; } = null!;
    public DateTime IssueDate { get; set; } = DateTime.Now;

    public DateTime DueDate { get; set; } = DateTime.Now.AddDays(30);

    public int TotalAmount { get; set; }

    public string Status { get; set; } = null!;
    


   

}
