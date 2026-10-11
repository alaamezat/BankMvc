namespace BankMvc.Models;
public class BankTransaction
{
    public int Id { get; set; }
    public int BankAccountId { get; set; }
    public BankAccount BankAccount { get; set; } = null!;
    public string Type { get; set; } = "";
    public decimal Amount { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}