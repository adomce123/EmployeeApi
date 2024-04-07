using AutoMapper;
using EmployeeApi.Core.EmployeesService;
using EmployeeApi.Core.EmployeesService.Models;
using EmployeeApi.Infrastructure.Entities;
using EmployeeApi.Infrastructure.Repositories.Interfaces;
using FluentAssertions;
using Moq;

namespace EmployeeApi.Tests;

public class EmployeeServiceTests
{
    private readonly Mock<IEmployeesRepository> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly EmployeesService _employeeService;

    public EmployeeServiceTests()
    {
        _mockRepository = new Mock<IEmployeesRepository>();
        _mockMapper = new Mock<IMapper>();
        _employeeService = new EmployeesService(_mockRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task ShouldCreateEmployee()
    {
        // Arrange
        var employeeToCreate = new EmployeeCreateRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Role = "Developer",
            HomeAddress = "123 Main St",
            Birthdate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            EmploymentDate = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            BossId = 1,
            CurrentSalary = 50000
        };

        var createdEntity = new Employee
        {
            Id = 1,
            FirstName = employeeToCreate.FirstName,
            LastName = employeeToCreate.LastName,
            Role = employeeToCreate.Role,
            HomeAddress = employeeToCreate.HomeAddress,
            Birthdate = employeeToCreate.Birthdate,
            EmploymentDate = employeeToCreate.EmploymentDate,
            BossId = employeeToCreate.BossId,
            CurrentSalary = employeeToCreate.CurrentSalary
        };

        var expectedDto = new EmployeeDto
        {
            Id = createdEntity.Id,
            FirstName = createdEntity.FirstName,
            LastName = createdEntity.LastName,
            Role = createdEntity.Role,
            HomeAddress = createdEntity.HomeAddress,
            Birthdate = createdEntity.Birthdate,
            EmploymentDate = createdEntity.EmploymentDate,
            BossId = createdEntity.BossId,
            CurrentSalary = createdEntity.CurrentSalary
        };

        _mockRepository.Setup(repo => repo.Add(It.IsAny<Employee>()))
                       .ReturnsAsync(createdEntity);

        _mockMapper.Setup(mapper => mapper.Map<Employee>(It.IsAny<EmployeeCreateRequest>()))
                   .Returns(createdEntity);

        _mockMapper.Setup(mapper => mapper.Map<EmployeeDto>(It.IsAny<Employee>()))
                   .Returns(expectedDto);

        // Act
        var result = await _employeeService.Create(employeeToCreate);

        // Assert
        result.Should().BeEquivalentTo(expectedDto, options => options.ComparingByMembers<EmployeeDto>());
    }

    [Fact]
    public void CeoRoleValidationShouldThrowExceptionWhenCeoHasBoss()
    {
        // Arrange
        var ceoRole = "Ceo";

        var employeeToCreate = new EmployeeCreateRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Role = ceoRole,
            BossId = 1, // CEO should not have a boss
        };

        // Act & Assert
        var act = () => _employeeService.Create(employeeToCreate);

        // Assert
        act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"{ceoRole} role cannot have boss id as it has no boss");
    }
}

