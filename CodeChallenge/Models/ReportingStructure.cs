using CodeChallenge.Data;
using CodeChallenge.Repositories;
using System.Linq;

namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public Employee employee { get; set; }
        public int numberOfReports {
            get { return getReports(employee); }
        }
        public ReportingStructure(Employee emp)
        {
            employee = emp;
        }
        private int getReports(Employee emp)
        {
            int reports = 0;
            //Ah of course, DirectReports can be null. Not initialized unless it has vals
            if(emp.DirectReports != null)
            {
                foreach (Employee report in emp.DirectReports)
                {
                    reports += getReports(report);//Recursively gather those directly under me
                    reports++;//Add 1 for this direct report
                }
            }
            return reports;
        }
    }
}
