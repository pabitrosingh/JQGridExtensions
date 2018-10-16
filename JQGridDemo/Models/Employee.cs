using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JQGridDemo.Models
{
    public class Employee : IDisposable
    {
        public int EmpID { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public decimal Salary { get; set; }



        public List<Employee> GetEmployeeColection()
        {
            List<Employee> _Employees = new List<Employee>();

            _Employees.Add(new Employee() { EmpID = 1, Name = "Rahul", Gender = "Male", BirthDate = new DateTime(1980, 12, 30), Salary = 25000 });
            _Employees.Add(new Employee() { EmpID = 2, Name = "Smita", Gender = "Female", BirthDate = new DateTime(1990, 10, 15), Salary = 15000 });
            _Employees.Add(new Employee() { EmpID = 3, Name = "Romio", Gender = "Male", BirthDate = new DateTime(1991, 1, 3), Salary = 20000 });
            _Employees.Add(new Employee() { EmpID = 4, Name = "Rohan", Gender = "Male", BirthDate = new DateTime(1985, 5, 30), Salary = 35000 });
            _Employees.Add(new Employee() { EmpID = 4, Name = "Kavita", Gender = "Female", BirthDate = new DateTime(1993, 12, 10), Salary = 12000 });

            return _Employees;

        }


        public void Dispose()
        {
            GC.Collect();
        }
    }
}