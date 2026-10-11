using System.ComponentModel.DataAnnotations;
namespace BankMvc.ViewModels;
public class CreateAccountViewModel
{
    [Required, StringLength(100, MinimumLength = 2)]
    [Display(Name = "Account holder")]
    public string HolderName { get; set; } = "";
    [Range(typeof(decimal), "0", "999999999999")]
    [Display(Name = "Initial balance")]
    public decimal InitialBalance { get; set; }
}