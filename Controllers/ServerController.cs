using System;
using System.Linq;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSight.API.Models;
namespace NgSight.API.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]    
    public class ServerController : Controller
    {
        private readonly NgSightDBContext _ctx;

        public ServerController(NgSightDBContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var response = _ctx.servers.OrderBy(s=> s.Id).ToList();
            return Ok(response);
        }

        [HttpGet("{Id}", Name="GetServer")]
        public IActionResult Get(int id)
        {
            var response = _ctx.servers.Where(s=> s.Id == id).ToList();
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Server server)
        {
            if(server != null)
            {
                return BadRequest();
            }
            else
            {
                _ctx.servers.Add(server);
                _ctx.SaveChanges();
                return CreatedAtRoute("GetServer", new { id = server.Id}, server);
            }
        }

        [HttpPut("{Id}")]
        public IActionResult Put(int id, [FromBody] ServerMessage msg)
        {
            var server = _ctx.servers.Find(id);
            if(server==null)
            {
                NotFound();
            }
            else
            {
                if(msg.Payload == "activate")
                {
                    server.IsOnline = true;
                }

                if(msg.Payload == "deactivate")
                {
                    server.IsOnline = false;
                }
                _ctx.SaveChanges();
            }
            return new NoContentResult();
        }
    }
}