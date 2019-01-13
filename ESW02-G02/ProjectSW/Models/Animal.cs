using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Animal
    {
        public string Id { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Tamanho")]
        public string Size { get; set; }

        [Display(Name = "Género")]
        public string Gender { get; set; }

        [Display(Name = "Raça")]
        public string BreedId { get; set; }
        public Breed Breed { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de entrada")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Foto")]
        public byte[] Foto { get; set; }

        [Display(Name = "Ficheiro")]
        public string FileName { get; set; }

        [Display(Name = "Anexo")]
        public byte[] Attachment { get; set; }
    }
}