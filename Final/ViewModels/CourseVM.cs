using Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.ViewModels
{
    public class CourseVM
    {
        public Course Course { get; set; }
        public List<Course> Courses { get; set; }
        public List<Events> Events { get; set; }
        public List<Blog> blogs { get; set; }
        public List<Teacher> Teachers { get; set; }
    }
}
