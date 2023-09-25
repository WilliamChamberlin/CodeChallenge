using CodeChallenge.Models;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Compensation GetById(string id);
        Compensation Create(Compensation compensation);
    }
}
