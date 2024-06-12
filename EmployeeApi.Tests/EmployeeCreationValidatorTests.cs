using EmployeeApi.Controllers.Models.Validators;
using EmployeeApi.Core.EmployeesService.Interfaces;
using EmployeeApi.Core.EmployeesService.Models;
using FluentValidation.TestHelper;
using Moq;

namespace EmployeeApi.Tests;

public class EmployeeCreationValidatorTests
{
    private readonly EmployeeCreationValidator _validator;
    private readonly Mock<IEmployeesService> _employeesServiceMock;

    public EmployeeCreationValidatorTests()
    {
        _employeesServiceMock = new Mock<IEmployeesService>();
        _validator = new EmployeeCreationValidator(_employeesServiceMock.Object);
    }

    [Fact]
    public async Task FirstNameWhenEmptyShouldHaveValidationError()
    {
        // Arrange
        var model = new EmployeeCreateRequest { FirstName = string.Empty };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.FirstName)
              .WithErrorMessage("First name is required.");
    }

    [Fact]
    public async Task LastNameWhenEmptyShouldHaveValidationError()
    {
        // Arrange
        var model = new EmployeeCreateRequest { LastName = string.Empty };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.LastName)
              .WithErrorMessage("Last name is required.");
    }

    [Fact]
    public async Task BirthdateSlightlyUnder18YearsShouldFail()
    {
        // Arrange
        var today = DateTime.Today;
        var birthdate = today.AddYears(-18).AddDays(1);

        var model = new EmployeeCreateRequest { Birthdate = birthdate };

        // Act 
        var result = await _validator.TestValidateAsync(model);

        // Assert 
        result.ShouldHaveValidationErrorFor(request => request.Birthdate)
              .WithErrorMessage("Employee must be at least 18 years old and not older than 70 years.");
    }

    [Fact]
    public async Task ModelWhenValidShouldNotHaveValidationErrors()
    {
        // Arrange
        var model = new EmployeeCreateRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Role = "Developer",
            BossId = 1,
            HomeAddress = "123 Main St",
            Birthdate = new DateTime(1985, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            EmploymentDate = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CurrentSalary = 50000
        };

        // Act
        var result = await _validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task ShouldFailIfTryingToCreateSecondCeoRole()
    {
        // Arrange
        var model = new EmployeeCreateRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Role = "CEO",
            HomeAddress = "123 Main St",
            Birthdate = new DateTime(1985, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            EmploymentDate = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CurrentSalary = 50000
        };

        var cancellationToken = new CancellationToken();

        _employeesServiceMock.Setup(_ => _.CheckIfCeoExists(cancellationToken))
            .ReturnsAsync(true);

        // Act
        var result = await _validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(request => request.Role)
              .WithErrorMessage("Only a single Ceo role can exist.");

        _employeesServiceMock.Verify(_ => _.CheckIfCeoExists(cancellationToken), Times.Once);
    }
}

