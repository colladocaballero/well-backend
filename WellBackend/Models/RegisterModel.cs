using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellBackend.Models
{
    public class RegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
    }
}
