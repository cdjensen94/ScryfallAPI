using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ScryfallAPI.Utilities;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace ScryfallAPI.Pages
{
    public class LoginModel : PageModel
    {
		public ScryfallContext _context {  get; set; }

		public LoginModel(ScryfallContext context)
		{
			_context = context;
		}
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                string? emailFromInput = Request.Form["emailaddress"];
                AuthenticateUser authenticateUser = new(_context);
				string returnUrl = "https://localhost:7223/index";

                if (emailFromInput is not null)
                {
                   var exist = await authenticateUser.AuthenticateUsers(emailFromInput);

					var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, exist.Email),
					new Claim("Email", exist.Email),
					new Claim("UserId", exist.Id.ToString()),
					new Claim(ClaimTypes.Role, "Administrator"),
				};

					var claimsIdentity = new ClaimsIdentity(
						claims, CookieAuthenticationDefaults.AuthenticationScheme);

					var authProperties = new AuthenticationProperties
					{
						AllowRefresh = true,

						IsPersistent = true,

						RedirectUri = "https://localhost:7223/index"
					};

					await HttpContext.SignInAsync(
						CookieAuthenticationDefaults.AuthenticationScheme,
						new ClaimsPrincipal(claimsIdentity),
						authProperties);

					return Redirect(returnUrl);

				}
				else
                {
                    ModelState.AddModelError(string.Empty, "User not recognized");
                }

				
			}

			return Page();
        }
    }
}
