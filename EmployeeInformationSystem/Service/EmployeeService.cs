using EmployeeInformationSystem.Interface;
using EmployeeInformationSystem.Model;

namespace EmployeeInformationSystem.Service
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public void AddEmployee(Employee employee)
        {
            _repository.AddEmployee(employee);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _repository.GetAllEmployees();
        }

        public void UpdateEmployee(int id, string column, object value)
        {
            _repository.UpdateEmployee(id, column, value);
        }

        public void DeleteEmployee(int id)
        {
            _repository.DeleteEmployee(id);
        }
    }

}
