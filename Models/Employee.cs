using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace HumanResources.Models
{
    public class Employee
    {
        public int ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [Display(Name = "ФИО")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата трудоустройства")]
        public DateTime DateOfEmployment { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 11)]
        [Display(Name = "Телефон")]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Должность (ID)")]
        public int PositionID { get; set; }

        [Display(Name = "Отдел (ID)")]
        public int DepartmentID { get; set; }

        [Display(Name = "Отдел")]
        public Department Department { get; set; }
        [Display(Name = "Должность")]
        public Position Position { get; set; }

        public ICollection<Department> Departments { get; set; }
        public ICollection<Position> Positions { get; set; }
    }
}
