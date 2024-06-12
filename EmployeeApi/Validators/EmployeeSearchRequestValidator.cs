using EmployeeApi.Core.EmployeesService.Models;
using FluentValidation;

namespace EmployeeApi.Validators
{
    public class EmployeeSearchRequestValidator : AbstractValidator<EmployeeSearchRequest>
    {
        public EmployeeSearchRequestValidator()
        {
            RuleFor(request => request.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot be longer than 50 characters.");

            RuleFor(request => request.BirthDateFrom)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThanOrEqualTo(r => r.BirthDateTo).WithMessage("Start date must be before or equal to the end date.");

            RuleFor(request => request.BirthDateTo)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThanOrEqualTo(r => r.BirthDateFrom).WithMessage("End date must be after or equal to the start date.")
                .LessThanOrEqualTo(DateTime.Today).WithMessage("End date cannot be in the future.");
        }
    }
}
