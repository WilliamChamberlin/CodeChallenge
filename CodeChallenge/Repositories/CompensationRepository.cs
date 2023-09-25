using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository :ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext context)
        {
            //Init our context
            _logger = logger;
            _employeeContext = context;
        }

        public Compensation GetById(string id)
        {
            //Find our compensation by Employee ID
            return _employeeContext.Compensation.SingleOrDefault(i => i.employee.EmployeeId == id);
        }

        public Compensation Add(Compensation compensation)
        {
            //Add new object to DB
            compensation.CompensationId = Guid.NewGuid().ToString();
            //Since this is a FK, we need to get the FK object
            compensation.employee = _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == compensation.employee.EmployeeId);
            _employeeContext.Compensation.Add(compensation);
            return compensation;
        }
        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
