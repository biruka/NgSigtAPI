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
    public class CustomerController : Controller
    {
        private readonly NgSightDBContext _ctx;

        public CustomerController(NgSightDBContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            var data = _ctx.customers.OrderBy(c=>c.Id);

            return Ok(data);
        }

        [HttpGet("{Id}", Name="GetCustomer")]
        public IActionResult Get(int Id) 
        {
            var data = _ctx.customers.Where(c=>c.Id == Id);

            return Ok(data);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Customer customer)
        {
            if(customer == null)
            {
                return BadRequest();
            }
            else
            {
                _ctx.customers.Add(customer);
                _ctx.SaveChanges();

                return CreatedAtRoute("GetCustomer", new {id=customer.Id}, customer);
            }
        }
    }
}