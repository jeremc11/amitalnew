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
using Newtonsoft.Json;

namespace WebApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return _context.Users.ToList();
        }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }

    [Route("api/users/tasks")]
    [ApiController]
    public class UsersTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/users/tasks
        [HttpGet]
        public IActionResult Get()
        {
            // Group tasks by UserId
            var tasks = _context.Tasks.AsEnumerable();
            var groupedTasks = tasks.GroupBy(task => task.UserId);

            Dictionary<int, int> userTasks = new Dictionary<int, int>();

            foreach (var user in groupedTasks)
            {
                userTasks.Add(user.Key, user.Where(t => t.IsCompleted == false).ToList().Count());
            }

            string jsonResult = JsonConvert.SerializeObject(userTasks);
            return Content(jsonResult, "application/json");
        }

        public HttpResponseMessage Options()
        {
            return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
        }
    }

}
