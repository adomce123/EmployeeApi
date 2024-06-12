using EmployeeApi.Infrastructure.Entities;

namespace EmployeeApi.Infrastructure.Repositories.Interfaces
{
    public interface IEmployeesRepository
    {
        Task<IEnumerable<Employee>> GetAll();
        Task<IEnumerable<Employee>> GetByBossId(int bossId);
        Task<Employee?> GetSingleById(int id);
        Task<Employee> Add(Employee employee);
        Task Delete(Employee employeeToDelete);
        Task Update(Employee employee);
        Task UpdateSalary(Employee employee);
        Task<IEnumerable<Employee>> SearchByNameAndBirthdateInterval(
            string name, DateTime startDate, DateTime endDate);
        Task<(int count, double averageSalary)> EmployeesCountAndAverageSalaryByRole(string role);
        Task<bool> CheckIfCeoExists(CancellationToken ct);
    }
}