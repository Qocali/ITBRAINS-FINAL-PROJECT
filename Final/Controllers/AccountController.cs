using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  Final.Extentions;
using static Final.Extentions.Helper;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
namespace Final.Controllers
{
    public class AccountController : Controller
    {
        #region DbContext
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<AppUser> userManager,
                                 RoleManager<IdentityRole> roleManager,
                                 SignInManager<AppUser> signInManager,
                                 ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region ForgotPassword
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetlink = Url.Action("ResetPassword", "Account", new { email = forgotPasswordVM.Email, token = token }, Request.Scheme);
                    //await _userManager.SendEmailAsync(token, "Reset Password", "Please reset your password by click <a href=\"" + passwordResetlink+"\">here</a>");
                    MimeMessage message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Devloper", "babayevqocheli@gmail.com"));

                    message.To.Add(MailboxAddress.Parse(user.Email));
                    message.Body = new TextPart("plain")
                    {
                        Text = passwordResetlink
                    };
                    message.Subject = "Reset your password please";
                    string email = "babayevqocheli@gmail.com";
                    string password = "Psg+1970";
                    SmtpClient client = new SmtpClient();
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate(email, password);
                    client.Send(message);
                    return View("ForgotPasswordConfirmation");

                }
            }
            return View(forgotPasswordVM);
        }
#endregion
        #region ResetPassword
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            if (token == null && email == null)
            {
                ModelState.AddModelError("", "Etibarsiz cehd");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
                if (user != null)
                {
                    var result= await _userManager.ResetPasswordAsync(user,resetPasswordVM.Token,resetPasswordVM.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(resetPasswordVM);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(resetPasswordVM);
        }
        #endregion
        #region Login
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View();
            AppUser appUser;
            appUser = await _userManager.FindByNameAsync(loginVM.UserName);
            if (appUser == null)
            {
                appUser = await _userManager.FindByEmailAsync(loginVM.UserName);
                if (appUser == null)
                {
                    ModelState.AddModelError("", "Istifadeci adi,Email ve ya parol sehvdi");
                    return View();
                }

            }
            if (appUser.IsDeactive)
            {
                ModelState.AddModelError("", "senin hesabin aktiv deyil");
                return View();
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginVM.Password, true, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Senin hesabin bloklanib,zehmet olmasa 10deq gozleyin ");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Istifadeci adi,Email ve ya parol sehvdi");
                return View();
            }
            //if (User.IsInRole("Admin"))
            //{

            //    return RedirectToAction("Index", "DashBoard","admin", new { area = "" });
            //}

            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser newUser = new AppUser
            {
                Name = registerVM.Name,
                Surname = registerVM.SurName,
                UserName = registerVM.UserName,
                Email = registerVM.Email


            };

            IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
                return View();
            }
           
                 await _userManager.AddToRoleAsync(newUser, Role.Mederator.ToString());
            await _signInManager.SignInAsync(newUser, true);
            return RedirectToAction("Index", "Home");

        }
        #endregion
        #region CreateRole
        //public async Task CreateRole()
        //{
        //    if (!(await _roleManager.RoleExistsAsync(Role.Admin.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Role.Admin.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Role.Member.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Role.Member.ToString() });
        //    }
        //    if (!(await _roleManager.RoleExistsAsync(Role.Mederator.ToString())))
        //    {
        //        await _roleManager.CreateAsync(new IdentityRole { Name = Role.Mederator.ToString() });
        //    }

        //}
        #endregion

    }
}
