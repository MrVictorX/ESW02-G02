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

        [Display(Name = "Nome do Utilizador")]
        public string UserName { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data")]
        public DateTime EntryDate { get; set; }

        [Display(Name = "Trabalhador")]
        public string EmployeeId { get; set; }

    }
}
