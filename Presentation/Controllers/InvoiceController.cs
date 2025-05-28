using Microsoft.AspNetCore.Mvc;
using Presentation.Documentation;
using Presentation.Models;
using Presentation.Services;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController(IInvoiceService invoiceService) : ControllerBase
{
    private readonly IInvoiceService _invoiceService = invoiceService;
    private readonly InvoiceCreatedSenderService _invoiceCreatedSenderService;

    [HttpPost("create")]
    [SwaggerOperation(Summary = "Creates an invoice")]
    [SwaggerResponse(400, "Invoice Request contained invalid or missing data")]
    [SwaggerRequestExample(typeof(InvoiceRequest), typeof(InvoiceRequestExample))] // Exempel på hur request ser ut

    public async Task<IActionResult> CreateInvoice([FromBody] InvoiceRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(request.BookingId))
        {
            return BadRequest(ModelState);
        }

        //Skapar en Invoice från request från BookingProvider
        //Matchar den här med Fabrice modell
        var invoice = new Invoice
        {
            InvoiceId = Guid.NewGuid().ToString(), // Genererar här ett ID för fakturan
            BookingId = request.BookingId,
            UserId = request.UserId,
            EventId = request.EventId,
            IssueDate = DateTime.UtcNow, //Sätter dagens datum
            DueDate = DateTime.UtcNow.AddDays(30), //Sätter duedate till 30 dagar framåt
            TotalAmount = request.TotalAmount,
            Status = "Unpaid"
        };

        var result = await _invoiceService.CreateAsync(invoice);

        //Anropar att skicka till serviceBus
        await _invoiceCreatedSenderService.SendMessageAsync(invoice.InvoiceId);

        return Ok(result);

    }

    [HttpGet("getAllWithToken")]
    [SwaggerOperation(Summary = "Returns a list of invoices")]

    public async Task<IActionResult> GetAllInvoicesWithToken()
    {
        var authorization = Request.Headers.Authorization[0];
        var token = authorization!.Split(" ")[1];

        using var http = new HttpClient();
        var response = await http.PostAsJsonAsync($"https://tokenservice.jfventixeeventmanagement.azurewebsites/api/validatetoken", new { token });

        if (!response.IsSuccessStatusCode)
        {
            return Unauthorized();
        }


        var invoices = await _invoiceService.GetAllAsync();
        foreach (var invoice in invoices)
        {
            if (invoice.Status == "Unpaid" && invoice.DueDate < DateTime.UtcNow)
            {
                invoice.Status = "Overdue";
            }

        }
        return Ok(invoices);
    }


    [HttpGet("getAll")]
    [SwaggerOperation(Summary = "Returns a list of invoices")]
    public async Task<IActionResult> GetAllInvoices()
    {
        var invoices = await _invoiceService.GetAllAsync();

        foreach (var invoice in invoices)
        {
            if (invoice.Status == "Unpaid" && invoice.DueDate < DateTime.UtcNow)
            {
                invoice.Status = "Overdue";
            }
            
        }
        return Ok(invoices);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoiceById(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("Id is empty");
        }
        var invoice = await _invoiceService.GetByIdAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }
        return Ok(invoice);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInvoice(string id, Invoice invoice)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _invoiceService.UpdateAsync(id, invoice);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    [HttpDelete("{id}")]

    public async Task<IActionResult> DeleteInvoice(string id)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var result = await _invoiceService.DeleteAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok(result);
    }






}
