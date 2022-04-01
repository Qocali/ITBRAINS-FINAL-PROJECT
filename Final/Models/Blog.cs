using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Final.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Creator { get; set; }
        public DateTime Time { get; set; }
        public string Idea { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
        public BlogDetail BlogDetail { get; set; }
        public bool IsDeactive { get; set; }

    }
}
