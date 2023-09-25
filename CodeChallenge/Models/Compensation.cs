using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeChallenge.Models
{
    public class Compensation
    {
        public string CompensationId { get; set; }//Not explicitly mentioned but we want a primary key right?
        [ForeignKey("EmployeeId")]
        public Employee employee { get; set; }
        public int salary { get; set; }
        public DateTime effectiveDate { get; set; }
    }
}
