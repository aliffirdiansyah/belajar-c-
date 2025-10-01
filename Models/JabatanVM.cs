namespace hr.Models
{
    public class JabatanVM
    {
        public class Index {
            public List<Jabatan> JabatanList { get; set; } = new List<Jabatan>();
        }

        public class JabatanListItem
        {
            public int Id { get; set; }
            public string Nama_Jabatan { get; set; }
            public double Gaji_Pokok { get; set; }
            public int JumlahPegawai { get; set; }
        }

        public class Create
        {
            public string Nama_Jabatan { get; set; }
            public double Gaji_Pokok { get; set; }
        }
       
        public class Edit
        {
            public string Nama_Jabatan { get; set; }
            public double Gaji_Pokok { get; set; }
        }

        public class Details
        {
            public int Id { get; set; }
            public string Nama_Jabatan { get; set; }
            public double Gaji_Pokok { get; set; }
        }

        public class Delete
        {
            public int Id { get; set; }
            public string Nama_Jabatan { get; set; }
            public double Gaji_Pokok { get; set; }
        }
    }
}
