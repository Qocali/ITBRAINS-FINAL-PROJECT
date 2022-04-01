using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public class CourseCategories
    {
        
        public int Id { get; set; }
        public string CourseName { get; set; }
        public List<Course> Courses { get; set; }
        public bool IsDeactive { get; set; }

    }
}
