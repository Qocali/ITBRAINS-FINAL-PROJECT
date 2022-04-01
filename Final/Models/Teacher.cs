using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public class Teacher

    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
        public TeacherDetails Teacherdetails { get; set; }
        
        public SocialMedia SocialMedia { get; set; }
        public Profession Profession { get; set; }
        [ForeignKey("Profession")]
        public int ProfessionId { get; set; }
       
        public bool IsDeactive { get; set; }
        public List<TeacherSkillsmany> TeacherSkillsmanys { get; set; }

    }
}
