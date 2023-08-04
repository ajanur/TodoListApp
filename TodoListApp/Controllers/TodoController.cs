using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApp.Filters;
using TodoListApp.Models;
using TodoListApp.ServiceExtension;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoListApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        //Log enabled for todo services
        private readonly ILoggerManager _logger;

        //Memory collection is being used
        static List<Todo> todoList = new List<Todo>();
        public TodoController(IList<Todo> list, ILoggerManager logger) 
        {
            _logger = logger;
            todoList = list as List<Todo>;
        }


        // GET: api/<TodoController>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInfo($" Return all todos with count: {todoList.Count}");
            return Ok(todoList);
        }

        //public async Task<IActionResult> GetAsync()
        //{
        //    _logger.LogInfo($" Return all todos with count: {todoList.Count}");
        //    return Ok(todoList);
        //}

        // GET api/<TodoController>/5
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            _logger.LogInfo($" Searching by Task Id: {id}");
            return Ok(todoList.Find(p => p.Id == id));
        }

        // GET api/<TodoController>/<name>
        [HttpGet]
        [Route("task/{name}")]
        public IActionResult GetByName(string name)
        {
            if(name == null)
            {
                _logger.LogInfo($" Invalid name");
                return BadRequest("Name is not valid");
            }
            _logger.LogInfo($" Searching by Task Name: {name}");
            return Ok(todoList.Find(p => p.Name == name));
        }

        // GET api/<TodoController>/<name>
        [HttpGet]
        [Route("priority/{id}")]
        public IActionResult GetTodoByPriority(int priority)
        {
            if (priority == 0)
            {
                _logger.LogInfo($" Invalid priority");
                return BadRequest("Priority is not valid");
            }
            _logger.LogInfo($" Searched by Task Priority: {priority}");
            return Ok(todoList.FindAll(p => p.Priority == priority));

        }

        // GET api/<TodoController>/<name>
        [HttpGet]
        [Route("status/{status}")]
        public IActionResult GetTodoByStatus(string status)
        {
            _logger.LogInfo($" Searched todo by Task Status: {status}");
            return Ok(todoList.FindAll(p => p.Status == status));
        }

        // GET api/<TodoController>/<name>
        [HttpGet]
        [Route("{status}/{priority}")]
        public IActionResult GetTodoByPriorityAndStatus(int priority, string status)
        {
            _logger.LogInfo($" Searched by Task Priority: {priority} and status {status}");
            return Ok(todoList.FindAll(p => p.Status == status && p.Priority == priority));
        }

        // POST api/<TodoController>
        [HttpPost]
        //[ServiceFilter(typeof(ModelValidationAttribute))]
        [ModelValidation]
        public IActionResult Post([FromBody] Todo value)
        {
            if(value == null)
            {
                _logger.LogInfo($" Invalid Todo");
                return BadRequest("Todo is not valid");
            }
            if(isNameExist(value.Name))
            {
                return StatusCode(409,"Task name is already exists");
            }
            todoList.Add(value);
            _logger.LogInfo($"Task: {value.Name} has been added");
            return Ok();
           
        }

        // PUT api/<TodoController>/<name>
        [HttpPut]
        public IActionResult Put([FromBody] Todo value)
        {
            if (value == null)
            {
                _logger.LogInfo($" Invalid Todo");
                return BadRequest("Todo is not valid");
            }
            var todo = todoList.Find(p => p.Id == value.Id);
            if(todo == null)
            {
                return NotFound("Todo is not found");
            }
            //todo.Name = value.Name;
            todo.Priority = value.Priority;
            todo.Status = value.Status;
            _logger.LogInfo($"Task: {todo.Name} has been updated");
            return Ok();
        }

        // PUT api/<TodoController>/<name>
        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody] Todo value)
        {
            if (value == null)
            {
                _logger.LogInfo($" Invalid Todo");
                return BadRequest();
            }

            var todo = todoList.Find(p => p.Name == name);
            if (todo == null)
            {
                return NotFound();
            }
            //todo.Name = value.Name;
            todo.Priority = value.Priority;
            todo.Status = value.Status;
            _logger.LogInfo($"Task: {todo.Name} has been updated");
            return Ok();
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {

            var todo = todoList.Find(p => p.Id == id);
            if (todo == null)
            {
                return BadRequest("Todo is not found");
            }
            todoList.Remove(todo);
            _logger.LogInfo($"Task: {todo.Name} has been removed");
            return Ok();

        }
        [HttpGet]
        [Route("status")]
        public IActionResult Status()
        {
            //Try catch can be removed as Global error handler enabled
            try
            {

                var list =  System.Enum.GetValues(typeof(StatusList))
                    .Cast<StatusList>()
                    .Select(v => v.ToString())
                    .ToList();
                return Ok(list);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                return StatusCode(500, "Internal error");
            }
        }

        //Private members

        private bool isExist(Guid id)
        {
            var res = todoList.Find(x => x.Id == id);
            return res == null ? false:true;
        }
        private bool isNameExist(string name)
        {
            var res = todoList.Find(x => x.Name == name);
            return res == null ? false : true;
        }
    }
}
