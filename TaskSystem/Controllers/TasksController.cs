using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskSystem.Data;
using TaskSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;


namespace WebApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        const int MaxTasksPerUser = 10;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<TaskSystem.Models.Task>> Get()
        {
            return _context.Tasks.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<TaskSystem.Models.Task> GetById(int id)
        {
            var item = _context.Tasks.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Post([FromBody] TaskSystem.Models.Task task)
        {
            // get all tasks for user in input
            var tasks = _context.Tasks.Where(t => t.UserId == task.UserId).ToList();

            // if the user reached the max opened tasks per user, prevent the request
            if (tasks.Where(t => t.IsCompleted == false).Count() >= MaxTasksPerUser)
            {
                return BadRequest("max opened tasks reached");
            }
            // if the task already exists for this user, prevent the request
            else if (tasks.Where(t => t.IsCompleted == false).Count() >= MaxTasksPerUser || tasks.Find(t => t.Subject == task.Subject) != null)
            {
                return BadRequest("task already exists for user");
            }
            else
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
            }
        }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }

}
