using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Qistflow_Semester_Project.Data;

namespace QistFlow.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public LoginModel(SignInManager<ApplicationUser> signInManager,
                      UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    // Same Identity page, two visual portal modes (no duplicate auth implementation).
    [BindProperty(SupportsGet = true)]
    public string Portal { get; set; } = "Customer";

    public class InputModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }

    public IActionResult OnGet()
    {
        Portal = string.Equals(Portal, "Admin", StringComparison.OrdinalIgnoreCase)
            ? "Admin" : "Customer";

        if (User.IsInRole("Admin"))
            return RedirectToAction("Index", "Admin", new { area = "" });
        if (User.IsInRole("Customer"))
            return RedirectToAction("Index", "Home", new { area = "" });
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Portal = string.Equals(Portal, "Admin", StringComparison.OrdinalIgnoreCase)
            ? "Admin" : "Customer";

        if (!ModelState.IsValid)
            return Page();

        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user is null || !await _userManager.IsInRoleAsync(user, Portal))
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password for this portal.");
            return Page();
        }

        // This issues the real encrypted Identity authentication cookie.
        var result = await _signInManager.PasswordSignInAsync(
            user, Input.Password, isPersistent: false, lockoutOnFailure: true);

        if (result.IsLockedOut)
            ModelState.AddModelError(string.Empty, "Account temporarily locked. Try again later.");
        else if (result.RequiresTwoFactor)
            ModelState.AddModelError(string.Empty, "Two-factor authentication is not enabled in this demo.");
        else if (!result.Succeeded)
            ModelState.AddModelError(string.Empty, "Invalid email or password for this portal.");
        else
        {
            // Ignore external/old ReturnUrl values: always redirect by verified role.
            return Portal == "Admin"
                ? RedirectToAction("Index", "Admin", new { area = "" })
                : RedirectToAction("Index", "Home", new { area = "" });
        }

        return Page();
    }
}
