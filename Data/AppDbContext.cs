using Microsoft.EntityFrameworkCore;
using hr.Models;   // ganti sesuai namespace models kamu

namespace hr.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Jabatan> Jabatans { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
