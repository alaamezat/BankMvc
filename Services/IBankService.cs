using BankMvc.Models;
namespace BankMvc.Services;
public interface IBankService
{
    Task<List<BankAccount>> GetAllAsync();
    Task<BankAccount?> GetAccountAsync(int id);
    Task<BankAccount> CreateAsync(string name, decimal initialBalance);
    Task<(bool Success, string Message)> DepositAsync(int accountId, decimal amount);
    Task<(bool Success, string Message)> WithdrawAsync(int accountId, decimal amount);
}