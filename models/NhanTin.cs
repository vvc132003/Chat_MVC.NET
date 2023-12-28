using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace models
{
    public class NhanTin
    {
        public int id { get; set; }
        public int idnguoidunggui { get; set; }
        public int idnguoidungnhan { get; set; }
        public string NoiDung { get; set; }
        public DateTime ThoiGianGui { get; set; }
    }
}
