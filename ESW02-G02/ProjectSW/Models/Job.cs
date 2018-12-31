using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Job
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Day { get; set; }
        public DateTime Hour { get; set; }
        public string Description { get; set; }


    }
}
