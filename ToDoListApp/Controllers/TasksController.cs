using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using Services;
using Serilog;
using SerilogTimings;

namespace ToDoListApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskRepository;
        private readonly ILogger<TasksController> _logger;
        private readonly IDiagnosticContext _diagnosticContext;
        public TasksController(ITaskService taskRepository, ILogger<TasksController> logger, IDiagnosticContext diagnosticContext)
        {
            _taskRepository = taskRepository;
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
                tasks = await _taskRepository.GetAllTasksAsync();
                _diagnosticContext.Set("Tasks", tasks);
                return View(tasks);
            };
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Details method called");
            var task = await _taskRepository.GetTaskByIdAsync(id);
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
                await _taskRepository.AddTaskAsync(task);
                _logger.LogInformation("New Task has been created..");
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
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
                await _taskRepository.UpdateTaskAsync(task);
                _logger.LogInformation("Task is edited.");
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskRepository.GetTaskByIdAsync(id);
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
            await _taskRepository.DeleteTaskAsync(id);
            _logger.LogInformation("Task is Deleted.");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportToCsv()
        {
            var tasks = await _taskRepository.GetAllTasksAsync();

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

