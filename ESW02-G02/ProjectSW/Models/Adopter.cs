using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Adopter
    {
        public string Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool Address { get; set; }
    }
}
