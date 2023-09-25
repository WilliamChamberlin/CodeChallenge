using CodeChallenge.Models;

namespace CodeChallenge.Services
{
    public interface IReportingService
    {
        ReportingStructure GetById(string id);
    }
}
