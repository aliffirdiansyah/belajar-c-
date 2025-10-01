namespace hr.Models
{
    public class DepartmentVM
    {
        public class Index
        {
            public List<Department> DepartmenList { get; set; } = new List<Department>();
        }

        public class DepartmentListItem
        {
            public int Id { get; set; }
            public string Nama_Department { get; set; }
            public int JumlahPegawai { get; set; }
        }

        public class Details
        {
            public int Id { get; set; }
            public string Nama_Department { get; set; }
            public int JumlahPegawai { get; set; }
        }

        public class Create
        {
            public string Nama_Department { get; set; }
        }

        public class Edit
        {
            public int Id { get; set; }
            public string Nama_Department { get; set; }
        }

        public class Delete
        {
            public int Id { get; set; }
            public string Nama_Department { get; set; } = string.Empty;
        }

    }
}
   