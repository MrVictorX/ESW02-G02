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

        [Display(Name = "Nome Animal")]
        public string AnimalId { get; set; }

        public string ReportId { get; set; }

        [Display(Name = "Nome do Adotante")]
        public string AdopterName { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Motivo")]
        public string Motive { get; set; }


        public Animal Animal { get; set; }


        public AnimalMonitoringReport Report { get; set; }
    }

   }

