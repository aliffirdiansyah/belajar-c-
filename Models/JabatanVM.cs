namespace hr.Models
{
    public class JabatanVM
    {
        public class Index {
            public List<Jabatan> JabatanlIST { get; set; } = new List<Jabatan>();
        }

        public class Create
        {
            public string Nama_Jabatan { get; set; }
            public double Gaji_Pokok { get; set; }
        }

       
    }
}
