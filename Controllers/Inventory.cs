using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using ConstructionApi.Contracts;
using ConstructionApi.Data;
using ConstructionApi.Enums;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConstructionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Inventory : ControllerBase
    {
        private readonly DataDbContext _context;

        public Inventory(DataDbContext context)
        {
            _context = context;

        }

        //test
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var invList = new List<int> { 3,4,5,6};
            return Ok(invList);
        }
        

        //get inventory list
        [HttpGet]
        public IActionResult GetInventory([FromHeader] int customerId)
        {
            var aa = _context.Inventory.Where(r => r.CustomerId == customerId).ToList();
            return Ok(aa);
        }

        [HttpPost]
        public IActionResult Post([FromBody] InventoryObj inv, [FromHeader] int customerId)
        {
            Console.WriteLine(inv);
            _context.Inventory.Add(new InventoryDb() { 
                Id=0,
                Name=inv.Name,
                Unit= inv.Unit,
                Quantity = inv.Quantity,
                Threshold= inv.Threshold,
                ModifiedDate= inv.ModifiedDate,
                CustomerId = customerId
            });

            _context.SaveChanges();
            return Ok("Inventory Created Successfully");
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] InventoryObj inv)
        {
            var foundInv = _context.Inventory.FirstOrDefault(i => i.Id == id);

            if (foundInv == null)
            {
                return NotFound();
            }

            foundInv.Name = inv.Name;
            foundInv.Unit = inv.Unit;
            foundInv.Quantity = inv.Quantity;
            foundInv.Threshold = inv.Threshold;
            foundInv.ModifiedDate = inv.ModifiedDate;

            _context.SaveChanges();
            return Ok("Inventory Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var inv=_context.Inventory.FirstOrDefault(i => i.Id == id);

            if (inv==null) { 
                return NotFound();
            }
            _context.Inventory.Remove(inv);
            _context.SaveChanges();
            return Ok("Inventory Deleted Successfully");
        }

        [HttpPut("takeout/{id}")]
        public IActionResult Takeout(int id,[FromBody] Takeout inv)
        {
            var foundInv = _context.Inventory.FirstOrDefault(i => i.Id == id);

            if (foundInv == null)
            {
                return NotFound();
            }

            var remaining = foundInv.Quantity - inv.Quantity;
            if (remaining < 0) remaining = 0;

            foundInv.Quantity = remaining;
            foundInv.ModifiedDate = inv.ModifiedDate;

            //record it to inventory history
            _context.InventoryHistory.Add( new InventoryHistoryDb() { 
                InvId= id,
                Quantity=inv.Quantity,
                Type= (InvHistType)1,
                DateDone=inv.ModifiedDate
            });;
            _context.SaveChanges();
            return Ok("Inventory Updated Successfully");
        }

        [HttpPut("addmore/{id}")]
        public IActionResult AddMore(int id, [FromBody] AddMore inv)
        {
            var foundInv = _context.Inventory.FirstOrDefault(i => i.Id == id);

            if (foundInv == null)
            {
                return NotFound();
            }

            var remaining = foundInv.Quantity + inv.Quantity;

            foundInv.Quantity = remaining;
            foundInv.ModifiedDate = inv.ModifiedDate;

            //record it to inventory history
            _context.InventoryHistory.Add(new InventoryHistoryDb()
            {
                InvId = id,
                Quantity = inv.Quantity,
                Type = (InvHistType) 2,
                DateDone = inv.ModifiedDate
            });
            _context.SaveChanges();
            return Ok("Inventory Updated Successfully");
        }


        //Inventory History
        [HttpGet("invhistory/{id}")]
        public IActionResult GetInvHistory(int id)
        {
            var aa = _context.InventoryHistory.Where(x => x.InvId == id).ToList();
            return Ok(aa);
        }



    }
}
