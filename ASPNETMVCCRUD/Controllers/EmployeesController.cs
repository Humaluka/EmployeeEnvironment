using EmployersEnvironment.Data;
using EmployersEnvironment.Models;
using EmployersEnvironment.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployersEnvironment.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeerDbContext employeeDbContext;

        public EmployeesController(EmployeerDbContext employeeDbContext)
        {
            this.employeeDbContext = employeeDbContext;
        }
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var employees = await employeeDbContext.Employees.ToListAsync(); 
            return View(employees); 
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeViewModel) 
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name= addEmployeeViewModel.Name,
                Email= addEmployeeViewModel.Email,
                Salary= addEmployeeViewModel.Salary,
                DateOfBirth= addEmployeeViewModel.DateOfBirth,
                Department= addEmployeeViewModel.Department
            };
            await employeeDbContext.Employees.AddAsync(employee);
            await employeeDbContext.SaveChangesAsync();
            return View("Add");
        }

        [HttpGet]

        public async Task<IActionResult> View(Guid id)
        { 
            var employee = await employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id==id);
            if (employee !=null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };
                return await System.Threading.Tasks.Task.Run(()=> View("View", viewModel));
            }
           
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await employeeDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email= model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth= model.DateOfBirth;
                employee.Department = model.Department;

                await employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
               return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await employeeDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employeeDbContext.Employees.Remove(employee);
                await employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
