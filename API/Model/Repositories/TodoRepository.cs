using API.Model.Context;
using API.Model.Entities;
using API.Model.Primitives;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model.Repositories
{
    [Repository]
    public class TodoRepository : BaseRepository
    {
        public TodoRepository(ApplicationDatabaseContext dbContext) : base(dbContext)
        {
        }

        public Todo Create(User user, string title, DateTime dueDate, string description = null, Priority priority = Priority.None)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (title == null)
            {
                throw new ArgumentNullException("title");
            }
            var todo = new Todo()
            {
                UserId = user.Id,
                Title = title,
                Description = description,
                DueDate = dueDate,
                Priority = priority,
                Done = false
            };

            _dbContext.Add(todo);
            _dbContext.SaveChanges();
            return todo;
        }

        public void Update(Todo todo)
        {
            _dbContext.Todos.Update(todo);
            _dbContext.SaveChanges();
        }

        public void Delete(Todo todo)
        {
            _dbContext.Todos.Remove(todo);
            _dbContext.SaveChanges();
        }

        public IList<Todo> Find(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return _dbContext.Todos.Where(e => e.UserId == user.Id).OrderByDescending(e => e.DueDate.Date).ThenBy(e => e.Priority).ThenBy(e => e.Title).ToList();
        }

        public Todo Find(int id)
        {
            return _dbContext.Todos.Find(id);
        }
    }
}
