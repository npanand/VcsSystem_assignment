using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VcsSystem_assignment.Models;
using VcsSystem_assignment.Repository;
using System.IO;

namespace VcsSystem_assignment.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(EmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var employeeList = _employeeRepository.GetAllEmployees().ToList();
                return View(employeeList);
            }
            catch (Exception)
            {
                TempData["Error"] = "Db error";
                return View(new List<Employee>());
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var employees =  _employeeRepository.GetAllEmployees().ToList();
            return Json(new { data = employees });
        }

        [HttpPost]
        public async Task<JsonResult> AddEmployee(Employee employee)
        {
            try
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                var file = HttpContext.Request.Form.Files;

                if (file.Count > 0)
                {
                    string newFileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootPath, @"Images\EmployeeImage");

                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                    }

                    var extension = Path.GetExtension(file[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                    {
                        await file[0].CopyToAsync(fileStream);
                    }
                    employee.Picture = @"\Images\EmployeeImage\" + newFileName + extension;
                }

                _employeeRepository.AddEmployee(employee.DateOfBirth, employee.Name, employee.Email, employee.Picture);
                return Json(new { success = true, message = "Added Successfully" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Db error" });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetById(int id)
        {
            var employee =  _employeeRepository.GetEmployeeById(id);
            return Json(new { data = employee });
        }

        [HttpPost]
        public async Task<JsonResult> UpdateEmployee(Employee employee)
        {
            try
            {
                var employeeToUpdate =  _employeeRepository.GetEmployeeById(employee.ID);
                employeeToUpdate.DateOfBirth = employee.DateOfBirth;
                employeeToUpdate.Name = employee.Name;
                employeeToUpdate.Email = employee.Email;
                string webRootPath = _webHostEnvironment.WebRootPath;
                var file = HttpContext.Request.Form.Files;
                if (file.Count > 0)
                {
                    string fullPath = Path.Combine(webRootPath, employeeToUpdate.Picture);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    string newFileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootPath, @"Images\EmployeeImage");
                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                    }

                    var extension = Path.GetExtension(file[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                    {
                        await file[0].CopyToAsync(fileStream);
                    }
                    employeeToUpdate.Picture = @"\Images\EmployeeImage\" + newFileName + extension;
                }

                _employeeRepository.UpdateEmployee(employeeToUpdate.ID, employeeToUpdate.DateOfBirth, employeeToUpdate.Name, employeeToUpdate.Email, employeeToUpdate.Picture);
                return Json(new { success = true, message = "Updated Successfully" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Db error" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> DeleteEmployee(int id)
        {
            try
            {
                var employee =  _employeeRepository.GetEmployeeById(id);
                if (employee != null)
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string fullPath = Path.Combine(webRootPath, employee.Picture);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    _employeeRepository.DeleteEmployee(id);
                    return Json(new { success = true, message = "Deleted Successfully" });
                }
                return Json(new { success = false, message = "Employee not found" });
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Db error" });
            }
        }
    }
}
