using System;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NgSight.API;
using NgSight.API.Models;

namespace NgSight.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class OrderController : Controller
    {
        private readonly NgSightDBContext _ctx;

        public OrderController(NgSightDBContext ctx)
        {
            _ctx = ctx;
        }
        //Get api/order/pagenumber/pagesize
        [HttpGet("{pageIndex:int}/{pageSize:int}")]
        public IActionResult Get(int pageIndex, int pageSize)
        {
            var data = _ctx.orders.Include(o => o.Customer)
                                  .OrderByDescending(c => c.Placed);
            var page = new PaginatedResponse<Order>(data, pageIndex, pageSize);
            var totalCount = data.Count();
            var totalPages = System.Math.Ceiling((double) totalCount / pageSize);

            var response = new 
            {
                Page = page,
                TotalPages = totalPages 
            };

            return Ok(response);
        }
        
        [HttpGet("ByState")]
        public IActionResult ByState() 
        {
            var orders = _ctx.orders.Include(o => o.Customer).ToList();
            var groupedResult = orders.GroupBy(o => o.Customer.State)
                                .ToList()
                                .Select(grp => new {
                                    State = grp.Key,
                                    Total = grp.Sum(x => x.OrderTotal)
                                }).OrderByDescending(res => res.Total)
                                .ToList();
            return Ok(groupedResult);
        }

        [HttpGet("ByCustomer/{n}")]
        public IActionResult ByCustomer(int n) 
        {
            var orders = _ctx.orders.Include(o => o.Customer).ToList();
            var groupedResult = orders.GroupBy(o => o.Customer.Id).ToList()
                                .Select(grp => new {
                                    Name =_ctx.customers.Find(grp.Key).Name,
                                    Total = grp.Sum(x => x.OrderTotal)
                                }).OrderByDescending(res => res.Total)
                                .Take(n)
                                .ToList();

            return Ok(groupedResult);
        }
        [HttpGet("GetOrder/{id}", Name="GetOrder")]
        public IActionResult GetOrder(int id) 
        {
            var order =  _ctx.orders.Include(o => o.Customer).ToList()
                                    .First(o => o.Id == id);
            return Ok(order);

        }
    }
}