using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SubTitle { get; set; }
        public string Image { get; set; }
        
        [NotMapped]
        public IFormFile Photo { get; set; }
        public CourseCategories CourseCategories { get; set; }
        public int? CourseCategoriesId { get; set; }
        public CourseDetail CourseDetail { get; set; }
        public AppUser AppUser { get; set; }
        public string AppUserId { get; set; }
        
        public bool IsDeactive { get; set; }
       
    }
}
