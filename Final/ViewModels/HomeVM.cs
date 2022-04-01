using Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.ViewModels
{
    public class HomeVM
    {
       
        public List< Slider> Slider { get; set; }
        public About About { get; set; }
        public List<Board> Boards { get; set; }
        public List<Events> Events { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Course> Course { get; set; }
        public Bio Bios { get; set; }
       
        public Subscribe subscribes { get; set; }


        
    }
}
