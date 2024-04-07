namespace EmployeeApi.Core.EmployeesService.Models
{
    public class EmployeeSearchRequest
    {
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
