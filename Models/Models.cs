using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace hr.Models
{
    public class Department
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nama_department")]
        public string Nama_Department { get; set; }
    }
    public class Jabatan
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("nama_jabatan")]
        public string Nama_Jabatan { get; set; }
        [Column("gaji_pokok")]
        public double Gaji_Pokok { get; set; }
    }

    public class User
    {
        public User()
        {
            Role = "User";
        }

        [Column("id")]
        public int Id { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "Email wajib diisi")]
        public string Email { get; set; }

        [Column("nama_lengkap")]
        [Required(ErrorMessage = "Nama lengkap wajib diisi")]
        public string Nama_Lengkap { get; set; }

        [Column("password")]
        [Required(ErrorMessage = "Password wajib diisi")]
        public string Password { get; set; }

        [Column("id_jabatan")]
        [Required(ErrorMessage = "Jabatan wajib dipilih")]
        public int? Id_Jabatan { get; set; }

        [Column("id_department")]
        [Required(ErrorMessage = "Department wajib dipilih")]
        public int? Id_Department { get; set; }

        [Column("role")]
        public string? Role { get; set; }

        [ForeignKey("Id_Jabatan")]
        public Jabatan? Jabatan { get; set; }

        [ForeignKey("Id_Department")]
        public Department? Department { get; set; }
    }

}
