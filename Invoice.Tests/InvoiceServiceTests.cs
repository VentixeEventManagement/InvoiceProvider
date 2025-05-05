

using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Presentation.Services;

namespace Invoice.Tests;

//public class InvoiceServiceTests(DataContext context, InvoiceService invoiceService) 
//    protected readonly DataContext _context = context;
//    protected readonly DbSet<InvoiceEntity> _dbSet = context.Set<InvoiceEntity>();

//    private readonly IInvoiceService _invoiceService;


//    [Fact]
//    public async Task CreateAsync_ShouldReturnCreatedInvoice()
//    {
//    // Arrange
//    var invoiceService = new Mock<IInvoiceService>();
//        var invoice = new Invoice
//        {
//            InvoiceId = "INV123",
//            BookingId = "BOOK123",
//            UserId = "USER123",
//            EventId = "EVENT123",
//            IssueDate = DateTime.UtcNow,
//            DueDate = DateTime.UtcNow.AddDays(30),
//            TotalAmount = 100,
//            Status = "Pending"
//        };
//        invoiceService.Setup(x => x.CreateAsync(It.IsAny<Invoice>())).ReturnsAsync(new InvoiceEntity
//        {
//            InvoiceId = invoice.InvoiceId,
//            BookingId = invoice.BookingId,
//            UserId = invoice.UserId,
//            EventId = invoice.EventId,
//            IssueDate = invoice.IssueDate,
//            DueDate = invoice.DueDate,
//            TotalAmount = invoice.TotalAmount,
//            Status = invoice.Status
//        });
//        // Act
//        var result = await invoiceService.Object.CreateAsync(invoice);
//        // Assert
//        Assert.NotNull(result);
//        Assert.Equal(invoice.InvoiceId, result.InvoiceId);
//    }
