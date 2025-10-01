using hr.Data;
using hr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace hr.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(AppDbContext context, ILogger<UserController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new Models.UserVM.Index();

            model.UserList = await _context.Users
                .Include(u => u.Jabatan)
                .Include(u => u.Department)
                .Select(u => new Models.UserVM.UserListItem
                {
                    Id = u.Id,
                    Email = u.Email,
                    Nama_Lengkap = u.Nama_Lengkap,
                    Nama_Jabatan = u.Jabatan != null ? u.Jabatan.Nama_Jabatan : "-",
                    Nama_Department = u.Department != null ? u.Department.Nama_Department : "-",
                    Role = u.Role
                })
                .ToListAsync();

            return View(model);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(u => u.Jabatan)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null) return NotFound();

            var model = new Models.UserVM.Details
            {
                Id = user.Id,
                Email = user.Email,
                Nama_Lengkap = user.Nama_Lengkap,
                Nama_Jabatan = user.Jabatan?.Nama_Jabatan,
                Nama_Department = user.Department?.Nama_Department,
                Role = user.Role
            };

            return View(model);
        }

        public IActionResult Create()
        {
            var vm = new Models.UserVM.Create();
            LoadDropdowns();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.UserVM.Create vm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User
                    {
                        Email = vm.Email,
                        Nama_Lengkap = vm.Nama_Lengkap,
                        Password = BCrypt.Net.BCrypt.HashPassword(vm.Password),
                        Id_Jabatan = vm.Id_Jabatan,
                        Id_Department = vm.Id_Department,
                        Role = "User"
                    };

                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "User berhasil ditambahkan.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saat membuat user baru");
                    TempData["ErrorMessage"] = "Gagal menyimpan data user.";
                }
            }

            LoadDropdowns(vm.Id_Jabatan, vm.Id_Department);
            return View(vm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            var vm = new Models.UserVM.Edit
            {
                Id = user.Id,
                Email = user.Email,
                Nama_Lengkap = user.Nama_Lengkap,
                Id_Jabatan = user.Id_Jabatan,
                Id_Department = user.Id_Department
            };

            LoadDropdowns(user.Id_Jabatan, user.Id_Department);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.UserVM.Edit vm)
        {
            if (id != vm.Id)
                return Json(new { success = false, errors = new { general = "ID tidak cocok" } });

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Users.FindAsync(id);
                    if (user == null)
                        return Json(new { success = false, errors = new { general = "User tidak ditemukan" } });

                    user.Nama_Lengkap = vm.Nama_Lengkap;
                    user.Id_Jabatan = vm.Id_Jabatan;
                    user.Id_Department = vm.Id_Department;

                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saat mengedit user");
                    return Json(new { success = false, errors = new { general = "Gagal menyimpan data user" } });
                }
            }

            // Ambil semua error validasi dan kirim sebagai JSON
            var errors = ModelState
                .Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return Json(new { success = false, errors });
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var user = await _context.Users
                .Include(u => u.Jabatan)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null) return NotFound();

            var vm = new Models.UserVM.Delete
            {
                Id = user.Id,
                Nama_Lengkap = user.Nama_Lengkap,
                Email = user.Email,
                Nama_Jabatan = user.Jabatan?.Nama_Jabatan,
                Nama_Department = user.Department?.Nama_Department
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "User berhasil dihapus.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Gagal menghapus user");
                TempData["ErrorMessage"] = "Tidak bisa menghapus user ini karena masih digunakan.";
            }

            return RedirectToAction(nameof(Index));
        }

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
