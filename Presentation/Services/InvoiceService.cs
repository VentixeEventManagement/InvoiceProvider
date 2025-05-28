using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Presentation.Models;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Presentation.Services;

public class InvoiceService(DataContext context) : IInvoiceService
{
    protected readonly DataContext _context = context;

    protected readonly DbSet<InvoiceEntity> _dbSet = context.Set<InvoiceEntity>();

    public virtual async Task<bool> AlreadyExistsAsync(string id)
    {
        return await _dbSet.AnyAsync(e => e.InvoiceId == id);
    }


    //CREATE
    public virtual async Task<InvoiceEntity> CreateAsync(Invoice invoice)
    {

        try
        {

            if (invoice == null)
                return null;

            if (await AlreadyExistsAsync(invoice.InvoiceId))
            {
                Debug.WriteLine("Invoice already exists");
                return null;
            }

            var entity = new InvoiceEntity
            {
                InvoiceId = invoice.InvoiceId,
                BookingId = invoice.BookingId,
                UserId = invoice.UserId,
                EventId = invoice.EventId,
                IssueDate = invoice.IssueDate,
                DueDate = invoice.DueDate,
                TotalAmount = invoice.TotalAmount,
                Status = invoice.Status
            };
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating {nameof(Invoice)} :: {ex.Message}");
            return null;
        }
    }
    public async Task<EmailRequest> GenerateInvoiceEmailAsync(string invoiceId, string email)
    {
        var invoice = await GetByIdAsync(invoiceId);
        if (invoice == null)
            return null;

        var emailRequest = new EmailRequest
        {
            //Hämta in email 
            Recipients = [email],
            Subject = $"Invoice {invoice.InvoiceId}",
            PlainText = $"Thank you for your purchase, here is your invoice:  INV{invoice.InvoiceId}",
            Html = $"<html><body><h1>INV{invoice.InvoiceId}</h1><p> Here is your invoice </p><p>Total Amount: {invoice.TotalAmount}</p><p>Due Date: {invoice.DueDate}</p></body></html>"

        };
        return emailRequest;
    }

    //READ

    public virtual async Task<InvoiceEntity?> GetByIdAsync(string id)
    {
        if (string.IsNullOrEmpty(id))
            return null;

        var entity = await _dbSet.FirstOrDefaultAsync(e => e.InvoiceId == id);
        return entity;
    }

    public virtual async Task<IEnumerable<InvoiceEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _dbSet.ToListAsync();

            return entities;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error reading {nameof(Invoice)} :: {ex.Message}");
            return null;
        }

    }

    //UPDATE
    public virtual async Task<InvoiceEntity> UpdateAsync(string id, Invoice invoice)
    {
        try
        {
            if (string.IsNullOrEmpty(id) || invoice == null)
                return null;
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return null;
            entity.BookingId = invoice.BookingId;
            entity.UserId = invoice.UserId;
            entity.EventId = invoice.EventId;
            entity.IssueDate = invoice.IssueDate;
            entity.DueDate = invoice.DueDate;
            entity.TotalAmount = invoice.TotalAmount;
            entity.Status = invoice.Status;
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(Invoice)} :: {ex.Message}");
            return null;
        }
    }

    //DELETE

    public virtual async Task<bool> DeleteAsync(string id)
    {
        try
        {


            var entity = await GetByIdAsync(id);
            if (entity == null) return false;
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting {nameof(Invoice)} :: {ex.Message}");
            return false;
        }
    }

}
