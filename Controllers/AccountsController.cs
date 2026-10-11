using BankMvc.Services;
using BankMvc.ViewModels;
using Microsoft.AspNetCore.Mvc;
namespace BankMvc.Controllers;
public class AccountsController(IBankService bank) : Controller
{
    public async Task<IActionResult> Index() => View(await bank.GetAllAsync());

    public IActionResult Create() => View(new CreateAccountViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateAccountViewModel model)
    {
        if (!ModelState.IsValid) return View(model);
        var account = await bank.CreateAsync(model.HolderName, model.InitialBalance);
        TempData["Success"] = "Account created successfully.";
        return RedirectToAction(nameof(Details), new { id = account.Id });
    }

    public async Task<IActionResult> Details(int id)
    {
        var account = await bank.GetAccountAsync(id);
        return account is null ? NotFound() : View(account);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Deposit(MoneyOperationViewModel model)
    {
        if (!ModelState.IsValid)
            TempData["Error"] = "Enter a valid amount.";
        else
        {
            var result = await bank.DepositAsync(model.AccountId, model.Amount);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
        }
        return RedirectToAction(nameof(Details), new { id = model.AccountId });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Withdraw(MoneyOperationViewModel model)
    {
        if (!ModelState.IsValid)
            TempData["Error"] = "Enter a valid amount.";
        else
        {
            var result = await bank.WithdrawAsync(model.AccountId, model.Amount);
            TempData[result.Success ? "Success" : "Error"] = result.Message;
        }
        return RedirectToAction(nameof(Details), new { id = model.AccountId });
    }

    public IActionResult Error() => View();
}