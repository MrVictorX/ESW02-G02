﻿using ProjectSW.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Employee
    {
        public string Id { get; set; }

        [Display(Name = "E-mail")]
        public string AccountId { get; set; }

        [Display(Name = "Cargo")]
        public string Type { get; set; }

        [Display(Name = "Informação Adicional")]
        public string AditionalInformation { get; set; }

        [Display(Name = "Conta")]
        public ProjectSWUser Account { get; set; }

        
    }


}