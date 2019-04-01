using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTestApp.Model
{
    public class ToDoTask
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }

        public ToDoTask(string text)
        {
            Id = Guid.NewGuid();
            Text = text;
            IsCompleted = false;
        }
    }
}
