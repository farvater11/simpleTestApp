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
        static public User UserRegistration()
        {
            Console.Write("Username: "); string username = Console.ReadLine();
            Console.Write("First name: "); string firstName = Console.ReadLine();
            Console.Write("Last name: "); string lastName = Console.ReadLine();
            Console.Write("Password "); string password = Console.ReadLine();
            SimpleTestApp.Model.User user = new User(username, firstName, lastName, password);
            if (user != null)
            {
                if (UserLab.Get().GetUser(user.Username, user.Password) != null) // If already exist currentUser with this userName and password
                    return null;
                UserLab.Get().AddUser(user);
            }
            return user;
        }
        static public User UserLoginIn()
        {
            Console.Write("Username: "); string username = Console.ReadLine();
            Console.Write("Password "); string password = Console.ReadLine();
            User user = UserLab.Get().GetUser(username, password);
            if (user == null)
                return null;
            return user;
        }
        static public void UserLoginOut()
        {
            //goto labelStart;
        }

        static public User AddToUserListOfList(string title, User user) // II stage
        {
            ToDoTaskList toDoTaskList = new ToDoTaskList(user.Id, title);

            user.ListOfLists.Add(toDoTaskList);
            UserLab.Get().ModifyUser(user);

            return UserLab.Get().GetUser(user.Id);
        }
        static public string GetTitlesOfAllTaskLists(User user) // Return list of titles All TaskList
        {
            //Remake from string to stringBuilder
            string allTitles = "";
            foreach (var item in user.ListOfLists)
                allTitles += "Title: \"" + item.Title + "\"; id: " + item.Id + "\n";

            return allTitles;
        }
        static public ToDoTaskList FindList(string title, User user)
        {
            foreach (var item in user.ListOfLists)
            {
                if (item.Title.Equals(title))
                    return item;
            }
            return null;
        }

        static public bool DeleteTaskList(ToDoTaskList list, User user) // Delete founded list
        {
            bool result = user.ListOfLists.Remove(list);
            if (result)
                UserLab.Get().ModifyUser(user); // Modify singletone 
            return result;
        }
        static public User AddTask(string title, string text, ToDoTaskList taskList, User user) // Add task into tasklist III stage
        {
            ToDoTask task = new ToDoTask(title, text);

            ToDoTaskList newList = taskList;
            newList.Tasks.Add(task);


            User modifyUser = ModifyUserTaskLists(taskList, user);
            //user.ListOfLists[user.ListOfLists.IndexOf(taskList)] = newList;

            //UserLab.Get().ModifyUser(user);
            UserLab.Get().ModifyUser(modifyUser); // Push to singleton
            return UserLab.Get().GetUser(user.Id); // Pull from singleton 
        }
        static public string GetAllTaskInList(ToDoTaskList taskList, User user) //Return list of titles All Tasks in list
        {
            //Remake from string to stringBuilder
            string result = "";
            List<ToDoTask> list = FindList(taskList.Title, user).Tasks; // Debug. Get  current 
            foreach (var item in list)
                result += "Title: \"" + item.Title + "\"; Text: \"" + item.Text + "\" id: " + item.Id + "\n";
            return result;
        }
        static public bool DeleteTask(ToDoTask task, ToDoTaskList taskList, User user)
        {
            bool result = taskList.Tasks.Remove(task);
            User modifyUser = ModifyUserTaskLists(taskList, user);
            if (result)
                UserLab.Get().ModifyUser(modifyUser); // Modify singletone 
            return result;
        }

        static public User ModifyUserTaskLists(ToDoTaskList list, User user) // Modify users one Task list
        {
            Guid lsitId = list.Id;
            for (int i = 0; i < user.ListOfLists.Count; i++)
            {
                if (user.ListOfLists[i].Id.Equals(list.Id))
                    user.ListOfLists[i] = list;
            }
            UserLab.Get().ModifyUser(user);
            return UserLab.Get().GetUser(user.Id);
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

        static public User ModifyTask(ToDoTask task, ToDoTaskList taskList, User user) // Modify task in users taskList 
        {
            Guid taskId = task.Id;
            for (int i = 0; i < taskList.Tasks.Count; i++)
            {
                if (taskList.Tasks[i].Id.Equals(task.Id))
                    taskList.Tasks[i] = task;
            }
            User userModify = ModifyUserTaskLists(taskList, user);
            UserLab.Get().ModifyUser(userModify);
            return userModify;
        }
    }
}
