using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using NewProjectSimulation.Data;
using NewProjectSimulation.Model;
using Newtonsoft.Json;

namespace NewProjectSimulation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly NewProjectSimulationContext _context;
        private readonly IDistributedCache _distributedCache;
        private static string azureCacheKey = "Key_1";
        //commenting for git 
        public EmployeesController(NewProjectSimulationContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetEmployees()
        {
            try
            {
                //_logger.LogError("Logerror in get employees");
                //_logger.LogInformation("LogInfo in get employees");
                //_logger.LogDebug("Logdebug in get employees");
                //_logger.LogTrace("Logtrace in get employees");
                List<EmployeeViewModel> empView = new List<EmployeeViewModel>();
                string serialized_EmployeeList;
                List<Employee> emp = null;
                var resultFromCache = await _distributedCache.GetAsync(key: azureCacheKey);
                if (resultFromCache != null)
                {
                    serialized_EmployeeList = Encoding.UTF8.GetString(resultFromCache);
                    IEnumerable<EmployeeViewModel> employeeList = JsonConvert.DeserializeObject<IEnumerable<EmployeeViewModel>>(serialized_EmployeeList);
                    foreach (EmployeeViewModel eachemp in employeeList)
                    {
                        //getting department names from each record
                        //get the list of dept ids with emp id
                        List<int> deptListid = await _context.EmployeeDepartmentMap.Where(w => w.EmployeeId == eachemp.id).Select(s => s.DepartmentId).ToListAsync();

                        List<string> departmentList = new List<string>();
                        //get dept name from dept id
                        if (deptListid != null)
                        {
                            departmentList = await _context.Department.Where(w => deptListid.Contains(w.id)).Select(s => s.name).ToListAsync();
                        }
                        //insert to final list

                        empView.Add(new EmployeeViewModel()
                        {
                            id = eachemp.id,
                            EmployeeName = eachemp.EmployeeName,
                            EmployeeImage = eachemp.EmployeeImage,
                            Gender = eachemp.Gender,
                            Salary = eachemp.Salary,
                            StartDate = eachemp.StartDate,
                            Notes = eachemp.Notes,
                            CreatedDate = eachemp.CreatedDate,
                            UpdatedDate = eachemp.UpdatedDate,
                            DepartmentName = departmentList
                        });
                    }
                    return empView.ToList();
                }
                else
                {
                    emp = await _context.Employee.ToListAsync();
                }

                //lists all employee
                if (emp == null)
                {
                    return NoContent();
                }
                foreach (Employee eachemp in emp)
                {
                    //getting department names from each record
                    //get the list of dept ids with emp id
                    List<int> deptListid = await _context.EmployeeDepartmentMap.Where(w => w.EmployeeId == eachemp.id).Select(s => s.DepartmentId).ToListAsync();

                    List<string> departmentList = new List<string>();
                    //get dept name from dept id
                    if (deptListid != null)
                    {
                        departmentList = await _context.Department.Where(w => deptListid.Contains(w.id)).Select(s => s.name).ToListAsync();
                    }
                    //insert to final list

                    empView.Add(new EmployeeViewModel()
                    {
                        id = eachemp.id,
                        EmployeeName = eachemp.EmployeeName,
                        EmployeeImage = eachemp.EmployeeImage,
                        Gender = eachemp.Gender,
                        Salary = eachemp.Salary,
                        StartDate = eachemp.StartDate,
                        Notes = eachemp.Notes,
                        CreatedDate = eachemp.CreatedDate,
                        UpdatedDate = eachemp.UpdatedDate,
                        DepartmentName = departmentList
                    });
                }
                #region code is for redis cache
                serialized_EmployeeList = JsonConvert.SerializeObject(empView);
                resultFromCache = Encoding.UTF8.GetBytes(serialized_EmployeeList);
                var options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await _distributedCache.SetAsync(azureCacheKey, resultFromCache, options);
                #endregion
                if (empView==null)
                {
                    return NoContent();
                }
                return empView;
            }
            catch (Exception ex)
            {
                var tempEx = ex;
                //return StatusCode(StatusCodes.Status500InternalServerError,
                //    "Error retrieving data from the database");
                return NoContent();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeViewModel>> GetEmployee(int id)
        {
            try
            {
                //_logger.LogError("Logerror in GetEmployee");
                //_logger.LogInformation("LogInfo in GetEmployee");
                //_logger.LogDebug("Logdebug in GetEmployee");
                //_logger.LogTrace("Logtrace in GetEmployee");
                var emp = await _context.Employee
                .FirstOrDefaultAsync(e => e.id == id);
                List<int> deptListid = await _context.EmployeeDepartmentMap.Where(w => w.EmployeeId == emp.id).Select(s => s.DepartmentId).ToListAsync();

                List<string> departmentList = new List<string>();
                //get dept name from dept id
                if (deptListid != null)
                {
                    departmentList = await _context.Department.Where(w => deptListid.Contains(w.id)).Select(s => s.name).ToListAsync();
                }
                var empView = new EmployeeViewModel()
                {
                    //todo:pass id
                    id = emp.id,
                    EmployeeName = emp.EmployeeName,
                    EmployeeImage = emp.EmployeeImage,
                    Gender = emp.Gender,
                    Salary = emp.Salary,
                    StartDate = emp.StartDate,
                    Notes = emp.Notes,
                    CreatedDate = emp.CreatedDate,
                    UpdatedDate = emp.UpdatedDate,
                    DepartmentName = departmentList
                };

                if (empView == null)
                {
                    return NoContent();
                }
                return empView;
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(EmployeeViewModel employee)
        {
            try
            {
                //_logger.LogError("Logerror in PostEmployee");
                //_logger.LogInformation("LogInfo in PostEmployee");
                //_logger.LogDebug("Logdebug in PostEmployee");
                //_logger.LogTrace("Logtrace in PostEmployee");
                if (employee == null)
                {
                    return BadRequest();
                }
                var tempEmp = new Employee()
                {
                    EmployeeName = employee.EmployeeName,
                    EmployeeImage = employee.EmployeeImage,
                    Gender = employee.Gender,
                    Salary = employee.Salary,
                    StartDate = employee.StartDate,
                    Notes = employee.Notes,
                    CreatedDate = employee.CreatedDate,
                    UpdatedDate = employee.UpdatedDate

                };

                var result = await _context.Employee.AddAsync(tempEmp);
                await _context.SaveChangesAsync();
                foreach (var id in employee.DepartmentId)
                {
                    //int id = dept.id;
                    var tempMap = new EmployeeDepartmentMap()
                    {
                        EmployeeId = tempEmp.id,
                        DepartmentId = id
                    };
                    _context.EmployeeDepartmentMap.Add(tempMap);
                    await _context.SaveChangesAsync();
                }
                return result.Entity;
            }
            catch
            {
                return NoContent();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> PutEmployee(EmployeeViewModel employee)
        {
            try
            {
                //custom logging
                //_logger.LogError("Logerror in PutEmployee");
                //_logger.LogInformation("LogInfo in PutEmployee");
                //_logger.LogDebug("Logdebug in PutEmployee");
                //_logger.LogTrace("Logtrace in PutEmployee");
                var result = await _context.Employee
                .FirstOrDefaultAsync(e => e.id == employee.id);
                List<EmployeeDepartmentMap> tempmap = await _context.EmployeeDepartmentMap.Where(w => w.EmployeeId == employee.id).ToListAsync();
                if (tempmap != null)
                {
                    foreach (var entry in tempmap)
                    {
                        _context.EmployeeDepartmentMap.Remove(entry);
                        await _context.SaveChangesAsync();
                    }
                }
                if (result != null)
                {
                    result.EmployeeName = employee.EmployeeName;
                    result.EmployeeImage = employee.EmployeeImage;
                    result.Gender = employee.Gender;
                    result.CreatedDate = employee.CreatedDate;
                    result.Salary = employee.Salary;
                    result.StartDate = employee.StartDate;
                    result.Notes = employee.Notes;
                    result.UpdatedDate = employee.UpdatedDate;

                    await _context.SaveChangesAsync();
                    foreach (var id in employee.DepartmentId)
                    {
                        //int id = dept.id;
                        var tempMap = new EmployeeDepartmentMap()
                        {
                            EmployeeId = employee.id,
                            DepartmentId = id
                        };
                        _context.EmployeeDepartmentMap.Add(tempMap);
                        await _context.SaveChangesAsync();
                    }
                    //redis
                    await _distributedCache.RemoveAsync(azureCacheKey);
                    return result;
                }

                return null;
            }
            catch
            {
                return NoContent();
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                //_logger.LogError("Logerror in DeleteEmployee");
                //_logger.LogInformation("LogInfo in DeleteEmployee");
                //_logger.LogDebug("Logdebug in DeleteEmployee");
                //_logger.LogTrace("Logtrace in DeleteEmployee");
                var result = await _context.Employee.FindAsync(id);

                if (result != null)
                {
                    _context.Employee.Remove(result);
                    await _context.SaveChangesAsync();
                }
                await _distributedCache.RemoveAsync(azureCacheKey);
                return NoContent();
            }
            catch
            {
                return NoContent();
            }
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.id == id);
        }
    }
}
