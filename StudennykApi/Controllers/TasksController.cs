using Microsoft.AspNetCore.Mvc;
using StudennykApi.Models;
using StudennykApi.Repositories;

using Task = StudennykApi.Models.Task;

namespace StudennykApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly SQLiteRepository _repository;
        private List<Task> _tasks;

        public TasksController(SQLiteRepository sqliteRepository)
        {
            _repository = sqliteRepository;
            _tasks = sqliteRepository.GetTasks().ToList();
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(_tasks);
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] Task task)
        {
            task.Id = _repository.AddTask(task);
            _tasks.Add(task);

            return CreatedAtAction(nameof(GetTaskById), new { task_id = task.Id }, task.Id);
        }

        [HttpGet("{task_id}")]
        public IActionResult GetTaskById(int task_id)
        {

            var task = _tasks.Find(t => t.Id == task_id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPut("{task_id}")]
        public IActionResult UpdateTask(int task_id, [FromBody] UpdateTask updatedTask)
        {
            var task = _tasks.Find(t => t.Id == task_id);
            if (task == null)
            {
                return NotFound();
            }

            // Update only specified fields
            if (updatedTask.Title is not null && !updatedTask.Title!.Equals(task.Title))
            {
                task.Title = updatedTask.Title;
                _repository.UpdateTaskTitle(task.Id, updatedTask.Title);
            }
            if (updatedTask.Description is not null && !updatedTask.Description!.Equals(task.Description))
            {
                task.Description = updatedTask.Description;
                _repository.UpdateTaskDescription(task.Id, updatedTask.Description);
            }
            if (updatedTask.Due is not null && !updatedTask.Due!.Equals(task.Due))
            {
                var due = updatedTask.Due ?? task.Due;
                task.Due = due;
                _repository.UpdateTaskDueTime(task.Id, due);
            }
            if (updatedTask.IsDone is not null && !updatedTask.IsDone!.Equals(task.IsDone))
            {
                var is_done = updatedTask.IsDone ?? task.IsDone;
                task.IsDone = is_done;
                _repository.UpdateTaskStatus(task.Id, is_done);
            }

            return NoContent();
        }

        [HttpDelete("{task_id}")]
        public IActionResult DeleteTask(int task_id)
        {
            var task = _tasks.Find(t => t.Id == task_id);

            if (task == null)
            {
                return NotFound();
            }

            _tasks.Remove(task);
            _repository.DeleteTask(task.Id);
            return NoContent();
        }
    }

}
