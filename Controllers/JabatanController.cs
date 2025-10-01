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
    public class JabatanController : Controller
    {
        private readonly AppDbContext _context;

        public JabatanController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Jabatan
        public async Task<IActionResult> Index()
        {
            var Models = new JabatanVM.Index();
            var a = _context.Jabatans.ToList();
            foreach (var item in a)
            {
                var j = new Jabatan();
                j.Id = item.Id;
                j.Nama_Jabatan = item.Nama_Jabatan;
                j.Gaji_Pokok = item.Gaji_Pokok;
                Models.JabatanList.Add(j);
            }
            return View(Models);
        }

        // GET: Jabatan/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jabatan = await _context.Jabatans
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jabatan == null) return NotFound();

            var model = new JabatanVM.Details
            {
                Id = jabatan.Id,
                Nama_Jabatan = jabatan.Nama_Jabatan,
                Gaji_Pokok = jabatan.Gaji_Pokok
            };

            return View(model);
        }

        // GET: Jabatan/Create
        public IActionResult Create()
        {
            var Model = new Models.JabatanVM.Create();
            return View(Model);
        }

        // POST: Jabatan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nama_Jabatan,Gaji_Pokok")] Jabatan jabatan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jabatan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jabatan);
        }

        // GET: Jabatan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jabatan = await _context.Jabatans.FindAsync(id);
            if (jabatan == null)
            {
                return NotFound();
            }
            return View(jabatan);
        }

        // POST: Jabatan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nama_Jabatan,Gaji_Pokok")] Jabatan jabatan)
        {
            if (id != jabatan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jabatan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JabatanExists(jabatan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jabatan);
        }

        // GET: Jabatan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var jabatan = await _context.Jabatans.FindAsync(id);
            if (jabatan == null)
                return NotFound();

            var vm = new JabatanVM.Delete
            {
                Id = jabatan.Id,
                Nama_Jabatan = jabatan.Nama_Jabatan,
                Gaji_Pokok = jabatan.Gaji_Pokok
            };

            return View(vm);
        }

        // POST: Jabatan/Delete/5
        [HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var jabatan = await _context.Jabatans.FindAsync(id);
    if (jabatan == null)
        return NotFound();

    try
    {
        _context.Jabatans.Remove(jabatan);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Jabatan berhasil dihapus.";
    }
    catch (DbUpdateException)
    {
        TempData["ErrorMessage"] = "Tidak bisa menghapus jabatan ini karena masih digunakan.";
    }

    return RedirectToAction(nameof(Index));
}

        private bool JabatanExists(int id)
        {
            return _context.Jabatans.Any(e => e.Id == id);
        }
    }
}
