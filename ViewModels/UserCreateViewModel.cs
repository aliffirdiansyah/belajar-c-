using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hr.ViewModels
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Email wajib diisi")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Nama lengkap wajib diisi")]
        public string Nama_Lengkap { get; set; }

        [Required(ErrorMessage = "Password wajib diisi")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Jabatan wajib dipilih")]
        public int? Id_Jabatan { get; set; }

        [Required(ErrorMessage = "Department wajib dipilih")]
        public int? Id_Department { get; set; }
    }
}
