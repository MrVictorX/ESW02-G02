using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Attachment
    {
        public string Id { get; set; }

        public string AnimalId { get; set; }

        public Animal Animal { get; set; }

        public string Name { get; set; }

        public byte[] File { get; set; }
    }
}
