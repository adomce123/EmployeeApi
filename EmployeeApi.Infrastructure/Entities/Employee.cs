using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeApi.Infrastructure.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string HomeAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = string.Empty;

        public DateTime Birthdate { get; set; }

        public DateTime EmploymentDate { get; set; }

        public int? BossId { get; set; }

        [ForeignKey("BossId")]
        public Employee? Boss { get; set; }

        public int CurrentSalary { get; set; }
    }
}
