using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class AdoptionsHist
    {
        public string Id { get; set; }

        public string AdopterEmail { get; set; }

        public string AdopterAddress { get; set; }

        [DataType(DataType.Date)]
        public DateTime AnimalDateOfBirth { get; set; }

        public string AnimalBreed { get; set; }

        public string AnimalGender { get; set; }

        public string Result { get; set; }

        [DataType(DataType.Date)]
        public DateTime EntryDate { get; set; }
    }
}
