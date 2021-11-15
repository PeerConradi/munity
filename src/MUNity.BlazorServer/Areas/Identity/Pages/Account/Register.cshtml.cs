using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MUNity.Database.Context;
using MUNity.Database.Models.User;
using MUNity.Services;

namespace MUNityCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<MunityUser> _signInManager;
        private readonly UserManager<MunityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IMailService _emailSender;
        private readonly MunityContext _dbContext;

        public RegisterModel(
            UserManager<MunityUser> userManager,
            SignInManager<MunityUser> signInManager,
            ILogger<RegisterModel> logger,
            IMailService emailSender,
            MunityContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _dbContext = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public enum RegistrationStates
        {
            Waiting,
            Success,
            FollowedInvitation
        }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [MinLength(3)]
            [MaxLength(30)]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [MaxLength(200)]
            public string Forename { get; set; }

            [Required]
            [MaxLength(200)]
            public string Lastname { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [Display(Name = "Birthday Year")]
            public int BirthdayYear { get; set; } = DateTime.Now.Year - 13;

            [Required]
            [Display(Name = "Birthday Month")]
            public int BirthdayMonth { get; set; } = 1;

            [Required]
            [Display(Name = "Birthday Day")]
            public int BirthdayDay { get; set; } = 1;

            [Required]
            public bool AcceptedAGB { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid && Input.AcceptedAGB)
            {
                var userFound = await _userManager.FindByEmailAsync(Input.Email);
                if (userFound != null)
                {
                    if (userFound.IsShadowUser)
                    {
                        userFound.UserName = Input.Username;
                        userFound.Forename = Input.Forename;
                        userFound.Lastname = Input.Lastname;
                        await _userManager.RemovePasswordAsync(userFound);
                        await _userManager.ChangePasswordAsync(userFound, String.Empty, Input.Password);
                        _dbContext.Update(userFound);
                        _dbContext.SaveChanges();
                        await _signInManager.SignInAsync(userFound, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    _logger.LogWarning($"User already exisits and was not a shadow user: {Input.Username}");
                }
                else
                {
                    var user = new MunityUser
                    {
                        UserName = Input.Username,
                        Email = Input.Email,
                        RegistrationDate = DateTime.UtcNow,
                        Birthday = new DateOnly(Input.BirthdayYear, Input.BirthdayMonth, Input.BirthdayDay),
                        Forename = Input.Forename,
                        Lastname = Input.Lastname
                    };

                    var result = await _userManager.CreateAsync(user, Input.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        //var callbackUrl = Url.Page(
                        //    "/Account/ConfirmEmail",
                        //    pageHandler: null,
                        //    values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        //    protocol: Request.Scheme);

                        //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        //{
                        //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        //}
                        //else
                        //{
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                        //}
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                
            }
            else
            {
                Console.WriteLine("AGB not accepted!");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
