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
            return userList;
        }

        public void AddUser(User user)
        {
            userList.Add(user);
        }

        public User GetUser(string username, string password)
        {
            foreach (var item in userList)
            {
                if (item.Username.Equals(username) && item.Password.Equals(password))
                    return item;
            }
            return null;
        }

        public User GetUser(Guid id)
        {
            foreach (var item in userList)
            {
                if (item.Id.Equals(id))
                    return item;
            }
            return null;
        }

        public void ModifyUser (User user)
        {
            for(int i = 0; i < userList.Count; i++)
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
