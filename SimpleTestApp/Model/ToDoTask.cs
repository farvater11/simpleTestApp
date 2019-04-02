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
        public string Title { get; set; }
        public bool IsCompleted { get; set; }

        public ToDoTask(string title, string text)
        {
            Title = title;
            Text = text;
            Id = Guid.NewGuid();
            IsCompleted = false;
        }
    }
}
