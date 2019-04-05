using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTestApp.Model
{
    public class UserLab
    {
        private static UserLab userLab;
        private List<User> userList;
        SimpleTestAppContext context = new SimpleTestAppContext();

        public static UserLab Get()
        {
            if (userLab == null)
                userLab = new UserLab();
            return userLab;
        }

        private UserLab()
        {
            userList = new List<User>();
        }

        public List<User> GetUserList()
        {
            return null;
        }

        public void Add(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }
        public void Add(ToDoTask task)
        {
            context.ToDoTasks.Add(task);
            context.SaveChanges();
        }
        public void Add(ToDoTaskList list)
        {
            context.ToDoTaskLists.Add(list);
            context.SaveChanges();
        }

        public void Remove(User user)
        {
            context.Users.Remove(user);
            context.Entry(user).State = EntityState.Deleted;
            context.SaveChanges();
        }
        public void Remove(ToDoTaskList list)
        {
            context.ToDoTaskLists.Remove(list);
            //context.Entry(list).State = EntityState.Deleted;
            context.SaveChanges();
        }
        public void Remove(ToDoTask task)
        {
            context.ToDoTasks.Remove(task);
            //context.Entry(task).State = EntityState.Deleted;
            context.SaveChanges();
        }

        public User GetUser(string username, string password)
        {
            User user = context.Users
                .Include(u => u.ListOfLists)
                .Include(u => u.ListOfLists.Select(t => t.Tasks))
                .Where(c => (c.Username == username && c.Password == password))
                .FirstOrDefault();
            return user;
        }
        public User GetUser(string username)
        {
            User user = context.Users
               .Include(u => u.ListOfLists)
               .Include(u => u.ListOfLists.Select(t => t.Tasks))
               .Where(c => (c.Username == username))
               .FirstOrDefault();
            return user;
        }
        public User GetUser(Guid id)
        {
            User user = context.Users
                .Include(u => u.ListOfLists)
                .Include(u => u.ListOfLists.Select(t => t.Tasks))
                .Where(c => (c.Id.Equals(id)))
                .FirstOrDefault();
            return user;
        }

        public void Modify(User user)
        {
            User userModify = context.Users.Find(user.Id);
            userModify.FirstName = user.FirstName;
            userModify.LastName = user.LastName;
            userModify.Username = user.Username;
            userModify.Password = user.Password;
            context.SaveChanges();
        }
        public void Modify(ToDoTaskList list)
        {
            ToDoTaskList modifyList = context.ToDoTaskLists.Find(list.Id);
            modifyList.Title = list.Title;
            context.SaveChanges();
        }
        public void Modify(ToDoTask task)
        {
            ToDoTask modifyTask = context.ToDoTasks.Find(task.Id);
            modifyTask.Text = task.Text;
            modifyTask.Title = task.Title;
            modifyTask.IsCompleted = task.IsCompleted;
            context.SaveChanges();
        }
    }
}
