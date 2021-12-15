using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectSimulation.Model
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string Gender { get; set; }
        public float Salary { get; set; }
        public DateTime StartDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
