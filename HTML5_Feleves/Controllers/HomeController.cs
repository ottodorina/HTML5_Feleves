using HTML5_Feleves.Data;
using HTML5_Feleves.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace HTML5_Feleves.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;

        public HomeController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<HomeController> logger, ApplicationDbContext db, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _db = db;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> DelegateAdmin()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            var role = new IdentityRole()
            {
                Name = "Admin"
            };
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(role);
            }
            user.IsAdmin = true;
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View(_db.Tables);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(Models.Table table)
        {

            var old = _db.Tables.FirstOrDefault(t => t.Id == table.Id);
            if (old == null)
            {
                _db.Tables.Add(table);
                _db.SaveChanges();
            }

            var user = await _userManager.GetUserAsync(this.User);
            await _emailSender.SendEmailAsync(user.FirstName, "Table added!", $"{table.Name}");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int tableId)
        {
            var table = _db.Tables.FirstOrDefault(t => t.Id == tableId);
            ViewBag.Table = table;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Models.Table table)
        {
            if (table != null)
            {
                _db.Tables.Update(table);
                _db.SaveChanges();
            }
            else
            {
                return StatusCode(500);
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDelete(int uid)
        {
            var item = _db.Tables.FirstOrDefault(t => t.Id == uid);
            if (item != null)
            {
                _db.Tables.Remove(item);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminFree(int uid)
        {
            Models.Table table = _db.Tables.FirstOrDefault(t => t.Id == uid);
            table.Reserved = false;
            if (table != null)
            {
                _db.Tables.Update(table);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminLock(int uid)
        {
            Models.Table table = _db.Tables.FirstOrDefault(t => t.Id == uid);
            table.Reserved = true;
            if (table != null)
            {
                _db.Tables.Update(table);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Privacy()
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Reserv(int tableId)
        {
            var table = _db.Tables.FirstOrDefault(t => t.Id == tableId);
            ViewBag.Table = table;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reserv(Reservation reservation)
        {
            var old = _db.Reservations.FirstOrDefault(t => t.Id == reservation.Id);
            var table = _db.Tables.FirstOrDefault(t => t.Id == reservation.Table.Id);
            table.Reserved = true;

            if (old == null&&Convert.ToInt64(table.Capacity)>= Convert.ToInt64(reservation.Person))
            {
                _db.Reservations.Add(reservation);
                _db.Tables.Update(table);
                _db.SaveChanges();
            }
            else
            {
                return StatusCode(500);
            }
            return RedirectToAction(nameof(Index));

        }

    }
}