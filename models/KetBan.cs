using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public class KetBan
    {
        public int id { get; set; }
        public int idnguoidung1 { get; set; }
        public int idnguoigui { get; set; }
        public int idnguoidung2 { get; set; }
        public string trangthai { get; set; }
        public DateTime thoigianketban { get; set; }
        public string anhdaidien { get; set; }
        public int idnguoidung { get; set; }
    }
}