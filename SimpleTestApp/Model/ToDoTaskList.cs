using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTestApp.Model
{
    public class ToDoTaskList
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set;}
        public string Title { get; set; }
        public List<ToDoTask> Tasks { get; set; }

       public ToDoTaskList(Guid userId, string title)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Title = title;
            Tasks = new List<ToDoTask>();
        }
    }
}
