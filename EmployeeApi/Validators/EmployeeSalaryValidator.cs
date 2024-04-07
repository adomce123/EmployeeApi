using EmployeeApi.Core.EmployeesService.Models;
using FluentValidation;

namespace EmployeeApi.Validators
{
    public class EmployeeSalaryValidator : AbstractValidator<EmployeeSalaryUpdateRequest>
    {
        public EmployeeSalaryValidator()
        {
            RuleFor(e => e.CurrentSalary)
                .NotEmpty().WithMessage("Current salary is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Current salary must be non-negative.");
        }
    }
}
