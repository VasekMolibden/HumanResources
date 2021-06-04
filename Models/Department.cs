using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace HumanResources.Models
{
    public class Department
    {
        [Display(Name = "ID")]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Глава (ID)")]
        public int? HeadID { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 6)]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Глава отдела")]
        public Employee Employee { get; set; }
        //public ICollection<Employee> Employees { get; set; }
    }
}
