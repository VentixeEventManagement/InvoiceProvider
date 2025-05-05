
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Data.Contexts;

public class DataContext : DbContext
{
   
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
    
    public virtual DbSet<InvoiceEntity> Invoices { get; set; }

}
