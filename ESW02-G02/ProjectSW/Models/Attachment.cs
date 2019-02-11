using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Attachment
    {
        public string Id { get; set; }

        [Display(Name = "Animal")]
        public string AnimalId { get; set; }

        [Display(Name = "Animal")]
        public Animal Animal { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Ficheiros")]
        public byte[] File { get; set; }
    }
}
