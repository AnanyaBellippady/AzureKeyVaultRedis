using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectSimulation.Model
{
    [Table("EmployeeDepartmentMap")]
    public class EmployeeDepartmentMap
    {
        [Key]
        public int id { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
    }
}
