using Microsoft;
using Microsoft.Extensions;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NgSight.API.Models;

namespace NgSight.API.Models
{
    public class NgSightDBContext: DbContext 
    {
        public NgSightDBContext(DbContextOptions<NgSightDBContext> options) 
        : base(options)
        {

        }

        public DbSet<Customer> customers {get; set;}
        public DbSet<Order> orders { get; set; }
        public DbSet<Server> servers { get; set; }
    }
}