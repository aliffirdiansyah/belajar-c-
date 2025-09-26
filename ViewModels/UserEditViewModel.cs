using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hr.ViewModels
{
    public class UserEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama lengkap wajib diisi")]
        public string Nama_Lengkap { get; set; }

        [Required(ErrorMessage = "Jabatan wajib dipilih")]
        public int? Id_Jabatan { get; set; }

        [Required(ErrorMessage = "Department wajib dipilih")]
        public int? Id_Department { get; set; }
    }
}
