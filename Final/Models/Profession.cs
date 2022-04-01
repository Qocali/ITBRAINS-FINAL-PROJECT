using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public class Profession
    {
        public int Id { get; set; }
        public string WorkName { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
