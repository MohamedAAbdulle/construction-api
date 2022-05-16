using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Tools(DataDbContext context)
        {
            _context = context;

        }
        [HttpGet]
        public IActionResult GetTools( [FromHeader] int customerId)
        {
            var tools = _context.Tool.Where(r=> r.CustomerId==customerId).ToList();
            return Ok(tools);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ToolDb tool, [FromHeader] int customerId)
        {
            Console.WriteLine(tool);
            _context.Tool.Add(new ToolDb()
            {
                Name = tool.Name,
                Quantity = tool.Quantity,
                InUse = tool.InUse,
                CustomerId= customerId
            });

            _context.SaveChanges();
            return Ok("Tool Created Successfully");
        }

        [HttpPut]
        public IActionResult Put([FromBody] ToolDb tool)
        {
            var foundTool = _context.Tool.FirstOrDefault(i => i.Id == tool.Id);

            if (foundTool == null)
            {
                return NotFound();
            }

            foundTool.Name = tool.Name;
            foundTool.Quantity = tool.Quantity;
            foundTool.InUse = tool.InUse;

            _context.SaveChanges();
            return Ok("Tool Updated Successfully");
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
        public IActionResult PostInUseTool([FromBody] InUseToolDb tool, [FromHeader] int customerId)
        {
            _context.InUseTool.Add(new InUseToolDb()
            {
                WorkerId = tool.WorkerId,
                ToolId = tool.ToolId,
                DateAssigned = tool.DateAssigned,
                CustomerId = customerId
            });

            var foundTool = _context.Tool.FirstOrDefault(i => i.Id == tool.ToolId);

            if (foundTool == null)
            {
                return NotFound();
            }

            foundTool.InUse++;

            _context.SaveChanges();
            return Ok("Tool Assigned Successfully");
        }


        [HttpDelete("inuse")]
        public IActionResult DeleteInUseTool(int toolId, int workerId)
        {
            var foundInuseTool = _context.InUseTool.FirstOrDefault(i => i.ToolId == toolId&&i.WorkerId==workerId);
            var foundTool = _context.Tool.FirstOrDefault(i => i.Id == toolId);


            if (foundInuseTool == null|| foundTool == null)
            {
                return NotFound();
            }

            foundTool.InUse--;

            _context.InUseTool.Remove(foundInuseTool);
            _context.SaveChanges();
            return Ok("Tool Returned Successfully");
        }



    }
}
