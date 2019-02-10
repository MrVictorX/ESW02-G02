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

namespace ProjectSW.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ProjectSWUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ProjectSWUser> userManager, IEmailSender emailSender)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                var email = await _userManager.GetEmailAsync(user);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);

                var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                //Remover Key quando push
                var client = new SendGridClient("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");
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("QuintaDoMiao@exemplo.com", "Quinta do Miao"),
                    PlainTextContent = "Porfavor resete a sua password",
                    Subject = "Resetar password",
                    HtmlContent = $"Porfavor resete a sua password <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Clicando aqui</a>."
                };
                msg.AddTo(new EmailAddress(user.Email, user.Name));
                var response = await client.SendEmailAsync(msg);

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
