using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using Microsoft.AspNetCore.Http;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService, IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }

        [HttpGet("{id}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationById(string id)
        {
            _logger.LogDebug($"Received compensation get request for '{id}'");
            var compensation = _compensationService.GetById(id);

            if(compensation == null )
            {
                return NotFound();
            }
            return Ok(compensation);
        }
        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.employee.FirstName} {compensation.employee.LastName}'");

            //Verify this employee actually exists
            var employee = _employeeService.GetById(compensation.employee.EmployeeId);
            if (employee == null)
            {
                //Alernatively, we could just send along a create request but I don't personally like that method.
                return BadRequest("Employee not found");
            }
            _compensationService.Create(compensation);

            return CreatedAtRoute("getCompensationByEmployeeId", new { id = compensation.employee.EmployeeId }, compensation);
        }
    }
}
