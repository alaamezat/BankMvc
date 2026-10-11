using BankMvc.Models;
using Microsoft.EntityFrameworkCore;
namespace BankMvc.Data;
public class BankDbContext(DbContextOptions<BankDbContext> options) : DbContext(options)
{
    public DbSet<BankAccount> Accounts => Set<BankAccount>();
    public DbSet<BankTransaction> Transactions => Set<BankTransaction>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>().Property(x => x.Balance).HasPrecision(18, 2);
        modelBuilder.Entity<BankTransaction>().Property(x => x.Amount).HasPrecision(18, 2);
        modelBuilder.Entity<BankTransaction>()
            .HasOne(t => t.BankAccount).WithMany(a => a.Transactions)
            .HasForeignKey(t => t.BankAccountId).OnDelete(DeleteBehavior.Cascade);
    }
}