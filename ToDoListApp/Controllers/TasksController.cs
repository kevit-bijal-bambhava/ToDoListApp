using CsvHelper;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SerilogTimings;
using ServiceContracts;
using System.Globalization;

namespace ToDoListApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public TasksController(ITaskService taskService, ILogger<TasksController> logger, IDiagnosticContext diagnosticContext)
        {
            _taskService = taskService;
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Index method called.");
            dynamic tasks;
            using(Operation.Time("Total time taken to fetch all tasks: "))
            {
                tasks = await _taskService.GetAllTasksAsync();
                _diagnosticContext.Set("Tasks", tasks);
                return View(tasks);
            };
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Details method called");
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                await _taskService.AddTaskAsync(task);
                _logger.LogInformation("New Task has been created..");
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                await _taskService.UpdateTaskAsync(task);
                _logger.LogInformation("Task is edited.");
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            _logger.LogInformation("Task is Deleted.");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportToCsv()
        {
            var tasks = await _taskService.GetAllTasksAsync();

            using (var memoryStream = new MemoryStream())
            using (var writer = new StreamWriter(memoryStream))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture, leaveOpen:true))
            {
                csv.WriteRecords(tasks);
                writer.Flush();
                return File(memoryStream.ToArray(), "text/csv", "MyTasks.csv");
            }
        }
    }
}

