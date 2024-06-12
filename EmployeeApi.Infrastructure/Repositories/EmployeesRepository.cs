using EmployeeApi.Infrastructure.Entities;
using EmployeeApi.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Infrastructure.Repositories
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly EmployeesDbContext _dbContext;

        public EmployeesRepository(EmployeesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _dbContext.Employees
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee?> GetSingleById(int id)
        {
            return await _dbContext.Employees
                .SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Employee>> GetByBossId(int bossId)
        {
            return await _dbContext.Employees
                .Where(e => e.BossId == bossId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Employee> Add(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task Update(Employee employee)
        {
            _dbContext.Employees.Update(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSalary(Employee employee)
        {
            _dbContext.Entry(employee).Property(e => e.CurrentSalary).IsModified = true;
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Employee employeeToDelete)
        {
            _dbContext.Employees.Remove(employeeToDelete);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> SearchByNameAndBirthdateInterval(
            string name, DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Employees
                .Where(e => EF.Functions.Like(e.FirstName, $"%{name}%"))
                .Where(e => e.Birthdate >= startDate && e.Birthdate <= endDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<(int count, double averageSalary)> EmployeesCountAndAverageSalaryByRole(string role)
        {
            var employeesByRole = _dbContext.Employees
                .Where(e => e.Role == role)
                .AsNoTracking();

            int count = await employeesByRole.CountAsync();
            double averageSalary = await employeesByRole.AverageAsync(e => e.CurrentSalary);

            return (count, averageSalary);
        }

        public async Task<bool> CheckIfCeoExists(CancellationToken ct)
        {
            return await _dbContext.Employees.AnyAsync(emp => emp.Role == "Ceo", ct);
        }
    }
}
