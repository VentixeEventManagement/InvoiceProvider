using Presentation.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Documentation;

public class InvoiceRequestExample : IExamplesProvider<InvoiceRequest>
{
    public InvoiceRequest GetExamples() => new()
    {
        BookingId = "Abc123",
        UserId = "User123",
        EventId = "Event123",
        TotalAmount = 7889
    };

}
