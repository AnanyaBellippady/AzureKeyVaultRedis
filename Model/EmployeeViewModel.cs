using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectSimulation.Model
{
    public class EmployeeViewModel
    {
        public int id { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeImage { get; set; }
        public string Gender { get; set; }
        public float Salary { get; set; }
        public DateTime StartDate { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<string> DepartmentName { get; set; }
        public List<int> DepartmentId { get; set; }
    }
}
