using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartSaccos.ApplicationCore.DomainServices;
using SmartSaccos.ApplicationCore.Models;
using SmartSaccos.ApplicationCore.Services;
using SmartSaccos.Domains.Entities;
using SmartSaccos.Domains.Enums;
using SmartSaccos.MemberPortal.Helpers;
using SmartSaccos.MemberPortal.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartSaccos.MemberPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly AppSettings appSettings;
        private readonly MemberService memberService;
        private readonly MessageService messageService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AdminController(
            MessageService msgService,
            MemberService membaService,
            IOptions<AppSettings> AppSettings,
            UserManager<ApplicationUser> UserManager,
            SignInManager<ApplicationUser> SignInManager)
        {
            userManager = UserManager;
            messageService = msgService;
            signInManager = SignInManager;
            memberService = membaService;
            appSettings = AppSettings.Value;
        }

        
        [AllowAnonymous]
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
                if (user.UserType == UserType.Member)
                {
                    return Unauthorized();
                }
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
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret));
                    var signingCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                    var claims = new[] { new Claim(ClaimTypes.Name, user.UserName) };


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

        [HttpGet("GetMembers")]
        public async Task<ActionResult<List<Member>>> GetMembers()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return Unauthorized();
            if (user.UserType != UserType.Admin)
                return Unauthorized();
            return await memberService.GetDetailedMembersAsync(user.CompanyId);
        }

        [HttpPost("ApproveMember")]
        public async Task<ActionResult<Member>> ApproveMember(ApprovalModel model)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return Unauthorized();
                if (user.UserType != UserType.Admin)
                    return Unauthorized();

                MemberApproval memberApproval = new MemberApproval
                {
                    ApplicationUserId = user.Id,
                    ApprovalAction = ApprovalAction.Approved,
                    Comments = model.Comments,
                    MemberId = model.MemberId,
                    MessageToMember = model.MessageToMember
                };
                Member member = await memberService.ApproveMember(memberApproval);

                string html = Properties.Resources.ApprovedEmailTemplate;
                html = html.Replace("{Username}", $"{member.OtherNames} {member.Surname}");
                html = html.Replace("{memberno}", member.MemberNumber);

                await messageService.SendEmail(member.Email, $"{member.OtherNames} {member.Surname}", "Membership Approval", html);
                return member;
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
            
        }

        [HttpPost("PutMemberOnHold")]
        public async Task<ActionResult<Member>> PutMemberOnHold(ApprovalModel model)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return Unauthorized();
                if (user.UserType != UserType.Admin)
                    return Unauthorized();
                MemberApproval memberApproval = new MemberApproval
                {
                    ApplicationUserId = user.Id,
                    ApprovalAction = ApprovalAction.OnHold,
                    Comments = model.Comments,
                    MemberId = model.MemberId,
                    MessageToMember = model.MessageToMember
                };
                return await memberService.PutMemberOnHold(memberApproval);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("RejectMember")]
        public async Task<ActionResult<Member>> RejectMember(ApprovalModel model)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return Unauthorized();
                if (user.UserType != UserType.Admin)
                    return Unauthorized();
                MemberApproval memberApproval = new MemberApproval
                {
                    ApplicationUserId = user.Id,
                    ApprovalAction = ApprovalAction.Rejected,
                    Comments = model.Comments,
                    MemberId = model.MemberId,
                    MessageToMember = model.MessageToMember
                };
                return await memberService.RejectMember(memberApproval);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

        }

    }
}
