using API.Model.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public Priority Priority { get; set; }

        public bool Done { get; set; }
    }
}
