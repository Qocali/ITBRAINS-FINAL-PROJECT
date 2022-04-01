using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public class TeacherSkillsmany
    {
        public int Id { get; set; }
        public Teacher Teacher { get; set; }
        public int TeacherId { get; set; }
        public TeacherSkills TeacherSkills { get; set; }
        public int TeacherSkillsId { get; set; }
    }
}
