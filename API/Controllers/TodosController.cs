using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Filters;
using API.Model.Entities;
using API.Model.Primitives;
using API.Model.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers.API
{
    [Route("api/todos")]
    [TransactionFilter]
    public class TodosController : BaseAPIController
    {

        public TodoRepository TodoRepository {
            get {
                return HttpContext.RequestServices.GetService<TodoRepository>();
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<TodoObject>> Get()
        {
            return Ok(TodoRepository.Find(CurrentUser).Select(e => transform(e)));
        }

        [HttpGet("{id}")]
        public ActionResult<TodoObject> Get(int id)
        {
            Todo todo = TodoRepository.Find(id);
            if (todo == null || todo.UserId != CurrentUser.Id)
            {
                return NotFound();
            }
            return Ok(transform(todo));
        }

        [HttpPost]
        public ActionResult<TodoObject> Post([FromBody] PostRequestData requestData)
        {
            Todo todo = TodoRepository.Create(
                user: CurrentUser,
                title: requestData.Title,
                dueDate: requestData.DueDate,
                description: requestData.Description,
                priority: requestData.Priority
                );
            return CreatedAtAction(nameof(Get), new { id = todo.Id }, transform(todo));
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] PutRequestData requestData)
        {
            Todo todo = TodoRepository.Find(id);
            if (todo == null || todo.UserId != CurrentUser.Id)
            {
                return NotFound();
            }
            todo.Title = requestData.Title ?? todo.Title;
            todo.Description = requestData.Description ?? todo.Description;
            todo.DueDate = requestData.DueDate ?? todo.DueDate;
            todo.Priority = requestData.Priority ?? todo.Priority;
            todo.Done = requestData.Done ?? todo.Done;
            TodoRepository.Update(todo);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Todo todo = TodoRepository.Find(id);
            if (todo == null || todo.UserId != CurrentUser.Id)
            {
                return NotFound();
            }
            TodoRepository.Delete(todo);
            return NoContent();
        }

        public TodoObject transform(Todo todo)
        {
            return new TodoObject()
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                DueDate = todo.DueDate,
                Priority = todo.Priority,
                Done = todo.Done
            };
        }

        public class TodoObject
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
            public Priority Priority { get; set; }
            public bool Done { get; set; }
        }

        public class PostRequestData
        {
            [JsonProperty(Required = Required.Always)]
            public string Title { get; set; }
            [JsonProperty(Required = Required.Default)]
            public string Description { get; set; }
            [JsonProperty(Required = Required.Always)]
            public DateTime DueDate { get; set; }
            [JsonProperty(Required = Required.Default)]
            public Priority Priority { get; set; }
        }

        public class PutRequestData
        {
            [JsonProperty(Required = Required.Default)]
            public string Title { get; set; }
            [JsonProperty(Required = Required.Default)]
            public string Description { get; set; }
            [JsonProperty(Required = Required.Default)]
            public DateTime? DueDate { get; set; }
            [JsonProperty(Required = Required.Default)]
            public Priority? Priority { get; set; }
            [JsonProperty(Required = Required.Default)]
            public bool? Done { get; set; }
        }
    }
}
