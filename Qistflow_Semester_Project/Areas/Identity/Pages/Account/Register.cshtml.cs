using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qistflow_Semester_Project.Data;

namespace QistFlow.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public RegisterModel(UserManager<ApplicationUser> userManager,
                         SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required, DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = "";
        
        [Required, DataType(DataType.Text)]
        public string firstName { get; set; } = "";
        
        [Required, DataType(DataType.Text)]
        public string lastName { get; set; } = "";

        [Required, DataType(DataType.PhoneNumber)]
        public string phoneNumber { get; set; } = "";

        [Required, DataType(DataType.Text)]
        public string cnic { get; set; } = "";

        [Required, DataType(DataType.MultilineText)]
        public string Address { get; set; } = "";

        [Required, DataType(DataType.Date)]
        public string dateOfBirth { get; set; } = "";
    }

    public IActionResult OnGet()
    {
        if (User.IsInRole("Admin"))
            return RedirectToAction("Index", "Admin", new { area = "" });
        if (User.IsInRole("Customer"))
            return RedirectToAction("Index", "Home", new { area = "" });
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = new ApplicationUser
        {
            UserName = Input.firstName.Trim(),
            //PasswordHash = Input.Password.Trim(),
            Email = Input.Email.Trim(),
            FirstName = Input.firstName.Trim(),
            LastName = Input.lastName.Trim(),
            Cnic = Input.cnic.Trim(),
            DateOfBirth = DateTime.Parse(Input.dateOfBirth),
            PhoneNumber = Input.phoneNumber.Trim()
        };

        var result = await _userManager.CreateAsync(user, Input.Password);
        if (result.Succeeded)
        {
            // Public forms may never pick or provide their own role.
            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                foreach (var error in roleResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return Page();
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        return Page();
    }
}
