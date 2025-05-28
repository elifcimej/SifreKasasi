using Microsoft.AspNetCore.Mvc;
using SifreKasasi.Data;
using SifreKasasi.Data.Models;
using SifreKasasi.API;

namespace SifreKasasi.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly PasswordVaultDbContext _context;

        public AccountController(PasswordVaultDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromBody] AppUser user)
        {
            if (_context.AppUsers.Any(x => x.Username == user.Username))
                return BadRequest("Bu kullanıcı adı zaten var!");

            user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash); //hash
            _context.AppUsers.Add(user);
            _context.SaveChanges();

            return Ok("Kayıt başarılı!");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            var user = _context.AppUsers.FirstOrDefault(x => x.Username == request.Username);

            if (user == null)
                return BadRequest("Kullanıcı bulunamadı");

            var hashedPassword = PasswordHasher.HashPassword(request.Password);

            if (user.PasswordHash != hashedPassword)
                return BadRequest("Şifre yanlış");

            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Username", user.Username);

            return Ok("Giriş başarılı!");
        }
        [HttpGet]
        public IActionResult Passwords()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        [HttpPost]
        public IActionResult AddPassword([FromBody] AccountData data)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var existing = _context.Accounts.FirstOrDefault(x =>
                x.AppUserId == userId && x.SiteName == data.SiteName);

            if (existing != null)
            {
                existing.SiteUsername = data.SiteUsername;
                existing.EncryptedPassword = EncryptionHelper.Encrypt(data.EncryptedPassword);
                _context.SaveChanges();
                return Ok("Güncellendi.");
            }
            else
            {
                data.AppUserId = userId.Value;
                data.EncryptedPassword = EncryptionHelper.Encrypt(data.EncryptedPassword);
                _context.Accounts.Add(data);
                _context.SaveChanges();
                return Ok("Eklendi.");
            }
        }


        [HttpGet]
        public IActionResult GetPasswords()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var accounts = _context.Accounts
                .Where(a => a.AppUserId == userId)
                .Select(a => new
                {
                    a.SiteName,
                    a.SiteUsername,
                    Password = EncryptionHelper.Decrypt(a.EncryptedPassword)
                }).ToList();

            return Json(accounts);
        }

        [HttpPost]
        public IActionResult DeletePassword([FromBody] string siteName)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var record = _context.Accounts.FirstOrDefault(x => x.AppUserId == userId && x.SiteName == siteName);
            if (record == null) return NotFound("Kayıt bulunamadı");

            _context.Accounts.Remove(record);
            _context.SaveChanges();

            return Ok("Silindi");
        }

        [HttpPost]
        public IActionResult UpdatePassword([FromBody] AccountData updated)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return Unauthorized();

            var record = _context.Accounts.FirstOrDefault(x => x.AppUserId == userId && x.SiteName == updated.SiteName);
            if (record == null) return NotFound("Kayıt bulunamadı");

            record.SiteUsername = updated.SiteUsername;
            record.EncryptedPassword = EncryptionHelper.Encrypt(updated.EncryptedPassword);
            _context.SaveChanges();

            return Ok("Güncellendi");
        }



    }
}
