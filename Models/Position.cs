using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace HumanResources.Models
{
    public class Position
    {
        [Display(Name = "ID")]
        public int PositionID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Наименование")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Оклад")]
        public int Salary { get; set; }


        public Employee Employee { get; set; }

        //public ICollection<Employee> Employees { get; set; }
    }
}
