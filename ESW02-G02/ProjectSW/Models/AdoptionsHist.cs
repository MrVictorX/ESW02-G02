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

        [Display(Name = "E-mail do adotante")]
        public string AdopterEmail { get; set; }

        [Display(Name = "Morada do adotante")]
        public string AdopterAddress { get; set; }

        [Display(Name = "Cartão de Cidadão do adotante")]
        public string AdopterCitizenCard { get; set; }

        [Display(Name = "Número de identificação")]
        public string AdopterPostalCode { get; set; }

        [Display(Name = "Motivo")]
        public string Motive { get; set; }

        [Display(Name = "Data de nascimento do animal")]
        [DataType(DataType.Date)]
        public DateTime AnimalDateOfBirth { get; set; }

        [Display(Name = "Raça")]
        public string AnimalBreedName { get; set; }

        [Display(Name = "Género")]
        public string AnimalGender { get; set; }

        [Display(Name = "Informação do animal")]
        public string AditionalInformation { get; set; }

        [Display(Name = "Resultado")]
        public string Result { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de entrada")]
        public DateTime EntryDate { get; set; }
    }
}
