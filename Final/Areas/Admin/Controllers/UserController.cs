using Final.Extentions;
using Final.Models;
using Final.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> Users = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in Users)
            {
                UserVM userVM = new UserVM
                {
                    Id = user.Id,
                    Name = user.Name,
                    Surname = user.Surname,
                    Email = user.Email,
                    IsDeactive = user.IsDeactive,
                    Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault()
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }
        public async Task<IActionResult> ChangeRole(string id)
        {
            if (id == null)
                return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                BadRequest();
            List<string> roles = new List<string>();
            roles.Add(Helper.Role.Admin.ToString());
            roles.Add(Helper.Role.Member.ToString());
            roles.Add(Helper.Role.Mederator.ToString());
            ChangeRoleVM changeRoleVM = new ChangeRoleVM
            {
                UserName = user.UserName,
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault(),
                Roles = roles
            };
            return View(changeRoleVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id,string role)
        {
            if (id == null)
                return NotFound();
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                BadRequest();
            List<string> roles = new List<string>();
            roles.Add(Helper.Role.Admin.ToString());
            roles.Add(Helper.Role.Member.ToString());
            roles.Add(Helper.Role.Mederator.ToString());
            string oldrole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            ChangeRoleVM changeRoleVM = new ChangeRoleVM
            {
                UserName = user.UserName,
                Role= oldrole,
                Roles = roles
            };
            IdentityResult addIdentityResult = await _userManager.AddToRoleAsync(user, role);
            if (!addIdentityResult.Succeeded)
            {
                ModelState.AddModelError("", "Elave ede bilmersiniz cunki her hansi problem var!");
                return View(changeRoleVM);
            }
            IdentityResult RemoveIdentityResult = await _userManager.RemoveFromRoleAsync(user, oldrole);
            if (!RemoveIdentityResult.Succeeded)
            {
                ModelState.AddModelError("", "Sile bilmersiniz cunki her hansi problem var");
                return View(changeRoleVM);
            }
            return RedirectToAction("Index");
        }
    }
}
