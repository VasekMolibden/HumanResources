using HumanResources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HumanResources.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HumanResourcesContext context)
        {
            context.Database.EnsureCreated();

            if (context.Employees.Any())
            {
                return;
            }

            var positions = new Position[]
            {
                new Position{Title="Директор",Salary=50000},
                new Position{Title="Охранник",Salary=35000},
                new Position{Title="Разработчик",Salary=45000},
                new Position{Title="Менеджер по продажам",Salary=40000},
                new Position{Title="Электрик",Salary=40000}
            };
            foreach (Position p in positions)
            {
                context.Positions.Add(p);
            }
            context.SaveChanges();

            var departments = new Department[]
            {
                new Department{Name="Охрана",PhoneNumber="43-80-62"},
                new Department{Name="Разработка",PhoneNumber="63-72-70"},
                new Department{Name="Продажа",PhoneNumber="31-05-05"},
                new Department{Name="Техническое обслуживание",PhoneNumber="87-95-37"}
            };
            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }
            context.SaveChanges();

            var employees = new Employee[]
            {   
                new Employee{Name="Иванов И.В.",DateOfEmployment=DateTime.Parse("2012-02-01"),PhoneNumber="+71223344556",DateOfBirth=DateTime.Parse("1990-02-01"),PositionID=3,DepartmentID=2},
                new Employee{Name="Смирнова М.С.",DateOfEmployment=DateTime.Parse("2015-05-11"),PhoneNumber="+79856777513",DateOfBirth=DateTime.Parse("1992-12-15"),PositionID=5,DepartmentID=4},
                new Employee{Name="Мельников А.А.",DateOfEmployment=DateTime.Parse("2016-01-25"),PhoneNumber="+77518564780",DateOfBirth=DateTime.Parse("1995-04-09"),PositionID=3,DepartmentID=2},
                new Employee{Name="Архипов П.И.",DateOfEmployment=DateTime.Parse("2016-11-25"),PhoneNumber="+79813254100",DateOfBirth=DateTime.Parse("1985-06-10"),PositionID=2,DepartmentID=1},
                new Employee{Name="Белый А.Г.",DateOfEmployment=DateTime.Parse("2017-05-05"),PhoneNumber="+73273454499",DateOfBirth=DateTime.Parse("1991-01-10"),PositionID=3,DepartmentID=2},
                new Employee{Name="Раскольникова В.В.",DateOfEmployment=DateTime.Parse("2017-10-15"),PhoneNumber="+79673444999",DateOfBirth=DateTime.Parse("1990-06-15"),PositionID=4,DepartmentID=3}
            };
            foreach (Employee e in employees)
            {
                context.Employees.Add(e);
            }
            context.SaveChanges();
        }
    }
}
