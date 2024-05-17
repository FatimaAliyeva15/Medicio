using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCProjects_Medico.ViewModel;
using MVCProjects_MedicoCore.Models;

namespace MVCProjects_Medico.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = new AppUser()
            {
                FullName = registerVM.Name,
                UserName = registerVM.UserName,
                Email = registerVM.Email

            };
            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return RedirectToAction("Login");

        }

        public async Task<IActionResult> CreateRole()
        {
            IdentityRole identityRole1 = new IdentityRole("Admin");
            IdentityRole identityRole2 = new IdentityRole("Member");

            await _roleManager.CreateAsync(identityRole1);
            await _roleManager.CreateAsync(identityRole2);

            return Ok("Rollar yarandi");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user;

            if (loginVM.UserNameOrEmail.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            }
            else
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "UserName or Password is not valid");
                return View();
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, true);

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Try again shortly");
                return View();
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }

            await _signInManager.SignInAsync(user, loginVM.IsPersistent);

            var role = await _userManager.GetRolesAsync(user);

            if (role.Contains("Admin"))
            {
                return RedirectToAction("Index", "Dashboard", new {area = "Admin"});
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
    }
}
