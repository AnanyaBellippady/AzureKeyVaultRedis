using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewProjectSimulation.Model;

namespace NewProjectSimulation.Data
{
    public class NewProjectSimulationContext : DbContext
    {
        public NewProjectSimulationContext (DbContextOptions<NewProjectSimulationContext> options)
            : base(options)
        {
        }

        public DbSet<NewProjectSimulation.Model.Employee> Employee { get; set; }
        public DbSet<NewProjectSimulation.Model.Department> Department { get; set; }
        public DbSet<NewProjectSimulation.Model.EmployeeDepartmentMap> EmployeeDepartmentMap { get; set; }


    }
}
