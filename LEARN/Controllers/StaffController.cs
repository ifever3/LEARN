using LEARN.data;
using LEARN.model;
using LEARN.redis;
using MassTransit;
using MassTransit.Transports;
using Mediator.Net;
using Mediator.Net.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Data;
using message;

namespace LEARN.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StaffController : Controller
    {
        private readonly redisop _redisop;
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        public StaffController(IMediator mediator, redisop redisop, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _redisop = redisop;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("redis")]
        public string Get()
        {        
            var k = _redisop.GetValue("key");
            return k;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] createStaffcommand command) =>
            Ok(await _mediator.SendAsync<createStaffcommand, createStaffresponse>(command));

        [HttpGet("get")]
        public async Task<IActionResult> GetAsync([FromQuery] getStaffrequest request) =>
            Ok(await _mediator.RequestAsync<getStaffrequest, getStaffresponse<Staff>>(request));

        [HttpPatch("update")]       
        public async Task<IActionResult> UpdateAsync([FromQuery] Guid id, [FromBody] updateStaffparam command,
            CancellationToken cancellationToken = default)
        {
            var response = await _mediator.SendAsync<updateStaffcommand, updateStaffresponse>(new updateStaffcommand
            {
                Id = id,
                Name = command.Name,
                Major = command.Major
            }, cancellationToken);
              return Ok(response);
         }   
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] Guid id, CancellationToken cancellationToken = default)
        {
            await _mediator.SendAsync(new deleteStaffcommand { Id = id },cancellationToken);
            return NoContent();
        }

        [HttpPost("rabbitmq")]
        public async Task<ActionResult> Post()
        { 
            await _publishEndpoint.Publish<sendedevent>(new sendedevent("tom","math"));
            return Ok();
        }

        //[HttpPost]
        //public IEnumerable<Staff> Post([FromBody] Staff staff)
        //{

        //    _context.AddAsync(staff);
        //    _context.SaveChanges();
        //    return new List<Staff>();
        //}

        // [HttpGet]
        // public IActionResult Get(string major)
        // {
        //     Staff staff = _context.Staff.FirstOrDefault(s => s.major == major); 
        //         return Ok(staff);
        // }

        // [HttpDelete("{id}")]
        // public IActionResult Delete(Guid id)
        // {
        //     Staff staff = _context.Staff.FirstOrDefault(s => s.id == id);
        //     if (staff == null)
        //     {
        //         return NotFound(); // 如果未找到要删除的Staff对象，返回404 Not Found
        //     }

        //     _context.Staff.Remove(staff);
        //     _context.SaveChanges();
        //     return NoContent();
        // }

        // [HttpPut("{id}")]
        // public IActionResult Put(Guid id, [FromBody] Staff staff)
        // {
        //     Staff existingStaff = _context.Staff.FirstOrDefault(s => s.id == id);
        //     if (existingStaff == null)
        //     {
        //         return NotFound(); // 如果未找到要更新的Staff对象，返回404 Not Found
        //     }

        //     existingStaff.name = staff.name;
        //     existingStaff.major = staff.major;

        //     _context.SaveChanges();
        //     return Ok(existingStaff);
        // }
    }
}
