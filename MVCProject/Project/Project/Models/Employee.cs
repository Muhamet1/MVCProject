using System.ComponentModel;

namespace Project.Models
{
    public class Employee
    {
        public int employeeId { get; set; }
        [DisplayName("Employee Name")]
        public string employeeName { get; set; }
        [DisplayName("Employee LastName")]
        public string employeeLastName { get; set; }
        [DisplayName("Employee City")]
        public string employeeCity { get; set; }

        [DisplayName("Employee Department")]
        public string employeeDepartment { get; set; }

        [DisplayName("Employee Wage")]
        public int employeeWages { get; set; }

       }
}
