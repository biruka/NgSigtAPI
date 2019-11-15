using System;
using System.Linq;
using System.Collections.Generic;
using  NgSight.API.Models;

namespace NgSight.API
{
    public class DataSeed
    {
        private readonly NgSightDBContext _ctx;

        public DataSeed(NgSightDBContext ctx) {
            _ctx = ctx;
        }

        public void SeedData(int nCusomers, int nOrders) 
        {
            if(!_ctx.customers.Any())
            {
                SeedCustomers(nCusomers);
                _ctx.SaveChanges();
            }

            if(!_ctx.orders.Any())
            {
                SeedOrders(nOrders);
                _ctx.SaveChanges();
            }
            
            if(!_ctx.servers.Any())
            {
                SeedServers();
                _ctx.SaveChanges();

            }
        }
        
        private List<Customer> BuildCustomerList(int nCustomers)
        {
            var customers = new List<Customer>();
            var names = new List<string>();
            for(var i=1; i<=nCustomers; i++)
            {   
                var name = Helpers.MakeUniqueCustomerName(names);
                names.Add(name);
                customers.Add(new Customer {
                    Id= i,
                    Name = name,
                    Email = Helpers.MakeCustomerEmail(name),
                    State = Helpers.GetRandomState()
                });
            }    
            return customers; 
        }

        private void SeedCustomers(int n)
        {
            List<Customer> customers = BuildCustomerList(n);
            foreach(var customer in customers)
            {
                _ctx.customers.Add(customer);
            }
        }

        private List<Order> BuildOrderList(int nOrders)
        {
            var orders = new List<Order>();
            var rand = new Random();
            for(int index = 1; index <= nOrders; index++)
            {
                var randCustomerId =  rand.Next(1, _ctx.customers.Count()); 
                var placed = Helpers.GetRandomOrderPlaced();
                var completed = Helpers.GetRandomOrderCompleted(placed);
                var customers = _ctx.customers.ToList();
                orders.Add(new Order
                {
                  Id = index,
                  Customer = customers.First(c => c.Id == randCustomerId),
                  OrderTotal = Helpers.GetRandomOrderTotal(), 
                  Placed = placed,
                  Completed = completed
                });
            }
            return orders;
        }
        private void SeedOrders(int n)
        {
            List<Order> orders = BuildOrderList(n);
            foreach(var order in orders)
            {
                _ctx.orders.Add(order);
            }
        }

        internal List<Server> BuildServerList() 
        {
           return new List<Server>() 
           {
               new Server {
                   Id=1,
                   Name = "Dev-Web",
                   IsOnline = true
               },
               new Server {
                   Id=2,
                   Name = "Dev-Mail",
                   IsOnline = false
               },
               new Server {
                   Id=3,
                   Name = "Dev-Services",
                   IsOnline = true
               },
               new Server {
                   Id=4,
                   Name = "QA-Web",
                   IsOnline = true
               },
               new Server {
                   Id=5,
                   Name = "QA-Mail",
                   IsOnline = false
               },
               new Server {
                   Id=6,
                   Name = "QA-Services",
                   IsOnline = true
               },
               new Server {
                   Id=7,
                   Name = "Prod-Web",
                   IsOnline = true
               },
               new Server {
                   Id=8,
                   Name = "Prod-Mail",
                   IsOnline = true
               },
               new Server {
                   Id=9,
                   Name = "Prod-Services",
                   IsOnline = true
               }
           };
        }

        private void SeedServers()
        {
            List<Server> servers = BuildServerList();
            foreach(var server in servers)
            {
                _ctx.servers.Add(server);
            }
        }
    }
}