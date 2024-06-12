using Humanizer;
using DocumentApprovalSystem.Data;
using DocumentApprovalSystem.Web.Pages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DocumentApprovalSystem.Web.Areas.Register.Pages;

[AllowAnonymous]
public class IndexModel : SysPageModel
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty]
    public Employee Employee { get; set; }
    public SelectList Titles { get; private set; }
    public SelectList Departments { get; private set; }

    [BindProperty]
    public string Mobile { get; set; }
    [BindProperty]
    public string Password { get; set; }
    [BindProperty]
    public string Email { get; set; }

    public async Task OnGet()
    {
        Title = PageTitle = "Create a new account";
        Titles = new SelectList(await Db.Titles.OrderBy(c => c.Name).ToListAsync(), nameof(Data.Title.Id), nameof(Data.Title.Name));
        Departments = new SelectList(await Db.Departments.OrderBy(c => c.Name).ToListAsync(), nameof(Data.Department.Id), nameof(Data.Department.Name));

    }

    public async Task<IActionResult> OnPostAsync()
    {
        var duplicateUser = await Db.Users.FirstOrDefaultAsync(c => c.LoginId == Email);
        if (duplicateUser != null)
        {
            Title = PageTitle = "Create a new account";
            Titles = new SelectList(await Db.Titles.OrderBy(c => c.Name).ToListAsync(), nameof(Data.Title.Id), nameof(Data.Title.Name));
            ModelState.AddModelError($"{nameof(Email)}", "A user with the same email already exists. Try a different email.");
            return Page();
        }



        Employee.CreationDate = DateTime.Now;
        Employee.Surname = Employee.Surname.Trim().Humanize(LetterCasing.Title);
        Employee.FirstName = Employee.FirstName.Trim().Humanize(LetterCasing.Title);
        Employee.Position = Employee.Position.Trim().Humanize(LetterCasing.Title);
        Employee.Address = Employee.Address.Trim();
        Employee.IdNumber = Employee.IdNumber.Replace(" ", "").Replace("-", "").ToUpper();

        Employee.IdNavigation = new User
        {
            Id = Employee.Id,
            Email = Email.ToLower(),
            IsActive = true,
            IsMobileConfirmed = true,
            LoginId = Email.ToLower(),
            Mobile = Mobile,
            CreationDate = DateTime.Now,
            Name = $"{Employee.FirstName} {Employee.Surname}",
            Role = Lib.UserRole.USER,
            ActivationDate = DateTime.Now,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        var count = (await Db.Employees.CountAsync(c => c.CreationDate > startDate) + 1) % 10000;
        var year = (DateTime.Now.Year - 2020) % 100;
        Employee.AccountId = $"{Employee.IdNavigation.Initials}{year:00}{startDate.Month:X}{count:000}".ToUpper();
        Employee.IdNavigation.PasswordHash = _userManager.PasswordHasher.HashPassword(Employee.IdNavigation, Password);
        Db.Employees.Add(Employee);
        await Db.SaveChangesAsync();
        await _signInManager.SignInAsync(Employee.IdNavigation, false);






        return RedirectToPage("/Index", new { area = "Account" });
    }
}
