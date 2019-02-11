using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class ExitForm
    {
        public string Id { get; set; }

        [Display(Name = "Animal")]
        public string AnimalId { get; set; }

        [Display(Name = "Nome do Adotante")]
        public string AdopterName { get; set; }

        [Display(Name = "Morada do adotante")]
        public string AdopterAddress { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail")]
        public string AdopterEmail { get; set; }

        [Display(Name = "Numero de identificação")]
        public string AdopterCitizenCard { get; set; }

        [Display(Name = "Codigo Postal")]
        public string AdopterPostalCode { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Motivo")]
        public string Motive { get; set; }

        [Display(Name = "Informação Adicional")]
        public string Description { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "Animal")]
        public Animal Animal { get; set; }
    }

}