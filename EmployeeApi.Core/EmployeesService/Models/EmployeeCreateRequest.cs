using System.Globalization;

namespace EmployeeApi.Core.EmployeesService.Models
{
    public class EmployeeCreateRequest
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _role = string.Empty;

        public string FirstName
        {
            get => _firstName;
            set => _firstName = ToUpperFirst(value);
        }

        public string LastName
        {
            get => _lastName;
            set => _lastName = ToUpperFirst(value);
        }

        public string Role
        {
            get => _role;
            set => _role = ToUpperFirst(value);
        }

        public string HomeAddress { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public DateTime EmploymentDate { get; set; }
        public int? BossId { get; set; }
        public int CurrentSalary { get; set; }

        private string ToUpperFirst(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            char[] array = value.ToLower(CultureInfo.InvariantCulture).ToCharArray();
            array[0] = char.ToUpper(array[0], CultureInfo.CurrentCulture);
            return new string(array);
        }
    }
}
