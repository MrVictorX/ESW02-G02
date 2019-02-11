using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Breed
    {
        public int Id { get; set; }

        [Display(Name = "Raça")]
        public string Name { get; set; }

    }
}
