using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public class NguoiDung
    {
        public int id { get; set; }
        public string tendangnhap { get; set; }
        public string matkhau { get; set; }
        public string email { get; set; }
        public string hovaten { get; set; }
        public string anhdaidien { get; set; }
        public string trangthai { get; set; }
        public DateTime ngaythamgia { get; set; }
    }
}
