using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionApi.Contracts;
using ConstructionApi.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConstructionApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Admin : ControllerBase
    {
        private readonly DataDbContext _context;
        private readonly DateTime currentTime;

        public Admin(DataDbContext context)
        {
            _context = context;
            currentTime = DateTime.UtcNow;

        }

        private MyRequest GetHeaders()
        {
            return new MyRequest() { UserId = int.Parse(Request.Headers["userId"]), CustomerId = int.Parse(Request.Headers["customerId"]) };

        }

        [HttpGet("testing")]
        public IActionResult Testing([FromHeader] int userId)
        {
            var a = GetHeaders();
            Console.WriteLine($"Local Now: {a.CustomerId}");
            var now = DateTime.Now;
            var utcNow = DateTime.UtcNow;
            Console.WriteLine($"Local Now: {now}");
            Console.WriteLine($"UTC Now: {utcNow}");
            Console.WriteLine($"userId: {userId}");

            return Ok();
        }

        [HttpDelete("testing")]
        public IActionResult DeleteTesting([FromHeader] int customerId, [FromHeader] int userId, [FromHeader] int bb)
        { 
            var aa = _context.Inventory.Where(i => i.CustomerId == 2).ToList();
            aa.ForEach(q =>
            {
                _context.Remove(q);
            });
            //_context.SaveChanges();

            return Ok(aa);
        }

    }
}
