using EmployersEnvironment.Data;
using EmployersEnvironment.Models;
using EmployersEnvironment.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployersEnvironment.Controllers
{
    public class TasksController : Controller
    {
        private readonly EmployeerDbContext employeeDbContext;

        public TasksController(EmployeerDbContext employeeDbContext)
        {
            this.employeeDbContext = employeeDbContext;
        }
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            var tasks = await employeeDbContext.Tasks.ToListAsync(); 
            return View(tasks); 
        }



        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTaskViewModel addTaskViewModel) 
        {
            var task = new Models.Domain.Task()
            {
                TaskID = Guid.NewGuid(),
                Description= addTaskViewModel.Description,
                Spring = addTaskViewModel.Spring,
                CompleteTime = addTaskViewModel.CompleteTime,
                Stage = addTaskViewModel.Stage,
                AssigneeID = addTaskViewModel.AssigneeID,
                AuthorID = addTaskViewModel.AuthorID,
                SubtaskID = addTaskViewModel.SubtaskID
            };
            await employeeDbContext.Tasks.AddAsync(task);
            await employeeDbContext.SaveChangesAsync();
            return View("Add");
        }

        [HttpGet]

        public async Task<IActionResult> View(Guid id)
        { 
            var task = await employeeDbContext.Tasks.FirstOrDefaultAsync(x => x.TaskID==id);
            if (task !=null)
            {
                var viewModel = new UpdateTaskViewModel()
                {
                    TaskID = task.TaskID,
                    Description = task.Description,
                    Spring = task.Spring,
                    CompleteTime = task.CompleteTime,
                    Stage = task.Stage,
                    AssigneeID = task.AssigneeID,
                    AuthorID = task.AuthorID,
                    SubtaskID = task.SubtaskID
                };
                return await System.Threading.Tasks.Task.Run(()=> View("View", viewModel));
            }
           
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateTaskViewModel model)
        {
            var task = await employeeDbContext.Tasks.FindAsync(model.TaskID);
            if (task != null)
            {
                task.TaskID = model.TaskID;
                task.Description = model.Description;
                task.Spring = model.Spring;
                task.CompleteTime = model.CompleteTime;
                task.Stage = model.Stage;
                task.AssigneeID = model.AssigneeID;
                task.AuthorID = model.AuthorID;
                task.SubtaskID = model.SubtaskID;

                await employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
               return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateTaskViewModel model)
        {
            var task = await employeeDbContext.Tasks.FindAsync(model.TaskID);
            if (task != null)
            {
                employeeDbContext.Tasks.Remove(task);
                await employeeDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
