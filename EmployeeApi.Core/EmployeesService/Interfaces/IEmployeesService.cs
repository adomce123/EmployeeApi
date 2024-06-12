using EmployeeApi.Core.EmployeesService.Models;

namespace EmployeeApi.Core.EmployeesService.Interfaces
{
    public interface IEmployeesService
    {
        Task<EmployeeDto> Create(EmployeeCreateRequest employeeToCreate);
        Task Delete(int id);
        Task<(int count, double averageSalary)> EmployeesCountAndAverageSalaryByRole(string role);
        Task<IEnumerable<EmployeeDto>> GetAll();
        Task<IEnumerable<EmployeeDto>> GetAllByBossId(int bossId);
        Task<EmployeeDto> GetSingleById(int id);
        Task<IEnumerable<EmployeeDto>> SearchByNameAndBirthdateInterval(EmployeeSearchRequest searchRequest);
        Task Update(int id, EmployeeCreateRequest employeeToUpdate);
        Task UpdateSalary(int id, EmployeeSalaryUpdateRequest salaryUpdateRequest);
        bool CheckIfCeoExists();
    }
}