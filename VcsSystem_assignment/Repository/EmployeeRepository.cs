using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VcsSystem_assignment.Data;
using VcsSystem_assignment.Models;

namespace VcsSystem_assignment.Repository
{
    public class EmployeeRepository
    {
        private readonly ApplicationDbContext _dbcontext;
 
        public EmployeeRepository(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public string AddEmployee(DateTime dateOfBirth, string name, string email, string pictureUrl)
        {
            var dobParam = new SqlParameter("@DateOfBirth", dateOfBirth);
            var nameParam = new SqlParameter("@Name", name);
            var emailParam = new SqlParameter("@Email", email);
            var pictureParam = new SqlParameter("@pictureUrl", pictureUrl);

            var result = _dbcontext.Database.ExecuteSqlRaw(
                "EXEC addEmployee @DateOfBirth, @Name, @Email, @pictureUrl",
                dobParam, nameParam, emailParam, pictureParam);

            return result > 0 ? "Employee inserted successfully." : "Failed to insert employee.";
        }
        public Employee GetEmployeeById(int id)
        {
            var employeeIdParam = new SqlParameter("@ID", id);

            var employee = _dbcontext.Employee
                .FromSqlRaw("EXEC GetEmployeeByID @ID", employeeIdParam)
                .ToList()
                .FirstOrDefault();

            if (employee == null)
            {
                throw new InvalidOperationException("No employee found with the specified ID.");
            }

            return employee;
        }

        public void UpdateEmployee(int id, DateTime dateOfBirth, string name, string email, string pictureUrl)
        {
            var idParam = new SqlParameter("@ID", id);
            var dobParam = new SqlParameter("@DateOfBirth", dateOfBirth);
            var nameParam = new SqlParameter("@Name", name);
            var emailParam = new SqlParameter("@Email", email);
            var pictureParam = new SqlParameter("@PictureUrl", pictureUrl);

            // Execute the stored procedure
            try
            {
                var result = _dbcontext.Database.ExecuteSqlRaw(
                    "EXEC updateEmployee @ID, @DateOfBirth, @Name, @Email, @PictureUrl",
                    idParam, dobParam, nameParam, emailParam, pictureParam);

                if (result == 0)
                {
                    throw new InvalidOperationException("No employee found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                throw new InvalidOperationException($"An error occurred while updating the employee: {ex.Message}");
            }
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            try
            {
                return _dbcontext.Employee
                    .FromSqlRaw("EXEC GetAllEmployees")
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while retrieving employees: {ex.Message}");
            }
        }
        public void DeleteEmployee(int id)
        {
            var idParam = new SqlParameter("@ID", id);

            try
            {
                var result = _dbcontext.Database.ExecuteSqlRaw("EXEC DeleteEmployee @ID", idParam);

                if (result == 0)
                {
                    throw new InvalidOperationException("No employee found with the specified ID.");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"An error occurred while deleting the employee: {ex.Message}");
            }
        }

        internal void AddEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
