using hr.Data;
using hr.Models;
using hr.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace hr.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(AppDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // ================== INDEX ==================
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .Include(u => u.Jabatan)
                .Include(u => u.Department)
                .ToListAsync();

            return View(users);
        }

        // ================== DETAILS ==================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(u => u.Jabatan)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null) return NotFound();

            return View(user);
        }

        // ================== CREATE ==================
        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User
                    {
                        Email = model.Email,
                        Nama_Lengkap = model.Nama_Lengkap,
                        Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                        Id_Jabatan = model.Id_Jabatan,
                        Id_Department = model.Id_Department,
                        Role = "User"
                    };

                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saat membuat user baru");
                    ModelState.AddModelError(string.Empty, "Gagal menyimpan data user.");
                }
            }

            LoadDropdowns(model.Id_Jabatan, model.Id_Department);
            return View(model);
        }


        // ================== EDIT ==================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var model = new UserEditViewModel
            {
                Id = user.Id,
                Nama_Lengkap = user.Nama_Lengkap,
                Id_Jabatan = user.Id_Jabatan,
                Id_Department = user.Id_Department
            };

            LoadDropdowns(user.Id_Jabatan, user.Id_Department);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FindAsync(id);
                    if (user == null) return NotFound();

                    // update hanya field tertentu
                    user.Nama_Lengkap = model.Nama_Lengkap;
                    user.Id_Jabatan = model.Id_Jabatan;
                    user.Id_Department = model.Id_Department;

                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saat mengedit user");
                    ModelState.AddModelError(string.Empty, "Gagal mengupdate data user.");
                }
            }

            LoadDropdowns(model.Id_Jabatan, model.Id_Department);
            return View(model);
        }

        // ================== DELETE ==================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(u => u.Jabatan)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // ================== HELPER ==================
        private void LoadDropdowns(int? selectedJabatan = null, int? selectedDepartment = null)
        {
            ViewBag.JabatanId = new SelectList(_context.Jabatans, "Id", "Nama_Jabatan", selectedJabatan);
            ViewBag.DepartmentId = new SelectList(_context.Departments, "Id", "Nama_Department", selectedDepartment);
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
