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
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Day { get; set; }

        [DataType(DataType.Time)]
        public DateTime Hour { get; set; }

        public string Description { get; set; }

        public Employee Employee { get; set; }

        public string EmployeeId { get; set; }
    }
}
