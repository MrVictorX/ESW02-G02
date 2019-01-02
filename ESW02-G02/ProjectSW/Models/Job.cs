using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Job
    {
        public string Id { get; set; }
        public string EmployeeId { get; set; }

        [Display(Name = "Tarefa")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Dia")]
        public DateTime Day { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Hora")]
        public DateTime Hour { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Trabalhador")]
        public Employee Employee { get; set; }
    }
}
