using EmployeeInformationSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Interface
{
    public interface IEmployeeRepository
    {
        void AddEmployee(Employee employee);
        IEnumerable<Employee> GetAllEmployees();
        void UpdateEmployee(int id, string column, object value);
        void DeleteEmployee(int id);
    }

}
