using hr.Data;
using hr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hr.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Department
        public async Task<IActionResult> Index()
        {
            var Models = new DepartmentVM.Index();
            var b = _context.Departments.ToList();
            foreach (var item in b)
            {
                var d = new Department();
                d.Id= item.Id;
                d.Nama_Department = item.Nama_Department;
                Models.DepartmenList.Add(d);
            }

            return View(Models);
        }

        // GET: Department/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            // Ambil department beserta list user-nya
            var department = await _context.Departments
                .Include(d => d.Users) // pastikan relasi Department -> Users ada
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null) return NotFound();

            var model = new DepartmentVM.Details
            {
                Id = department.Id,
                Nama_Department = department.Nama_Department,
                JumlahPegawai = department.Users?.Count ?? 0 // hitung jumlah karyawan
            };

            return View(model);
        }




        // GET: Department/Create
        public IActionResult Create()
        {
            var Model = new Models.DepartmentVM.Create();
            return View(Model);
        }

        // POST: Department/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nama_Department")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Department/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();

            var depertment = new DepartmentVM.Edit
            {
                Id = department.Id,
                Nama_Department = department.Nama_Department
            };

            return View(depertment);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentVM.Edit departmentEdit)
        {
            if (id != departmentEdit.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var department = await _context.Departments.FindAsync(id);
                    if (department == null) return NotFound();

                    // mapping VM → Entity
                    department.Nama_Department = departmentEdit.Nama_Department;

                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(departmentEdit.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departmentEdit);
        }



        // GET: Department/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);

            if (department == null) return NotFound();

            var model = new DepartmentVM.Delete
            {
                Id = department.Id,
                Nama_Department = department.Nama_Department
            };

            return View(model);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
