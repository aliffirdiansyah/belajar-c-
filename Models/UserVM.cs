using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace hr.Models.UserVM
{
    // ============ INDEX ============
    public class Index
    {
        public List<UserListItem> UserList { get; set; } = new List<UserListItem>();
    }

    public class UserListItem
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nama_Lengkap { get; set; }
        public string Nama_Jabatan { get; set; }
        public string Nama_Department { get; set; }
        public string Role { get; set; }
    }


    // ============ CREATE ============
    public class Create
    {
        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nama lengkap wajib diisi")]
        public string Nama_Lengkap { get; set; }

        [Required(ErrorMessage = "Password wajib diisi")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Jabatan wajib dipilih")]
        public int? Id_Jabatan { get; set; }

        [Required(ErrorMessage = "Department wajib dipilih")]
        public int? Id_Department { get; set; }

        public string? Role { get; set; } = "User";

        // Dropdowns
        public IEnumerable<SelectListItem>? JabatanList { get; set; }
        public IEnumerable<SelectListItem>? DepartmentList { get; set; }
    }

    // ============ EDIT ============
    public class Edit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email wajib diisi")]
        [EmailAddress(ErrorMessage = "Format email tidak valid")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nama lengkap wajib diisi")]
        public string Nama_Lengkap { get; set; }

        [Required(ErrorMessage = "Jabatan wajib dipilih")]
        public int? Id_Jabatan { get; set; }

        [Required(ErrorMessage = "Department wajib dipilih")]
        public int? Id_Department { get; set; }

        public string? Role { get; set; }

        // Dropdowns
        public IEnumerable<SelectListItem>? JabatanList { get; set; }
        public IEnumerable<SelectListItem>? DepartmentList { get; set; }
    }

    // ============ DELETE ============
    public class Delete
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nama_Lengkap { get; set; }
        public string? Role { get; set; }
        public string? Nama_Jabatan { get; set; }
        public string? Nama_Department { get; set; }
    }

    // ============ DETAILS ============
    public class Details
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Nama_Lengkap { get; set; }
        public string? Role { get; set; }
        public string? Nama_Jabatan { get; set; }
        public string? Nama_Department { get; set; }
    }
}
