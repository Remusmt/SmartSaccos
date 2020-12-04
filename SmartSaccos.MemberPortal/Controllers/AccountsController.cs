using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartSaccos.ApplicationCore.DomainServices;
using SmartSaccos.Domains.Entities;
using SmartSaccos.MemberPortal.Helpers;
using SmartSaccos.MemberPortal.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartSaccos.MemberPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AppSettings appSettings;
        private readonly MemberService memberService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly CompanyService companyService;

        public AccountsController(
            MemberService membaService,
            CompanyService kompanyService,
            IOptions<AppSettings> AppSettings,
            UserManager<ApplicationUser> UserManager,
            SignInManager<ApplicationUser> SignInManager)
        {
            userManager = UserManager;
            signInManager = SignInManager;
            memberService = membaService;
            appSettings = AppSettings.Value;
            companyService = kompanyService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            var user = await userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                if (!await userManager.IsEmailConfirmedAsync(user))
                {
                    return BadRequest(new { error = "Confirm your email" });
                }
            }
            else
            {
                return BadRequest(new { error = "Invalid login attempt." });
            }
            try
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, true);
                if (result.Succeeded)
                {
                    //get member
                    Member member = await memberService.GetMemberByUserIdAsync(user.Id);
                    //Generate token
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret));
                    var signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                    var tokenString = new JwtSecurityToken(
                         issuer: "witsoft.co.ke",
                         audience: "witsoft.co.ke",
                         expires: DateTime.Now.AddDays(15),
                         claims: claims,
                         signingCredentials: signingCred
                        );

                    LoggedInUserViewModel loggedInUser = new LoggedInUserViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        OtherNames = member.OtherNames,
                        Surname = member.Surname,
                        PhoneNumber = user.PhoneNumber,
                        CompanyId = user.CompanyId,
                        CompanyName = "Nesadi Sacco", //bad practice
                        TokenString = new JwtSecurityTokenHandler().WriteToken(tokenString),
                        Succeeded = true,
                        MemberNumber = member.MemberNumber,
                        Status = member.MemberStatus,
                        WeKnowCustomer = member.MemberStatus > Domains.Enums.MemberStatus.KycPersonal,
                        MemberId = member.Id
                    };

                    return Ok(loggedInUser);
                }
                if (result.RequiresTwoFactor)
                {
                    return BadRequest(new { error = "You are a miracle worker." });
                }
                if (result.IsLockedOut)
                {
                    return BadRequest(new { error = "Account locked out." });
                }
                else
                {
                    return BadRequest(new { error = "Invalid username or password." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CountryId == 0)
                {
                    return BadRequest(new { new Exception("Invalid country").Message });
                }

                var company = await companyService.Register(model.CompanyName, model.CountryId, DateTimeOffset.UtcNow);

                if (company.Id == 0)
                {
                    return StatusCode(500, new { error = "Error creating company" });
                }

                var user = new ApplicationUser
                {
                    Email = model.Email,
                    FullName = $"{model.OtherNames} {model.Surname}",
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CompanyId = company.Id
                };
                try
                {
                    var result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {

                        var registeredUser = await userManager.FindByNameAsync(model.Email);

                        // Generate validation code
                        string code = await userManager.GenerateEmailConfirmationTokenAsync(registeredUser);

                        result = await userManager.ConfirmEmailAsync(registeredUser, code);

                        if (result.Succeeded)
                        {
                            // save member
                            Member member = await memberService.Add(new Member
                            {
                                ApplicationUserId = registeredUser.Id,
                                CompanyId = registeredUser.CompanyId,
                                CreatedBy = registeredUser.Id,
                                CreatedByName = registeredUser.FullName,
                                CreatedOn = DateTimeOffset.UtcNow,
                                DateJoined = DateTime.Now,
                                Email = user.Email,
                                MemberStatus = Domains.Enums.MemberStatus.Entered,
                                PhoneNumber = model.PhoneNumber,
                                OtherNames = model.OtherNames,
                                Surname = model.Surname
                            });
                            if (member.Id > 0)
                            {
                                List<Claim> claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.UserName)
                            };
                                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret));
                                var signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                                var tokenString = new JwtSecurityToken(
                                     issuer: "witsoft.co.ke",
                                     expires: DateTime.Now.AddDays(15),
                                     claims: claims,
                                     signingCredentials: signingCred
                                    );

                                LoggedInUserViewModel loggedInUser = new LoggedInUserViewModel
                                {
                                    Id = registeredUser.Id,
                                    Email = user.Email,
                                    OtherNames = member.OtherNames,
                                    Surname = member.Surname,
                                    PhoneNumber = user.PhoneNumber,
                                    CompanyId = user.CompanyId,
                                    CompanyName = company.CompanyName,
                                    TokenString = new JwtSecurityTokenHandler().WriteToken(tokenString),
                                    Succeeded = true,
                                    Status = Domains.Enums.MemberStatus.Entered,
                                    WeKnowCustomer = false,
                                    MemberId = member.Id
                                };
                                return Ok(loggedInUser);
                            }
                        }
                        else
                        {
                            return BadRequest(new { result.Errors.FirstOrDefault().Description });
                        }

                    }
                    return Ok(result);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { ex.Message });
                }

            }

            // If we got this far, something failed, redisplay form
            return BadRequest(model);
        }
    }
}
