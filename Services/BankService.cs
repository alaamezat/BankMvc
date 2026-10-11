using System.Data;
using BankMvc.Data;
using BankMvc.Models;
using Microsoft.EntityFrameworkCore;
namespace BankMvc.Services;
public class BankService(BankDbContext db) : IBankService
{
    public Task<List<BankAccount>> GetAllAsync() =>
        db.Accounts.AsNoTracking().OrderBy(a => a.Id).ToListAsync();

    public Task<BankAccount?> GetAccountAsync(int id) =>
        db.Accounts.AsNoTracking().Include(a => a.Transactions)
          .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<BankAccount> CreateAsync(string name, decimal initialBalance)
    {
        if (string.IsNullOrWhiteSpace(name) || initialBalance < 0)
            throw new ArgumentException("Invalid account details");
        var account = new BankAccount { HolderName = name.Trim(), Balance = initialBalance };
        db.Accounts.Add(account);
        if (initialBalance > 0)
            account.Transactions.Add(new BankTransaction { Type = "Opening", Amount = initialBalance });
        await db.SaveChangesAsync();
        return account;
    }

    public Task<(bool Success, string Message)> DepositAsync(int accountId, decimal amount) =>
        ChangeBalanceAsync(accountId, amount, "Deposit");

    public Task<(bool Success, string Message)> WithdrawAsync(int accountId, decimal amount) =>
        ChangeBalanceAsync(accountId, amount, "Withdrawal");

    private async Task<(bool Success, string Message)> ChangeBalanceAsync(int id, decimal amount, string type)
    {
        if (amount <= 0 || decimal.Round(amount, 2) != amount)
            return (false, "Enter a positive amount with at most 2 decimal places.");

        // Serializable transaction protects the balance check against simultaneous withdrawals.
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var account = await db.Accounts.FirstOrDefaultAsync(a => a.Id == id);
        if (account is null) return (false, "Account not found.");
        if (type == "Withdrawal" && account.Balance < amount)
            return (false, "Insufficient balance.");
        try
        {
            account.Balance = checked(account.Balance + (type == "Deposit" ? amount : -amount));
            db.Transactions.Add(new BankTransaction
            {
                BankAccountId = id, Type = type, Amount = amount, CreatedAtUtc = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
            await tx.CommitAsync();
            return (true, type + " completed successfully.");
        }
        catch (OverflowException)
        {
            return (false, "Amount exceeds supported balance limit.");
        }
        catch (DbUpdateConcurrencyException)
        {
            return (false, "Account changed during operation. Please try again.");
        }
    }
}