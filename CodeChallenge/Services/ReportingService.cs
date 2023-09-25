using CodeChallenge.Models;
using CodeChallenge.Repositories;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
    public class ReportingService : IReportingService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingService> _logger;

        public ReportingService(ILogger<ReportingService> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
        }
        public ReportingStructure GetById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Employee emp = _employeeRepository.GetById(id);
                if (emp != null)
                {
                    return new ReportingStructure(_employeeRepository.GetById(id));
                }
            }
            return null;
        }

    }
}
