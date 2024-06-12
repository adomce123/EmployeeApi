using AutoMapper;
using EmployeeApi.Core.EmployeesService.Interfaces;
using EmployeeApi.Core.EmployeesService.Models;
using EmployeeApi.Core.Exceptions;
using EmployeeApi.Infrastructure.Entities;
using EmployeeApi.Infrastructure.Repositories.Interfaces;
using System.Data;

namespace EmployeeApi.Core.EmployeesService
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly IMapper _mapper;

        public EmployeesService(IEmployeesRepository employeesRepository, IMapper mapper)
        {
            _employeesRepository = employeesRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAll()
        {
            var entities = await _employeesRepository.GetAll();

            var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(entities);

            return dtos;
        }

        public async Task<EmployeeDto> GetSingleById(int id)
        {
            var entity = await _employeesRepository.GetSingleById(id);

            if (entity == null)
            {
                throw new NotFoundException($"Employee with ID {id} not found.");
            }

            var dto = _mapper.Map<EmployeeDto>(entity);

            return dto;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllByBossId(int bossId)
        {
            var employees = await _employeesRepository.GetAll();

            var employeesByBossId = employees.Where(e => e.BossId == bossId);

            if (!employeesByBossId.Any())
            {
                throw new NotFoundException($"No employees with boss id {bossId} were found");
            }

            var dtos = _mapper.Map<IEnumerable<EmployeeDto>>(employeesByBossId);

            return dtos;
        }

        public async Task<(int count, double averageSalary)> EmployeesCountAndAverageSalaryByRole(string role)
        {
            var (count, averageSalary) = await _employeesRepository
                .EmployeesCountAndAverageSalaryByRole(role);

            return (count, averageSalary);
        }

        public async Task<IEnumerable<EmployeeDto>> SearchByNameAndBirthdateInterval(
            EmployeeSearchRequest searchRequest)
        {
            var employees = await _employeesRepository
                .SearchByNameAndBirthdateInterval(
                searchRequest.Name, searchRequest.StartDate, searchRequest.EndDate);

            if (!employees.Any())
            {
                throw new NotFoundException("No employees matching the search criteria were found.");
            }

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

            return employeeDtos;
        }

        public async Task<EmployeeDto> Create(EmployeeCreateRequest employeeToCreate)
        {
            var entity = _mapper.Map<Employee>(employeeToCreate);

            var createdEmployee = await _employeesRepository.Add(entity);

            var dto = _mapper.Map<EmployeeDto>(createdEmployee);

            return dto;
        }

        public async Task Update(int id, EmployeeCreateRequest employeeToUpdate)
        {
            var existingEmployee = await _employeesRepository.GetSingleById(id);

            if (existingEmployee == null)
            {
                throw new NotFoundException($"Employee with ID {id} not found.");
            }

            _mapper.Map(employeeToUpdate, existingEmployee);

            await _employeesRepository.Update(existingEmployee);
        }

        public async Task UpdateSalary(int id, EmployeeSalaryUpdateRequest salaryUpdateRequest)
        {
            var existingEmployee = await _employeesRepository.GetSingleById(id);

            if (existingEmployee == null)
            {
                throw new NotFoundException($"Employee with ID {id} not found.");
            }

            existingEmployee.CurrentSalary = salaryUpdateRequest.CurrentSalary;

            await _employeesRepository.Update(existingEmployee);
        }

        public async Task Delete(int id)
        {
            var existingEmployee = await _employeesRepository.GetSingleById(id);

            if (existingEmployee == null)
            {
                throw new NotFoundException($"Employee with ID {id} not found.");
            }

            var subordinates = await _employeesRepository.GetByBossId(id);

            if (subordinates.Any())
            {
                throw new InvalidOperationException(
                    "Cannot delete an employee who is a boss without reassigning their subordinates.");
            }

            await _employeesRepository.Delete(existingEmployee);
        }

        public bool CheckIfCeoExists()
        {
            return _employeesRepository.CheckIfCeoExists();
        }
    }
}
