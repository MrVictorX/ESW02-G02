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

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Morada")]
        public string Address { get; set; }

        [Display(Name = "Número de identificação")]
        public string CitizenCard { get; set; }

        [Display(Name = "Código Postal")]
        public string PostalCode { get; set; }
    }
}
