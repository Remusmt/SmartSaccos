using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartSaccos.ApplicationCore.DomainServices;
using SmartSaccos.ApplicationCore.Models;
using SmartSaccos.ApplicationCore.Services;
using SmartSaccos.Domains.Entities;
using SmartSaccos.Domains.Enums;
using SmartSaccos.MemberPortal.Helpers;
using System;
using System.Threading.Tasks;

namespace SmartSaccos.MemberPortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MembersController : ControllerBase
    {
        private readonly MemberService memberService;
        private readonly MessageService messageService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostEnvironment;
        public MembersController(
            MemberService membaService,
            MessageService msgService,
            IWebHostEnvironment webHost,
            UserManager<ApplicationUser> usermanager)
        {
            memberService = membaService;
            messageService = msgService;
            hostEnvironment = webHost;
            userManager = usermanager;
        }

        [HttpGet("GetMember/{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            return await memberService.GetMemberAsync(id);
        }

        [HttpGet("GetDetailedMember/{id}")]
        public async Task<ActionResult<Member>> GetDetailedMember(int id)
        {
            return await memberService.GetDetailedMemberAsync(id);
        }

        [HttpPost("KycDetails")]
        public async Task<ActionResult<Member>> KycPersonalDetails(MemberModel model)
        {
            try
            {
                return await memberService.KycDetails(model, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        [HttpPost("KycDocs")]
        public async Task<ActionResult> KycDocs()
        {
            if (Request.Form == null)
                return BadRequest(new { new Exception("No file attachments found").Message });
            if (Request.Form.Files.Count == 0)
                return BadRequest(new { new Exception("No file attachments found").Message });
            try
            {
                // Use authorized user to get member
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return Unauthorized();
                //get member if user exits
                Member member = await memberService.GetMemberByUserIdAsync(user.Id);
                if (member == null)
                    return NotFound();
                //get attachment type
                AttachmentType attachmentType = (AttachmentType)Enum
                    .Parse(typeof(AttachmentType), Request.Form["kycDocType"]);

                //delete previous attachment if any
                switch (attachmentType)
                {
                    case AttachmentType.IdFront:
                        if (member.IdFrontAttachmentId > 0)
                            DeleteFile(member.IdFrontAttachmentId);
                        break;
                    case AttachmentType.IdBack:
                        if (member.IdBackAttachmentId > 0)
                            DeleteFile(member.IdFrontAttachmentId);
                        break;
                    case AttachmentType.Avator:
                        if (member.PassportPhotoId > 0)
                            DeleteFile(member.PassportPhotoId);
                        break;
                }

                //save file
                IFormFile formFile = Request.Form.Files[0];
                Attachment attachment = formFile
                    .CreateAttachment(member, hostEnvironment.WebRootPath);

                //copy file to server
                formFile.SaveFile(attachment.FullPath);

                return Ok(await memberService.SaveAttachment(member, attachment, attachmentType));
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        private async void DeleteFile(int id)
        {
            MemberAttachment memberAttachment = await memberService
                            .GetMemberAttachmentAsync(id);
            if (memberAttachment != null)
            {
                if (memberAttachment.Attachment != null)
                {
                    memberAttachment.Attachment.FullPath.DeleteFile();
                    await memberService.DeleteAttachmentAsync(memberAttachment);
                }
            }
        }
    }
}
