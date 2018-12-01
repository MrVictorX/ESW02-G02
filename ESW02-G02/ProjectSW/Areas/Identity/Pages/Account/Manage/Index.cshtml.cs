using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectSW.Areas.Identity.Data;

namespace ProjectSW.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ProjectSWUser> _userManager;
        private readonly SignInManager<ProjectSWUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(
            UserManager<ProjectSWUser> userManager,
            SignInManager<ProjectSWUser> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Nome completo")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Morada")]
            public string Address { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [ValidateYears]
            [Display(Name = "Data de nascimento")]
            public DateTime DateOfBirth { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Tipo de utilizador")]
            public string UserType { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Numero de telemovel")]
            public string PhoneNumber { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Input = new InputModel
            {
                Name = user.Name,
                Address = user.Address,
                DateOfBirth = user.DateOfBirth,
                UserType = user.UserType,
                Email = email,
                PhoneNumber = phoneNumber
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user.Id == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
                await _userManager.UpdateAsync(user);
            }

            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
                await _userManager.UpdateAsync(user);
            }

            if (Input.DateOfBirth != user.DateOfBirth)
            {
                user.DateOfBirth = Input.DateOfBirth;
                await _userManager.UpdateAsync(user);
            }

            if (Input.UserType != user.UserType)
            {
                user.UserType = Input.UserType;
                await _userManager.UpdateAsync(user);
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "O seu perfil foi editado";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirma o teu email",
                $"Porfavor confirme a sua conta <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicando aqui</a>.");

            StatusMessage = "Email de verificação enviado.";
            return RedirectToPage();
        }
    }
}
