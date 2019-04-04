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

        public static UserLab Get()
        {
            if (userLab == null)
                userLab = new UserLab();
            return userLab;
        }

        private UserLab()
        {
            userList = new List<User>();
            //Debug
            userList.Add(new User("uname", "fname", "lname", "pass"));

        }

        public List<User> GetUserList()
        {
            using (SimpleTestAppContext context = new SimpleTestAppContext())
            {

                return userList;
            }
        }

        public void AddUser(User user)
        {
            using (SimpleTestAppContext context = new SimpleTestAppContext())
            {
                userList.Add(user);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }

        public User GetUser(string username, string password)
        {
            using (SimpleTestAppContext context = new SimpleTestAppContext())
            {
                User user = context.Users
                    .Include(u => u.ListOfLists)
                    .Include(u => u.ListOfLists.Select(t => t.Tasks))
                    .Where(c => (c.Username == username && c.Password == password))
                    .FirstOrDefault();
                return user;

                foreach (var item in userList)
                {
                    if (item.Username.Equals(username) && item.Password.Equals(password))
                        return item;
                }
                return null;
            }
        }

        public User GetUser(Guid id)
        {
            using (SimpleTestAppContext context = new SimpleTestAppContext())
            {
                User user = context.Users
                    .Include(u => u.ListOfLists)
                    .Include(u => u.ListOfLists.Select(t => t.Tasks))
                    .Where(c => c.Id == id)
                    .FirstOrDefault();
                return context.Users.Find(user.Id);

                foreach (var item in userList)
                {
                    if (item.Id.Equals(id))
                        return item;
                }
                return null;
            }
        }

        public void ModifyUser (User user)
        {
            using (SimpleTestAppContext context = new SimpleTestAppContext())
            {
                User userModify = context.Users.Find(user.Id);
                //userModify = user;
                userModify.FirstName = user.FirstName;
                userModify.LastName = user.LastName;
                userModify.Username = user.Username;
                userModify.Password = user.Password;
                userModify.ListOfLists = user.ListOfLists;
                context.SaveChanges();
                /*
                for (int i = 0; i < userList.Count; i++)
                {
                    if (userList[i].Id.Equals(user.Id))
                    {
                        userList[i] = user;
                        break;
                    }
                }*/
            }
        }
    }
}
