using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class AnimalMonitoringReport
    {
        public string Id { get; set; }

        [Display(Name = "Pedido de adoção")]
        public string ExitFormId { get; set; }

        [Display(Name = "Informação da visita")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data de entrada")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Trabalhador")]
        public string EmployeeId { get; set; }

        [Display(Name = "Trabalhador")]
        public Employee Employee { get; set; }

        [Display(Name = "Pedido de adoção")]
        public ExitForm ExitForm { get; set; }
    }
}
