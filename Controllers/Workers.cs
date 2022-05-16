using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConstructionApi.Data;
using Microsoft.AspNetCore.Mvc;


namespace ConstructionApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Workers : ControllerBase
    {
        private readonly DataDbContext _context;

        public Workers(DataDbContext context)
        {
            _context = context;

        }
        [HttpGet]
        public IActionResult GetWorker([FromHeader] int customerId)
        {
            var workers = _context.Worker.Where(r => r.CustomerId == customerId).ToList();
            return Ok(workers);
        }

        [HttpPost]
        public IActionResult Post([FromBody] WorkerDb wkr, [FromHeader] int customerId)
        {
            _context.Worker.Add(new WorkerDb()
            {
                Id = 0,
                Name = wkr.Name,
                Rate = wkr.Rate,
                WorkerType= wkr.WorkerType,
                IdNumber= wkr.IdNumber,
                CustomerId = customerId
            });

            _context.SaveChanges();
            return Ok("Worker Created Successfully"); 
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] WorkerDb wkr, int id)
        {
            var foundWkr = _context.Worker.FirstOrDefault(i => i.Id == id);

            if (foundWkr == null)
            {
                return NotFound();
            }

            foundWkr.Name = wkr.Name;
            foundWkr.Rate = wkr.Rate;
            foundWkr.WorkerType = wkr.WorkerType;
            foundWkr.IdNumber = wkr.IdNumber;
            

            _context.SaveChanges();
            return Ok("Worker Updated Successfully");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var foundWkr = _context.Worker.FirstOrDefault(i => i.Id == id);

            if (foundWkr == null)
            {
                return NotFound();
            }
            _context.Worker.Remove(foundWkr);
            _context.SaveChanges();
            return Ok("Worker Deleted Successfully");
        }

        //test
        [HttpGet("ActiveWorkers/all")]
        public IActionResult GetActiveWorkerAll([FromQuery] DateTime weekof)
        {
            var workers = _context.ActiveWorker.ToList();
            return Ok(workers);
        }

        [HttpGet("ActiveWorkers")]
        public IActionResult GetActiveWorker([FromQuery] DateTime weekof, [FromHeader] int customerId)
        {
            Console.WriteLine(weekof);
            var workers = _context.ActiveWorker.Where(w => w.Date == weekof && w.CustomerId == customerId).ToList();
            return Ok(workers);
        }

        [HttpPost("ActiveWorkers")]
        public IActionResult PostActiveWorker([FromBody] ActiveWorkerDb wkr, [FromHeader] int customerId)
        {
            var ss = _context.ActiveWorker.FirstOrDefault(w => w.WorkerId == wkr.WorkerId&&w.Date==wkr.Date);
            if (ss == null)
            {
                _context.ActiveWorker.Add(new ActiveWorkerDb()
                {
                    Chart = wkr.Chart,
                    WorkerId = wkr.WorkerId,
                    Date = wkr.Date,
                    CustomerId = customerId
                }); ;

                _context.SaveChanges();
                return Ok("Worker Activated Successfully");
            }
            else {
                return Ok("Worker Already Active");
            }
           
        }

        [HttpDelete("ActiveWorkers/{id}")]
        public IActionResult DeleteActiveWorker(int id)
        {
            var foundWkr = _context.ActiveWorker.FirstOrDefault(i => i.Id == id);

            if (foundWkr == null)
            {
                return NotFound();
            }
            _context.ActiveWorker.Remove(foundWkr);
            _context.SaveChanges();
            return Ok("Worker Inactivated Successfully");
        }

        [HttpPut("activeWorkers/chart/{id}")]
        public IActionResult UpdateChart(int id, [FromQuery] string chart)
        {
            var foundInv = _context.ActiveWorker.FirstOrDefault(i => i.Id == id);

            if (foundInv == null)
            {
                return NotFound();
            }

            foundInv.Chart = chart;

            _context.SaveChanges();
            return Ok("Chart Updated Successfully");
        }

        [HttpGet("WorkerTypes")]
        public IActionResult GetWorkerTypes([FromHeader] int customerId)
        {
            var workers = _context.WorkerType.Where(r => r.CustomerId == customerId).ToList();
            return Ok(workers);
        }

        [HttpPost("WorkerTypes")]
        public IActionResult Post([FromBody] WorkerTypeDb workerType, [FromHeader] int customerId)
        {
            _context.WorkerType.Add(new WorkerTypeDb()
            {
                TypeName = workerType.TypeName,
                Rate = workerType.Rate,
                CustomerId = customerId
            });

            _context.SaveChanges();
            return Ok("Worker Type Created Successfully");
        }


        [HttpPut("WorkerTypes")]
        public IActionResult Put([FromBody] WorkerTypeDb workerType)
        {
            var foundType = _context.WorkerType.FirstOrDefault(i => i.TypeValue == workerType.TypeValue);

            if (foundType == null)
            {
                return NotFound();
            }

            foundType.TypeName = workerType.TypeName;
            foundType.Rate = workerType.Rate;

            _context.SaveChanges();
            return Ok("Worker Type Updated Successfully");
        }
    }
}
