using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public class TeacherSkills
    {
        public int Id { get; set; }
        public string Scills { get; set; }
      public int Percent { get; set; }
        public List<TeacherSkillsmany> TeacherSkillsmanys { get; set; }
       
     
    }
}
