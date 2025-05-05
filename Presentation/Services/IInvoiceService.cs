using Data.Entities;
using Presentation.Models;

namespace Presentation.Services
{
    public interface IInvoiceService
    {
        Task<bool> AlreadyExistsAsync(string id);
        Task<InvoiceEntity> CreateAsync(Invoice invoice);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<InvoiceEntity>> GetAllAsync();
        Task<InvoiceEntity?> GetByIdAsync(string id);
        Task<InvoiceEntity> UpdateAsync(string id, Invoice invoice);
    }
}