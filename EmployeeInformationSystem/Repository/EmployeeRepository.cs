using Dapper;
using EmployeeInformationSystem.Interface;
using EmployeeInformationSystem.Model;
using Microsoft.Data.SqlClient;


namespace EmployeeInformationSystem.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddEmployee(Employee employee)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Employees (FirstName, LastName, Email, DateOfBirth, Salary)
                             VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @Salary)";
                connection.Execute(query, employee);
            }
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Employees";
                return connection.Query<Employee>(query);
            }
        }

        public void UpdateEmployee(int id, string column, object value)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = $"UPDATE Employees SET {column} = @Value WHERE EmployeeID = @EmployeeID";
                connection.Execute(query, new { Value = value, EmployeeID = id });
            }
        }

        public void DeleteEmployee(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Employees WHERE EmployeeID = @EmployeeID";
                connection.Execute(query, new { EmployeeID = id });
            }
        }
    }
}
