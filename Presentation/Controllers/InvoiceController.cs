using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController(IInvoiceService invoiceService) : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IInvoiceService _invoiceService = invoiceService;

    //Eventuellt behövs ej CREATE - skapa faktura ska endast göras via ESB/i Backdend. 
    [HttpPost("create")]
    public async Task<IActionResult> CreateInvoice(Invoice invoice)
    {
        if (!ModelState.IsValid || invoice.InvoiceId == null)
        {
            return BadRequest(ModelState);
        }

        var result = await _invoiceService.CreateAsync(invoice);
        return Ok(result);

    }

    [HttpGet("getAll")]

    public async Task<IActionResult> GetAllInvoices()
    {
        var invoices = await _invoiceService.GetAllAsync();
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
