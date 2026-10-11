using System.ComponentModel.DataAnnotations;
namespace BankMvc.ViewModels;
public class MoneyOperationViewModel
{
    public int AccountId { get; set; }
    [Range(typeof(decimal), "0.01", "999999999999", ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }
}