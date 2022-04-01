using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public string   Description { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public DateTime STARTS { get; set; }
        public DateTime DURATION { get; set; }

        public string SKILLLEVEL{ get; set; }
        public string LANGUAGE { get; set; }
        public int STUDENTS { get; set; }
        public string ASSESMENTS { get; set; }
        public Course Course { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
    }
}
