using EmployeeApi.Core.EmployeesService.Interfaces;
using EmployeeApi.Core.EmployeesService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAll()
        {
            return Ok(await _employeesService.GetAll());
        }

        /// <summary>
        /// Gets all employees
        /// </summary>
        [HttpGet("bossId/{bossId}")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetAllByBossId(int bossId)
        {
            return Ok(await _employeesService.GetAllByBossId(bossId));
        }

        /// <summary>
        /// Gets single employee by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetSingleById(int id)
        {
            return Ok(await _employeesService.GetSingleById(id));
        }

        /// <summary>
        /// Gets the count and average salary of employees by role
        /// </summary>
        [HttpGet("stats/by-role/{role}")]
        public async Task<ActionResult<(int, double)>> GetEmployeeStatsByRole(string role)
        {
            var (count, averageSalary) = await _employeesService
                    .EmployeesCountAndAverageSalaryByRole(role);

            return Ok(new { Count = count, AverageSalary = averageSalary });
        }

        /// <summary>
        /// Search employees by name and birthday interval
        /// </summary>
        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> Search([FromBody] EmployeeSearchRequest searchRequest)
        {
            var employees = await _employeesService.SearchByNameAndBirthdateInterval(searchRequest);

            return Ok(employees);
        }

        /// <summary>
        /// Creates an employee by employee create request
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> Create(
            [FromBody] EmployeeCreateRequest request)
        {
            var createdEmployee = await _employeesService.Create(request);

            return CreatedAtAction(
                "GetSingleById",
                new { id = createdEmployee.Id },
                createdEmployee);
        }

        /// <summary>
        /// Updates employee by given id and employee request
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeCreateRequest request)
        {
            await _employeesService.Update(id, request);
            return NoContent();
        }

        /// <summary>
        /// Updates employee salary by given id and provided salary
        /// </summary>
        [HttpPatch("{id}/salary")]
        public async Task<IActionResult> UpdateSalary(
            int id, [FromBody] EmployeeSalaryUpdateRequest salaryUpdateRequest)
        {
            await _employeesService.UpdateSalary(id, salaryUpdateRequest);

            return NoContent();
        }

        /// <summary>
        /// Deletes employee by given id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeesService.Delete(id);

            return NoContent();
        }
    }
}
