using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTestApp.Model;

namespace SimpleTestApp.Control
{
    public static class MainControl
    {
        static public User UserRegistration(string username, string firstName, string lastName, string password)
        {
            SimpleTestApp.Model.User user = new User(username, firstName, lastName, password);
            if (user != null)
            {
                if (UserLab.Get().GetUser(user.Username) != null) // If already exist currentUser with this userName and password
                    return null;
                UserLab.Get().Add(user);
            }
            return user;
        }


        static public User FindUser(string username, string password)
        {
            User user = UserLab.Get().GetUser(username, password);
            if (user == null)
                return null;
            return user;
        }
        static public ToDoTaskList FindList(string title, User user)
        {
            User user1 = UserLab.Get().GetUser(user.Id);
            foreach (var item in user1.ListOfLists)
            {
                if (item.Title.Equals(title))
                    return item;
            }
            return null;
        }
        static public ToDoTask FindTask(string title, ToDoTaskList taskList)
        {
            foreach (var item in taskList.Tasks)
            {
                if (item.Title.Equals(title))
                    return item;
            }
            return null;
        }

        static public void Delete(ToDoTaskList list) // Delete founded list
        {
            UserLab.Get().Remove(list);
        }
        static public void Delete(ToDoTask task) // Delete founded list
        {
            UserLab.Get().Remove(task);
        }
        static public void Delete(User user) // Delete founded user
        {
            UserLab.Get().Remove(user);
        }

        static public User Add(string title, User user) // II stage
        {
            ToDoTaskList toDoTaskList = new ToDoTaskList(user.Id, title);
            UserLab.Get().Add(toDoTaskList);
            return UserLab.Get().GetUser(user.Id);
        }
        static public User Add(string title, string text, ToDoTaskList taskList, User user) // Add task into tasklist III stage
        {
            ToDoTask task = new ToDoTask(title, text, taskList.Id);
            UserLab.Get().Add(task);
            return UserLab.Get().GetUser(user.Id); // Pull from singleton 
        }

        static public string GetTitlesOfAllTaskLists(User user) // Return list of titles All TaskList
        {
            StringBuilder result = new StringBuilder();
            foreach (var item in user.ListOfLists)
                result.Append("Title: \"" + item.Title + "\"; id: " + item.Id + "\n");
            return result.ToString();
        }
        static public string GetAllTaskInList(ToDoTaskList taskList, User user) //Return list of titles All Tasks in list
        {
            StringBuilder result = new StringBuilder();
            List<ToDoTask> list = FindList(taskList.Title, user).Tasks; // Debug. Get  current 
            foreach (var item in list)
                result.Append("Title: \"" + item.Title + "\" is "+(item.IsCompleted ? "Completed" : "Not completed") +"; Text: \"" + item.Text + "\" id: " + item.Id + "\n");
            return result.ToString();
        }

        static public User Modify(ToDoTaskList list, User user) // Modify users one Task list
        {
            UserLab.Get().Modify(list);
            return UserLab.Get().GetUser(user.Id);
        }
        static public User Modify(ToDoTask task, User user) // Modify task in users taskList 
        {
            UserLab.Get().Modify(task);
            return UserLab.Get().GetUser(user.Id);
        }
    }
}
