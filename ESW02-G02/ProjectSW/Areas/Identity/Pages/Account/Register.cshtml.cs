using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using ProjectSW.Areas.Identity.Data;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace ProjectSW.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ProjectSWUser> _signInManager;
        private readonly UserManager<ProjectSWUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ProjectSWUser> userManager,
            SignInManager<ProjectSWUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required (ErrorMessage = "O Nome Completo é um campo obrigatório.")]
            [DataType(DataType.Text)]
            [Display(Name = "Nome completo")]
            public string Name { get; set; }

            [Required (ErrorMessage = "A Data de Nascimento é um campo obrigatório.")]
            [DataType(DataType.Date)]
            [Display(Name = "Data de Nascimento")]
            public DateTime DateOfBirth { get; set; }

            [Required (ErrorMessage = "O Tipo de Utilizador é um campo obrigatório.")]
            [DataType(DataType.Text)]
            [Display(Name = "Tipo Utilizador")]
            public string UserType { get; set; }

            [Required (ErrorMessage = "O Email é um campo obrigatório.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required (ErrorMessage = "A Password é um campo obrigatório.")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!_%*?&.,:;<>'-+])[A-Za-z\d@$!_%*?&.,:;<>'-+]{6,}$", ErrorMessage = "A {0} necessita de conter pelo menos uma letra Maiúscula, uma letra minúscula, um número, um caracter especial e no mínimo 6 caracteres.")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar password")]
            [Compare("Password", ErrorMessage = "A password e a confirmação da password não são iguais.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Page("./EmailSent");
            if (ModelState.IsValid)
            {
                var user = new ProjectSWUser {
                    Name = Input.Name,
                    DateOfBirth = Input.DateOfBirth,
                    UserType = Input.UserType,
                    UserName = Input.Email,
                    Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //  $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
                    var client = new SendGridClient("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");
                    var msg = new SendGridMessage()
                    {
                        From = new EmailAddress("QuintaDoMiao@exemplo.com", "Quinta do Miao"),
                        PlainTextContent = "Porfavor confirme o seu Email",
                        Subject = "Confirmar conta",
                        HtmlContent = $"Porfavor confirme o seu Email < a href = '{HtmlEncoder.Default.Encode(callbackUrl)}' > clicando aqui</ a >."
                    };
                    msg.AddTo(new EmailAddress(user.Email, user.Name));
                    var response = await client.SendEmailAsync(msg);

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

    }

}
