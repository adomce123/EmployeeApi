using EmployeeApi.Core.EmployeesService.Interfaces;
using EmployeeApi.Core.EmployeesService.Models;
using FluentValidation;

namespace EmployeeApi.Controllers.Models.Validators
{
    public class EmployeeCreationValidator : AbstractValidator<EmployeeCreateRequest>
    {
        private const int YoungestAge = 18;
        private const int OldestAge = 70;
        private const string CeoRole = "Ceo";
        private readonly IEmployeesService _employeesService;

        public EmployeeCreationValidator(IEmployeesService employeesService)
        {
            _employeesService = employeesService;

            RuleFor(e => e.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot be longer than 50 characters.");

            RuleFor(e => e.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot be longer than 50 characters.")
                .NotEqual(e => e.FirstName).WithMessage("FirstName and LastName cannot be the same.");

            RuleFor(e => e.HomeAddress)
                .NotEmpty().WithMessage("Home address is required.")
                .MaximumLength(100).WithMessage("Home address cannot be longer than 100 characters.");

            RuleFor(e => e.Role)
                .NotEmpty().WithMessage("Role is required.")
                .MaximumLength(50).WithMessage("Role cannot be longer than 50 characters.")
                .MustAsync(BeUniqueCeo).WithMessage($"Only a single {CeoRole} role can exist.");

            RuleFor(e => e.Birthdate)
                .NotEmpty().WithMessage("Birthdate is required.")
                .Must(BeAValidAge).WithMessage($"Employee must be at least {YoungestAge} years old and not older than {OldestAge} years.");

            RuleFor(e => e.EmploymentDate)
                .NotEmpty().WithMessage("Employment date is required.")
                .GreaterThanOrEqualTo(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .WithMessage("Employment date cannot be earlier than 2000-01-01.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("Employment date cannot be a future date.");

            RuleFor(e => e.CurrentSalary)
                .GreaterThanOrEqualTo(0).WithMessage("Current salary must be non-negative.");

            RuleFor(employee => employee)
                .Custom((employee, context) =>
                {
                    if (employee.Role == CeoRole && employee.BossId != null)
                    {
                        context.AddFailure($"{CeoRole} role cannot have a boss id as it has no boss");
                    }

                    if (employee.Role != CeoRole && employee.BossId == null)
                    {
                        context.AddFailure($"Only {CeoRole} role can have no boss");
                    }
                });
        }

        private bool BeAValidAge(DateTime birthdate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthdate.Year;
            // If birthday not already occured this year subtract one year from age
            if (birthdate > today.AddYears(-age)) age--;
            return age >= YoungestAge && age <= OldestAge;
        }

        private async Task<bool> BeUniqueCeo(string role, CancellationToken ct)
        {
            if (role == CeoRole)
            {
                var ifCeoExists = await _employeesService.CheckIfCeoExists(ct);
                if (ifCeoExists)
                    return false;
            }

            return true;
        }
    }
}
