using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTestApp.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public List<ToDoTaskList> ListOfLists { get; set; }

        public User(string username, string firstName, string lastName, string password)
        {
            Id = Guid.NewGuid();
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            ListOfLists = new List<ToDoTaskList>();
        }
        public User()
        {
        }
    }
}

