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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Relationship>()
            .HasOne(r => r.ParentCustomer)
            .WithMany(c => c.ChildRelationships)
            .HasForeignKey(r => r.ParentCustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Relationship>()
            .HasOne(r => r.ChildCustomer)
            .WithMany(c => c.ParentRelationships)
            .HasForeignKey(r => r.ChildCustomerId)
            .OnDelete(DeleteBehavior.Restrict);
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

    // Fina & SD Entities
    public DbSet<GlEntry> GlEntries { get; set; }
    public DbSet<SalesOrder> SalesOrders { get; set; }
    public DbSet<SalesOrderItem> SalesOrderItems { get; set; }
    public DbSet<Delivery> Deliveries { get; set; }
    public DbSet<BillingDocument> BillingDocuments { get; set; }

    // POS Entities
    public DbSet<PosTransaction> PosTransactions { get; set; }
    public DbSet<PosTransactionItem> PosTransactionItems { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    // CRM Entities
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<InteractionHistory> InteractionHistories { get; set; }
    public DbSet<Lead> Leads { get; set; }
    public DbSet<Opportunity> Opportunities { get; set; }
    public DbSet<Relationship> Relationships { get; set; }
}