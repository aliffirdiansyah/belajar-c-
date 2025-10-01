using hr.Data;
using hr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HRApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Ambil data department dengan jumlah pegawai
            var departments = _context.Departments
                .Select(d => new DepartmentVM.DepartmentListItem
                {
                    Id = d.Id,
                    Nama_Department = d.Nama_Department,
                    JumlahPegawai = d.Users.Count()   // asumsi ada relasi Users    
                })
                .Where(d => d.JumlahPegawai > 0) // hanya yang ada pegawai
                .ToList();

            // Ambil data jabatan dengan jumlah pegawai
            var jabatans = _context.Jabatans
    .Select(d => new JabatanVM.JabatanListItem
    {
        Id = d.Id,
        Nama_Jabatan = d.Nama_Jabatan,   // benar
        Gaji_Pokok = d.Gaji_Pokok,
        JumlahPegawai = d.Users.Count()  // hitung user per jabatan
    })
    .Where(d => d.JumlahPegawai > 0) // hanya yang ada pegawai
    .ToList();


            // Convert ke ViewBag
            ViewBag.LabelsDepartment = departments.Select(d => d.Nama_Department).ToList();
            ViewBag.DataDepartment = departments.Select(d => d.JumlahPegawai).ToList();
            ViewBag.LabelsJabatan = jabatans.Select(j => j.Nama_Jabatan).ToList();
            ViewBag.DataJabatan = jabatans.Select(j => j.JumlahPegawai).ToList();



            return View();
        }
    }
}
