using Microsoft.AspNetCore.Mvc;
using StudennykApi.Models;

using Task = StudennykApi.Models.Task;

namespace StudennykApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TasksController : ControllerBase
    {
        private static List<Task> tasks = new List<Task>
        {
            new Task { Id = 1, Title = "Task 1", Description = "Description for Task 1", Created = DateTime.UtcNow, Due = DateTime.UtcNow.AddDays(1), IsDone = false },
            new Task { Id = 2, Title = "Task 2", Description = "Description for Task 2", Created = DateTime.UtcNow, Due = DateTime.UtcNow.AddDays(2), IsDone = false }
        };

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(tasks);
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] Task task)
        {
            task.Id = tasks.Count + 1;
            tasks.Add(task);
            return CreatedAtAction(nameof(GetTaskById), new { task_id = task.Id }, task.Id);
        }

        [HttpGet("{task_id}")]
        public IActionResult GetTaskById(int task_id)
        {
            var task = tasks.Find(t => t.Id == task_id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPut("{task_id}")]
        public IActionResult UpdateTask(int task_id, [FromBody] UpdateTask updatedTask)
        {
            var task = tasks.Find(t => t.Id == task_id);
            if (task == null)
            {
                return NotFound();
            }

            // Update only specified fields
            task.Title = updatedTask.Title ?? task.Title;
            task.Description = updatedTask.Description ?? task.Description;
            var due = updatedTask.Due.Equals(task.Due) ? task.Due : updatedTask.Due;
            task.Due = due ?? task.Due;
            var isDone = updatedTask.IsDone.Equals(task.IsDone) ? task.IsDone : updatedTask.IsDone;
            task.IsDone = isDone ?? task.IsDone;

            return NoContent();
        }

        [HttpDelete("{task_id}")]
        public IActionResult DeleteTask(int task_id)
        {
            var task = tasks.Find(t => t.Id == task_id);
            if (task == null)
            {
                return NotFound();
            }

            tasks.Remove(task);
            return NoContent();
        }
    }

}
