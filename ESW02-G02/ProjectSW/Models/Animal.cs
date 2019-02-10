using ProjectSW.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Animal
    {

        private readonly DateTime _minValue = DateTime.UtcNow.AddYears(-20);
        private readonly DateTime _maxValue = DateTime.UtcNow.AddYears(0);

        public string Id { get; set; }

        [Display(Name = "Raça")]
        public int BreedId { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Tamanho")]
        public string Size { get; set; }

        [Display(Name = "Genero")]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de nascimento")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de entrada")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Disponivel para adoção")]
        public bool Available { get; set; }
        
        [Display(Name = "Foto")]
        public byte[] Foto { get; set; }

        [Display(Name = "Raça")]
        public Breed Breed { get; set; }

        [Display(Name = "Anexos")]
        public  List<Attachment> Attachments { get; set; }
    }
}