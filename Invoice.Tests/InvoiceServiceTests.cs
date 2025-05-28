using Data.Contexts;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Presentation.Services;
using Presentation.Models;
namespace Invoice.Tests;


//Tester framtagna med hjälp av chat gpt 4.1
public class InvoiceServiceTests
{
    private InvoiceService GetServiceWithInMemoryDb(string dbName)
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        var context = new DataContext(options);
        return new InvoiceService(context);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddInvoice()
    {
        // Arrange
        var service = GetServiceWithInMemoryDb(nameof(CreateAsync_ShouldAddInvoice));

        var invoice = new Presentation.Models.Invoice
        {
            InvoiceId = "inv-1",
            BookingId = "book-1",
            UserId = "user-1",
            EventId = "event-1",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(10),
            TotalAmount = 100,
            Status = "Unpaid"
        };

        // Act
        var result = await service.CreateAsync(invoice);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("inv-1", result.InvoiceId);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllInvoices()
    {
        // Arrange
        var service = GetServiceWithInMemoryDb(nameof(GetAllAsync_ShouldReturnAllInvoices));
        await service.CreateAsync(new Presentation.Models.Invoice
        {
            InvoiceId = "inv-2",
            BookingId = "book-2",
            UserId = "user-2",
            EventId = "event-2",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(5),
            TotalAmount = 200,
            Status = "Unpaid"
        });

        // Act
        var invoices = await service.GetAllAsync();

        // Assert
        Assert.NotNull(invoices);
        Assert.Single(invoices);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnInvoice_WhenExists()
    {
        // Arrange
        var service = GetServiceWithInMemoryDb(nameof(GetByIdAsync_ShouldReturnInvoice_WhenExists));
        await service.CreateAsync(new Presentation.Models.Invoice
        {
            InvoiceId = "inv-3",
            BookingId = "book-3",
            UserId = "user-3",
            EventId = "event-3",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(7),
            TotalAmount = 300,
            Status = "Unpaid"
        });

        // Act
        var invoice = await service.GetByIdAsync("inv-3");

        // Assert
        Assert.NotNull(invoice);
        Assert.Equal("inv-3", invoice.InvoiceId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateInvoice()
    {
        // Arrange
        var service = GetServiceWithInMemoryDb(nameof(UpdateAsync_ShouldUpdateInvoice));
        await service.CreateAsync(new Presentation.Models.Invoice
        {
            InvoiceId = "inv-4",
            BookingId = "book-4",
            UserId = "user-4",
            EventId = "event-4",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(8),
            TotalAmount = 400,
            Status = "Unpaid"
        });

        var updatedInvoice = new Presentation.Models.Invoice
        {
            InvoiceId = "inv-4",
            BookingId = "book-4",
            UserId = "user-4",
            EventId = "event-4",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(8),
            TotalAmount = 999,
            Status = "Paid"
        };

        // Act
        var result = await service.UpdateAsync("inv-4", updatedInvoice);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(999, result.TotalAmount);
        Assert.Equal("Paid", result.Status);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveInvoice()
    {
        // Arrange
        var service = GetServiceWithInMemoryDb(nameof(DeleteAsync_ShouldRemoveInvoice));
        await service.CreateAsync(new Presentation.Models.Invoice
        {
            InvoiceId = "inv-5",
            BookingId = "book-5",
            UserId = "user-5",
            EventId = "event-5",
            IssueDate = DateTime.UtcNow,
            DueDate = DateTime.UtcNow.AddDays(9),
            TotalAmount = 500,
            Status = "Unpaid"
        });

        // Act
        var result = await service.DeleteAsync("inv-5");
        var invoice = await service.GetByIdAsync("inv-5");

        // Assert
        Assert.True(result);
        Assert.Null(invoice);
    }
}