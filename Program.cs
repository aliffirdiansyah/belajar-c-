using BCrypt.Net;
using hr.Data;
using hr.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("dbConn")));

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options => {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

var app = builder.Build();

// 🔥 Seeder Admin User
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();

    if (!context.Users.Any(u => u.Email == "admin@hr.com"))
    {
        var admin = new User
        {
            Email = "admin@hr.com",
            Nama_Lengkap = "Administrator",
            Password = BCrypt.Net.BCrypt.HashPassword("admin123"), // ✅ hash password
            Role = "Admin",
            Id_Department = 7, // sesuaikan kalau ada relasi
            Id_Jabatan = 1     // sesuaikan kalau ada relasi
        };

        context.Users.Add(admin);
        context.SaveChanges();
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();
