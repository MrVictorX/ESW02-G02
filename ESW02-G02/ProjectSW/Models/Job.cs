using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ProjectSW.Data;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ProjectSW.Models
{
    public class Job
    {
        public string Id { get; set; }
        [Display(Name = "Trabalhador")]
        public string EmployeeId { get; set; }

        [Display(Name = "Tarefa")]
        [Required(ErrorMessage = "O nome da tarefa é um campo obrigatório.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Dia")]
        [Required(ErrorMessage = "O dia é um campo obrigatório.")]
        public DateTime Day { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Hora")]
        [Required(ErrorMessage = "A hora é um campo obrigatório.")]
        public DateTime Hour { get; set; }

        [Display(Name = "Descrição")]
        public string Description { get; set; }

        [Display(Name = "Trabalhador")]
        public Employee Employee { get; set; }

    }
}
