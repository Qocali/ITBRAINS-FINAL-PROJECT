using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Final.ViewModels
{
    public class ChangeRoleVM
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public List<string> Roles { get; set; }
    }
}
