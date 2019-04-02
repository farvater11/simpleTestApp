using System;
using System.Collections.Generic;
using System.Linq;
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
                for (int i = 0; i < userList.Count; i++)
                {
                    if (userList[i].Id.Equals(user.Id))
                    {
                        userList[i] = user;
                        break;
                    }
                }
            }
        }
    }
}
