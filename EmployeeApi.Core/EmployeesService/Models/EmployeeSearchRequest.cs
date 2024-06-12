namespace EmployeeApi.Core.EmployeesService.Models
{
    public class EmployeeSearchRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDateFrom { get; set; }
        public DateTime BirthDateTo { get; set; }
    }
}
