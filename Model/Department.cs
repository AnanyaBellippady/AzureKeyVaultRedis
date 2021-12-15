using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectSimulation.Model
{
    [Table("Department")]
    public class Department
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public DateTime Created_at { get; set; }
        public DateTime Modified_at { get; set; }
    }
}
