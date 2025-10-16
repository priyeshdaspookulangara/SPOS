using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Vendor> Vendors { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<ChartOfAccounts> ChartOfAccounts { get; set; }

    // WFM Entities
    public DbSet<Department> Departments { get; set; }
    public DbSet<PayGrade> PayGrades { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }

    // MM Entities
    public DbSet<PurchaseRequisition> PurchaseRequisitions { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<StockLedger> StockLedger { get; set; }
}