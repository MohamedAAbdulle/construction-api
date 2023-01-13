using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionApi.Contracts;
using ConstructionApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class Tools : ControllerBase
    {
        private readonly DataDbContext _context;
        private readonly DateTime currentTime;

        public Tools(DataDbContext context)
        {
            _context = context;
            currentTime = DateTime.UtcNow;

        }

        

        [HttpGet]
        public IActionResult GetTools( [FromHeader] int customerId)
        {
            var tools = _context.Tool.Where(r=> r.CustomerId==customerId).ToList();
            return Ok(tools);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ToolObj tool, [FromHeader] int customerId)
        {
            Console.WriteLine(tool);
            _context.Tool.Add(new ToolDb()
            {
                Name = tool.Name,
                Quantity = tool.Quantity,
                InUse = 0,
                CustomerId= customerId,
                LastModified= currentTime
               
            });

            _context.SaveChanges();
            return Ok("Tool Created Successfully");
        }

        [HttpPut] //change, include id here
        public IActionResult Put([FromBody] ToolObj tool)
        {
            var foundTool = _context.Tool.FirstOrDefault(i => i.Id == tool.Id);

            if (foundTool == null)
            {
                return NotFound();
            }

            if (tool.Quantity < foundTool.InUse) {
                return BadRequest("Quantity can't be Less than in use!");
            }

            foundTool.Name = tool.Name;
            foundTool.Quantity = tool.Quantity;
            foundTool.LastModified = currentTime;

            _context.SaveChanges();
            return Ok("Tool Updated Successfully");
        }

        [HttpPut("return-all")] //change, include id here
        public IActionResult ReturnAll([FromBody] ToolObj tool)
        {
            var foundTool = _context.Tool.FirstOrDefault(i => i.Id == tool.Id);

            if (foundTool == null)
            {
                return NotFound();
            }
            var inUses= _context.InUseTool.Where(r => r.ToolId == tool.Id).ToList();
            
            int length= inUses.Count;
            if (length>0) {
                inUses.ForEach(t => _context.InUseTool.Remove(t) );
            }

            var initialInuse = foundTool.InUse;
            if (initialInuse > 0) {
                foundTool.InUse = 0;
                foundTool.LastModified = currentTime;
            }

            if (length > 0 || initialInuse > 0) { 
                _context.SaveChanges();
            }
           
            return Ok($"Successfully return {length} assignment");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var tool = _context.Tool.FirstOrDefault(i => i.Id == id);

            if (tool == null)
            {
                return NotFound();
            }

            _context.Tool.Remove(tool);
            _context.SaveChanges();
            return Ok("Tool Deleted Successfully");
        }

        [HttpGet("inuse")]
        public IActionResult GetInUseTools([FromHeader] int customerId) {

            var tools = _context.InUseTool.Where(r => r.CustomerId == customerId).ToList();
            return Ok(tools);
        }

        [HttpPost("inuse")]
        public IActionResult PostInUseTool([FromBody] InUseToolObj tool, [FromHeader] int customerId)
        {
            _context.InUseTool.Add(new InUseToolDb()
            {
                WorkerId = tool.WorkerId,
                ToolId = tool.ToolId,
                Amount=tool.Amount,
                DateAssigned = currentTime,
                CustomerId = customerId
            });

            var foundTool = _context.Tool.FirstOrDefault(i => i.Id == tool.ToolId);

            if (foundTool == null)
            {
                return NotFound();
            }

            int newInuse= foundTool.InUse + (int)tool.Amount;
            if (newInuse>foundTool.Quantity) {
                return BadRequest("Can't Assign More than Instore");
            }

            foundTool.InUse=newInuse;

            _context.SaveChanges();
            return Ok("Tool Assigned Successfully");
        }


        [HttpDelete("inuse")]
        public IActionResult DeleteInUseTool(int toolId, int workerId, DateTime dateAssigned) //get the whole inuseTool
        {            
            var foundInuseTool = _context.InUseTool.FirstOrDefault(i => i.ToolId == toolId&&i.WorkerId==workerId&&i.DateAssigned== dateAssigned);
            var foundTool = _context.Tool.FirstOrDefault(i => i.Id == toolId);


            if (foundInuseTool == null)
            {
                return NotFound();
            }

            if ( foundTool != null)
            {
                foundTool.InUse -= (int)foundInuseTool.Amount;           
            }

            _context.InUseTool.Remove(foundInuseTool);
            _context.SaveChanges();
            return Ok("Tool Returned Successfully");
        }



    }
}
