using EmployeeApi.Controllers.Models.Validators;
using EmployeeApi.Core.EmployeesService.Models;
using FluentValidation.TestHelper;

namespace EmployeeApi.Tests;

public class EmployeeCreationValidatorTests
{
    private readonly EmployeeCreationValidator _validator = new EmployeeCreationValidator();

    [Fact]
    public void FirstNameWhenEmptyShouldHaveValidationError()
    {
        // Arrange
        var model = new EmployeeCreateRequest { FirstName = string.Empty };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.FirstName)
              .WithErrorMessage("First name is required.");
    }

    [Fact]
    public void LastNameWhenEmptyShouldHaveValidationError()
    {
        // Arrange
        var model = new EmployeeCreateRequest { LastName = string.Empty };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldHaveValidationErrorFor(request => request.LastName)
              .WithErrorMessage("Last name is required.");
    }

    [Fact]
    public void BirthdateSlightlyUnder18YearsShouldFail()
    {
        // Arrange
        var today = DateTime.Today;
        var birthdate = today.AddYears(-18).AddDays(1);

        var model = new EmployeeCreateRequest { Birthdate = birthdate };

        // Act 
        var result = _validator.TestValidate(model);

        // Assert 
        result.ShouldHaveValidationErrorFor(request => request.Birthdate)
              .WithErrorMessage($"Employee must be at least 18 years old and not older than 70 years.");
    }

    [Fact]
    public void ModelWhenValidShouldNotHaveValidationErrors()
    {
        // Arrange
        var model = new EmployeeCreateRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Role = "Developer",
            HomeAddress = "123 Main St",
            Birthdate = new DateTime(1985, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            EmploymentDate = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            CurrentSalary = 50000
        };

        // Act
        var result = _validator.TestValidate(model);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}

