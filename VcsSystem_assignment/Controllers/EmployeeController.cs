using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VcsSystem_assignment.Models;
using VcsSystem_assignment.Repository;

namespace VcsSystem_assignment.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(EmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            _webHostEnvironment=webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try { 
            var employeeList = _employeeRepository.GetAllEmployees().ToList();
            return View(employeeList);
             }
             catch (Exception ex)
            {
                TempData["Error"] = "Db error";
                return View();
            }
        }
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                string webRootpath = _webHostEnvironment.WebRootPath;
                var file = HttpContext.Request.Form.Files;
                if (file.Count > 0)
                {
                    string newFileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootpath, @"Images\EmployeeImage");

                    if (!Directory.Exists(upload))
                    {
                        Directory.CreateDirectory(upload);
                        Console.WriteLine($"Directory created at: {upload}");
                    }

                    var extension = Path.GetExtension(file[0].FileName);
                    using (var fileStream = new FileStream(Path.Combine(upload, newFileName + extension), FileMode.Create))
                    {
                        file[0].CopyTo(fileStream);
                    }
                   employee.Picture = @"\Images\EmployeeImage\" + newFileName + extension;
                }
                
                    _employeeRepository.AddEmployee(employee.DateOfBirth, employee.Name, employee.Email, employee.Picture);
                    TempData["Success"] = "Added Successfully";
                    return RedirectToAction(nameof(Index));
                
                return View();
               
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Db error";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            try
            {
                var employeeIdbasedDetails = _employeeRepository.GetEmployeeById(id);
                TempData["Success"] = "Added Successfully";
                return View(employeeIdbasedDetails);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Db error";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(Employee employee)
        {
            try
            {
                _employeeRepository.UpdateEmployee(employee.ID, employee.DateOfBirth,employee.Name, employee.Email, employee.Picture); 
                TempData["Success"] = "Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Db error";
                return View();
            }
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employeeIdbasedDetails = _employeeRepository.GetEmployeeById(id);

                TempData["Success"] = "Delete Successfully";
                return View(employeeIdbasedDetails);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Db error";
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Employee employee)
        {
            try
            {
                var employeeIdbasedDetails = _employeeRepository.GetEmployeeById(employee.ID);
                
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    string fullPath = Path.Combine(webRootPath, employeeIdbasedDetails.Picture); 
                _employeeRepository.DeleteEmployee(employee.ID);

                if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                        Console.WriteLine($"Deleted image: {fullPath}");
                    }
                    else
                    {
                        Console.WriteLine($"Image not found: {fullPath}");
                    }
                


                TempData["Success"] = "Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Db error";
                return View();
            }
        }


    }
}
