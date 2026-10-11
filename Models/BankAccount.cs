using System.ComponentModel.DataAnnotations;
namespace BankMvc.Models;
public class BankAccount
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string HolderName { get; set; } = "";
    [Range(typeof(decimal), "0", "999999999999")]
    public decimal Balance { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public List<BankTransaction> Transactions { get; set; } = new();
}