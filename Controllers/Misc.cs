using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApi.Contracts
{
    [Route("[controller]")]
    [ApiController]
    public class Misc : ControllerBase
    {
        private readonly DataDbContext _context;

        public DateTime CurrentTime = DateTime.UtcNow;

        public Misc(DataDbContext context)
        {
            _context = context;
            

        }
        [HttpGet]
        public IActionResult GetMiscs([FromHeader] int customerId)
        {
            var miscs = _context.Misc.Where(r => r.CustomerId == customerId).ToList();
            return Ok(miscs);
        }

        [HttpPost]
        public IActionResult PostMisc([FromBody] MiscContract misc, [FromHeader] int customerId, int userId)
        {
            
            _context.Misc.Add(new MiscDb()
            {
                MiscType=misc.MiscType,
                Price = misc.Price,
                Description = misc.Description,
                DateCreated = CurrentTime,
                CustomerId = customerId,
                UserId= userId
            });

            var siteCash = UpdateCash(customerId,0, misc.Price);
            if (siteCash == null) {
                return NotFound();
            }
            if (siteCash < 0)
            {
                return BadRequest(new ErrorResponse(){Title="No Enough Cash At Site"});
            }

            _context.SaveChanges();
            return Ok("Misc Created Successfully");
        }

        [HttpPut("{id}")] 
        public IActionResult PutMisc(int id, [FromBody] MiscContract misc, [FromHeader] int customerId,int userId)
        {
            var foundMisc = _context.Misc.FirstOrDefault(i => i.Id == id && i.CustomerId == customerId);

            if (foundMisc == null)
            {
                return NotFound();
            }

            var siteCash = UpdateCash(customerId, foundMisc.Price, misc.Price);
            if (siteCash == null)
            {
                return NotFound();
            }
            if (siteCash < 0)
            {
                return BadRequest();
            }

            foundMisc.MiscType = misc.MiscType;
            foundMisc.Price = misc.Price;
            foundMisc.Description = misc.Description;
            foundMisc.DateCreated = CurrentTime;
            foundMisc.UserId = userId;

            _context.SaveChanges();
            return Ok("Misc Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteMisc(int id, [FromHeader] int customerId)
        {
            var misc = _context.Misc.FirstOrDefault(i => i.Id == id && i.CustomerId == customerId);

            if (misc == null)
            {
                return NotFound();
            }

            var siteCash = UpdateCash(customerId, misc.Price,0);
            if (siteCash == null)
            {
                return NotFound();
            }

            _context.Misc.Remove(misc);
            _context.SaveChanges();
            return Ok("Misc Deleted Successfully");
        }

        [HttpGet("cash")]
        public IActionResult GetMiscCash([FromHeader] int customerId)
        {
            var siteCash = _context.SiteCash.FirstOrDefault(r => r.CustomerId == customerId);
            if (siteCash == null)
            {
                return NotFound();
            }
            return Ok(siteCash);
        }

        [HttpPost("cash")]
        public IActionResult PostMiscCash([FromHeader] int customerId, [FromBody] SiteCashContract _cash)
        {
            var siteCash = _context.SiteCash.FirstOrDefault(r => r.CustomerId == customerId);
            if (siteCash == null)
            {
                
                _context.SiteCash.Add(new SiteCashDb() { Amount = _cash.Amount, CustomerId = customerId, LastModified = CurrentTime });
                _context.SaveChanges();
                return Ok("Misc Created Successfully");
            }
            return NotFound();
        }

        [HttpPut("cash")]
        public IActionResult PutMiscCash([FromHeader] int customerId,int userId, [FromBody] SiteCashContract _cash)
        {
            var siteCash = _context.SiteCash.FirstOrDefault(r => r.CustomerId == customerId);
            if (siteCash == null)
            {
                return NotFound(); 
            }

            siteCash.Amount += _cash.Amount;
            siteCash.LastModified = CurrentTime;

            _context.Misc.Add(new MiscDb()
            {
                MiscType = Enums.MiscType.Deposit,
                Price = _cash.Amount,
                DateCreated = CurrentTime,
                CustomerId = customerId,
                UserId = userId
            });
            _context.SaveChanges();
            return Ok("Misc Updated Successfully");
        }


        private int? UpdateCash(int customerId, int? prevPrice, int? currentPrice)
        {
            var siteCash = _context.SiteCash.FirstOrDefault(r => r.CustomerId == customerId);

            if (siteCash == null)
            {
                //return NotFound();
                return null;
            }
            var difference =  currentPrice-prevPrice;
            if (difference > siteCash.Amount) {
                return -1;
            }
            siteCash.Amount -= difference;

            return siteCash.Amount;

        }
    }
}
