using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Final.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        ////[Required, DataType(DataType.EmailAddress)]
        ////public string Email { get; set; }
        //[Display(Name ="Remember")]
        //public bool Remember { get; set; }


    }
}
