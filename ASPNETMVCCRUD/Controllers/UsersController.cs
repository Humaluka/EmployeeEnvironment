using HelpComing.Data;
using HelpComing.Models.Domain;
using HelpComing.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class UsersController : Controller
{
    #region Constructors and Consts
    private readonly HelpComingDbContext helpComingDbContext;

    public UsersController(HelpComingDbContext helpComingDbContext)
    {
        this.helpComingDbContext = helpComingDbContext;
    }
    #endregion

    #region Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        List<User> users = await helpComingDbContext.Users.ToListAsync();
        return View(users);
    }
    #endregion

    #region Create
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Profile");
        }

        AddUserViewModel model = new AddUserViewModel
        {
            Roles = await helpComingDbContext.Roles.Select(r => new SelectListItem
            {
                Value = r.RoleID.ToString(),
                Text = r.RoleName
            }).ToListAsync(),

            Countries = await helpComingDbContext.Countries.Select(c => new SelectListItem
            {
                Value = c.CountryID.ToString(),
                Text = c.CountryName
            }).ToListAsync()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddUserViewModel addUserViewModel)
    {
        if (ModelState.IsValid)
        {
            User existingUser = await helpComingDbContext.Users
                .FirstOrDefaultAsync(u => u.Username == addUserViewModel.Username);

            if (existingUser != null)
            {
                ModelState.AddModelError("Username", "Такой логин уже зарегистрирован");
            }
            else
            {
                User user = new User()
                {
                    UserID = Guid.NewGuid(),
                    Username = addUserViewModel.Username,
                    Password = addUserViewModel.Password,
                    Email = addUserViewModel.Email,
                    RoleID = (int)Role.RoleTypes.User,
                    CountryID = addUserViewModel.CountryID
                };

                await helpComingDbContext.Users.AddAsync(user);
                await helpComingDbContext.SaveChangesAsync();
                return RedirectToAction("Login");
            }
        }

        addUserViewModel.Roles = await helpComingDbContext.Roles.Select(r => new SelectListItem
        {
            Value = r.RoleID.ToString(),
            Text = r.RoleName
        }).ToListAsync();

        addUserViewModel.Countries = await helpComingDbContext.Countries.Select(r => new SelectListItem
        {
            Value = r.CountryID.ToString(),
            Text = r.CountryName
        }).ToListAsync();

        return View(addUserViewModel);
    }
    #endregion

    #region Login
    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Profile");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            User user = await helpComingDbContext.Users.FirstOrDefaultAsync(u => u.Username == model.Username && u.Password == model.Password);

            if (user != null)
            {
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Role, (await helpComingDbContext.Roles.FirstOrDefaultAsync(r => r.RoleID == user.RoleID)).RoleName)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Profile");
            }
            else
            {
                ModelState.AddModelError("Username", "Неверные данные");
                ModelState.AddModelError("Password", "Неверные данные");
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region Profile
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        if (User.Identity.IsAuthenticated)
        {
            User user = await helpComingDbContext.Users.FirstOrDefaultAsync(x => x.UserID.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user != null)
            {
                UpdateUserViewModel model = new UpdateUserViewModel
                {
                    UserID = user.UserID,
                    Username = user.Username,
                    Password = user.Password,
                    Email = user.Email,
                    RoleID = user.RoleID,
                    CountryID = user.CountryID,

                    Roles = await helpComingDbContext.Roles.Select(r => new SelectListItem
                    {
                        Value = r.RoleID.ToString(),
                        Text = r.RoleName
                    }).ToListAsync(),

                    Countries = await helpComingDbContext.Countries.Select(c => new SelectListItem
                    {
                        Value = c.CountryID.ToString(),
                        Text = c.CountryName
                    }).ToListAsync()
                };
                return View(model);
            }
        }
        return RedirectToAction("Login");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateUserViewModel model)
    {
        User user = await helpComingDbContext.Users.FindAsync(model.UserID);
        if (user != null)
        {
            user.Username = model.Username;
            user.Password = model.Password;
            user.Email = model.Email;
            user.RoleID = model.RoleID;
            user.CountryID = model.CountryID;

            await helpComingDbContext.SaveChangesAsync();
        }
        return RedirectToAction("Index","Home");
    }
    #endregion

    #region Delete
    [HttpPost]
    public async Task<IActionResult> Delete(UpdateUserViewModel model)
    {
        User user = await helpComingDbContext.Users.FirstOrDefaultAsync(x => x.UserID == model.UserID);
        User currentUser = await helpComingDbContext.Users.FirstOrDefaultAsync(x => x.UserID.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (user != null && currentUser != null)
        {
            if (currentUser.Equals(user))
            {
                TempData["ConfirmMessage"] = "Вы уверены, что хотите удалить свой профиль? Это действие не может быть отменено";
                return View("DeleteConfirmation", user);
            }

            helpComingDbContext.Users.Remove(user);
            await helpComingDbContext.SaveChangesAsync();
        }

        if (currentUser?.RoleID == (int)Role.RoleTypes.Admin)
        {
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmDelete(Guid userID)
    {
        User user = await helpComingDbContext.Users.FindAsync(userID);
        if (user != null)
        {
            helpComingDbContext.Users.Remove(user);
            await helpComingDbContext.SaveChangesAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        return RedirectToAction("Index", "Home");
    }
    #endregion
}