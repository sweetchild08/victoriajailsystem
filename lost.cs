using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eserve2
{
    public class lost
    {
        public List<Lostnfound> data { get; set; }

       
    }
    public class Lostnfound
    {   public int id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string contact { get; set; }
        public string subject { get; set; }
        public string location { get; set; }
        public string notes { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string status { get; set; }
        public string email { get; set; }
        public int isopened { get; set; }
    }
    public class response
    {
        public string status { get; set; }
        public string msg { get; set; }
    }
}
