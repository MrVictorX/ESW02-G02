using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectSW.Data;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ProjectSW.Views.Jobs
{
    public class Create : PageModel
    {
        private readonly UserManager<ProjectSWUser> _userManager;
        private readonly IEmailSender _emailSender;

        public Create(UserManager<ProjectSWUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            //Antes de fazer push remover chave
            var client = new SendGridClient("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");
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("QuintaDoMiao@exemplo.com", "Quinta do Miao"),
                PlainTextContent = "Nova tarefa",
                Subject = "Nova tarefa",
                HtmlContent = "Olá " + user.Name + ".\n\nFoi-lhe atribuido uma nova tarefa a completar, por favor verifique a lista de tarefas.\n\n" +
                "Cumprimentos,\nQuinta do Mião."
            };
            msg.AddTo(new EmailAddress(user.Email, user.Name));
            var response = await client.SendEmailAsync(msg);

            return RedirectToPage("~/Views/Jobs/Index");
        }

    }
}
