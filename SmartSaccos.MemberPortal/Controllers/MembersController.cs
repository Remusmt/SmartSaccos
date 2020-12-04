using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartSaccos.ApplicationCore.DomainServices;
using SmartSaccos.ApplicationCore.Models;
using SmartSaccos.ApplicationCore.Services;
using SmartSaccos.Domains.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public MembersController(
            MemberService membaService,
            MessageService msgService)
        {
            memberService = membaService;
            messageService = msgService;
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
    }
}
