using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTestApp.Model;

namespace SimpleTestApp
{
    class Program
    {
        static private SimpleTestApp.Model.User currentUser; // Current user for session
        static private ToDoTaskList currentList; // Current tasklist 
        static private ToDoTask currentTask; // Current task


        static void Main(string[] args)
        {
        labelStart:
            bool newUser; // Mark for registration action
            currentList = null;
            currentTask = null;
            currentUser = null;
            Console.Clear();
            Console.WriteLine("==Task-list system==\n====================");

        labelAfterStart:

        //Remake to oneLine-code or reccursion
        labelNotCorrectChoose:
            Console.Write("LoginIn[1] or Register[2]: ");
            string action = Console.ReadLine();
            if (action.Equals("1"))
                newUser = false;
            else if (action.Equals("2"))
                newUser = true;
            else
                goto labelNotCorrectChoose;

            // Registration
            if (newUser)
            {
                currentUser = UserRegistration();
                if(currentUser != null) 
                    Console.WriteLine("\nYou are successfully registered!");
                else
                    Console.WriteLine("\nUser already does exist");
            }
            // LoginIn
            else
            {
                currentUser = UserLoginIn();
                if(currentUser == null)
                {
                    Console.Write("\n User not founded ");
                    Console.ReadKey();
                    goto labelStart;
                }
            }

            //If currentUser doesn`t have listOfLists. Create
            if (currentUser.ListOfLists.Count == 0)
                Console.Write("\nYou don`t have no one TaskList. ");
            
            
            //Create list
            Console.Write("\nDo you want to create new taskList? (skip if don`t)(%Title%): "); string answer = Console.ReadLine();
            if (!answer.Equals(""))
            {
                currentUser = AddToUserListOfList(answer, currentUser); // Add new List to ListOfList and refresh currentUser link 
                Console.WriteLine("Task List \"" + currentUser.ListOfLists.Last().Title + "\" has been created ");
            }

            //See all tasklists
            Console.Write("\nDo you want to see all your taskLists? (Yes/No): "); string answer1 = Console.ReadLine();
            if (answer1.Equals("Yes"))
            {
                Console.WriteLine("Total count of lists " + currentUser.ListOfLists.Count + " :");
                Console.WriteLine(GetTitlesOfAllTaskLists(currentUser));
            }

            //Choose some taskList by name
            Console.Write("\nDo you want to choose some taskList? (skip if don`t)(%Title%): "); string answerTitle1 = Console.ReadLine();
            if (!answerTitle1.Equals(""))
            {
                currentList = FindList(answerTitle1, currentUser);
                if (currentList != null)
                    Console.WriteLine("Choosen list has name: \"" + currentList.Title + "\"; and id: " + currentList.Id);
                else
                    Console.WriteLine("List with name: \"" + answerTitle1 + "\" not found");
            }

            //Delete tasklist  using name 
            Console.Write("\nDo you want to delete some taskList ? (skip if don`t) (%Title%): "); string answer2 = Console.ReadLine();
            if (!answer2.Equals(""))
            {
                if (DeleteTaskList(FindList(answer2, currentUser), currentUser)) // If deleted was correctly
                {
                    Console.WriteLine("List with name \"" + answer2 + "\" has been succesfully deleted");
                    currentUser = UserLab.Get().GetUser(currentUser.Id); // Refresh current user
                    if (currentList != null)
                    {
                        if (answer2 == currentList.Title) // If deleted list with current name has been deleted, refresh current list 
                            currentList = null;
                    }
                }
                else
                    Console.WriteLine("Err");
            }
            

            if (currentList != null)
            {
                //Modify tasklist 
                Console.Write("\nDo you want to modify current list ? (Yes/No): "); string answer5 = Console.ReadLine();
                if (answer5.Equals("Yes"))
                {
                    Console.Write("Input new title: "); string answerTitle = Console.ReadLine();
                    currentList.Title = answerTitle;

                    currentUser = ModifyUserTaskLists(currentList, currentUser);
                    currentList = FindList(currentList.Title, currentUser); // Refresh. Now not useful, but in future can be useful 

                    Console.WriteLine("You modified list with name \"" + currentList.Title + "\"; and id: " + currentList.Id);
                }


                //Adding new task in current list
                Console.Write("\nDo you want to add some task in current list ? (skip if don`t)(%Title%): "); string answer3 = Console.ReadLine();
                if (!answer3.Equals(""))
                {
                    Console.Write("Input new task: "); string answer7 = Console.ReadLine();
                    currentUser = AddTask(answer3, answer7, currentList, currentUser);
                    currentList = FindList(currentList.Title, currentUser);
                    ToDoTask task = FindTask(answer3, currentList);
                    Console.WriteLine("\nIn tasklist \"" + currentList.Title + "\" succesfully has been added new task \"" + task.Title + "\"");
                }


                //See all tasks in list 
                Console.Write("\nDo you want to see tasks in current list ? (Yes/No): "); string answer4 = Console.ReadLine();
                if (answer4.Equals("Yes"))
                {
                    List<ToDoTask> list = FindList(currentList.Title, currentUser).Tasks; // Debug. Get  current . Getting count of tasks

                    Console.WriteLine("Total count of tasks " + list.Count + " :");
                    Console.WriteLine(GetAllTaskInList(currentList, currentUser));
                }


                //Choose some task in current tasklist
                Console.Write("\nDo you want to choose task in current list ? (skip if don`t)(%Title%): "); string answer6 = Console.ReadLine();
                if (!answer6.Equals(""))
                {
                    currentTask = FindTask(answer6, currentList);
                    if (currentTask != null)
                        Console.WriteLine("Choosen task has title: \"" + currentTask.Title + "\"; and id: " + currentTask.Id);
                    else
                        Console.WriteLine("Task with name: \"" + answer6 + "\" not found");
                }

                //Delete task in current taskList using name 
                Console.Write("\nDo you want to delete some task in current taskList ? (skip if don`t) (%Title%): "); string answer8 = Console.ReadLine();
                if (!answer8.Equals(""))
                {
                    ToDoTask taskForDelete = FindTask(answer8, currentList);
                    if(taskForDelete != null)
                    {
                        if (DeleteTask(taskForDelete, currentList, currentUser)) // If deleted was correctly
                        {
                            Console.WriteLine("Task with name \"" + answer8 + "\" has been succesfully deleted");
                            currentUser = UserLab.Get().GetUser(currentUser.Id); // Refresh current user
                            currentList = FindList(currentList.Title, UserLab.Get().GetUser(currentUser.Id));
                            currentTask = FindTask(answer8, currentList);
                        }
                        else
                            Console.WriteLine("Err");
                    }
                    else
                        Console.WriteLine("Task with name: \"" + answer8 + "\" not found");
                }


                if (currentTask != null)
                {
                    Console.Write("\nDo you want to modify current task ? (Yes/No): "); string answer9 = Console.ReadLine();
                    if (answer9.Equals("Yes"))
                    {
                        Console.Write("Input new title: "); string answerTitle = Console.ReadLine();
                        currentTask.Title = answerTitle;
                        Console.Write("Input new task: "); string answerTask = Console.ReadLine();
                        currentTask.Text = answerTask;

                        currentUser = ModifyTask(currentTask, currentList, currentUser);
                        currentList = FindList(currentList.Title, currentUser);
                        currentTask = FindTask(currentTask.Title, currentList); // Refresh. Now not useful, but in future can be useful 

                        Console.WriteLine("You modified task with name \"" + currentTask.Title + "\"; and id: " + currentTask.Id);
                    }
                }
            }
            
            





            goto labelAfterStart;
            Console.ReadKey();
        }
        static private User UserRegistration()
        {
            Console.Write("Username: "); string username = Console.ReadLine();
            Console.Write("First name: "); string firstName = Console.ReadLine();
            Console.Write("Last name: "); string lastName = Console.ReadLine();
            Console.Write("Password "); string password = Console.ReadLine();
            SimpleTestApp.Model.User user = new User(username, firstName, lastName, password);
            if(user != null)
            {
                if (UserLab.Get().GetUser(user.Username, user.Password) != null) // If already exist currentUser with this userName and password
                    return null;
                UserLab.Get().AddUser(user);
            }
            return user;
        }
        static private User UserLoginIn()
        {
            Console.Write("Username: "); string username = Console.ReadLine();
            Console.Write("Password "); string password = Console.ReadLine();
            User user = UserLab.Get().GetUser(username, password);
            if (user == null) 
                return null;
            return user;
        }
        static private void UserLoginOut()
        {
            //goto labelStart;
        }

        static private User AddToUserListOfList(string title, User user) // II stage
        {
            ToDoTaskList toDoTaskList = new ToDoTaskList(user.Id, title);

            user.ListOfLists.Add(toDoTaskList);
            UserLab.Get().ModifyUser(user);

            return UserLab.Get().GetUser(user.Id);
        }
        static private string GetTitlesOfAllTaskLists(User user) // Return list of titles All TaskList
        {
            //Remake from string to stringBuilder
            string allTitles = "";
            foreach(var item in user.ListOfLists)
                allTitles += "Title: \"" + item.Title + "\"; id: " + item.Id + "\n";

            return allTitles;
        } 
        static ToDoTaskList FindList(string title, User user)
        {
            foreach (var item in user.ListOfLists)
            {
                if (item.Title.Equals(title))
                    return item;
            }
            return null;
        }

        static bool DeleteTaskList(ToDoTaskList list, User user) // Delete founded list
        {
            bool result = user.ListOfLists.Remove(list);
            if(result)
                UserLab.Get().ModifyUser(currentUser); // Modify singletone 
            return result;
        }
        static User AddTask(string title, string text, ToDoTaskList taskList, User user) // Add task into tasklist III stage
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
        static string GetAllTaskInList(ToDoTaskList taskList, User user) //Return list of titles All Tasks in list
        {
            //Remake from string to stringBuilder
            string result = "";
            List<ToDoTask> list = FindList(taskList.Title, currentUser).Tasks; // Debug. Get  current 
            foreach (var item in list)
                result += "Title: \"" + item.Title + "\"; Text: \"" + item.Text + "\" id: " + item.Id + "\n";
            return result;
        }
        static bool DeleteTask(ToDoTask task, ToDoTaskList taskList, User user)
        {
            bool result = taskList.Tasks.Remove(task);
            User modifyUser = ModifyUserTaskLists(taskList, user);
            if (result)
                UserLab.Get().ModifyUser(currentUser); // Modify singletone 
            return result;
        }

        static User ModifyUserTaskLists(ToDoTaskList list, User user) // Modify users one Task list
        {
            Guid lsitId = list.Id;
            for(int i = 0; i < user.ListOfLists.Count; i++)
            {
                if (user.ListOfLists[i].Id.Equals(list.Id))
                    user.ListOfLists[i] = list;
            }
            UserLab.Get().ModifyUser(user);
            return UserLab.Get().GetUser(user.Id);
        }

        static ToDoTask FindTask(string title, ToDoTaskList taskList)
        {
            foreach(var item in taskList.Tasks)
            {
                if (item.Title.Equals(title))
                    return item;
            }
            return null;
        }

        static User ModifyTask(ToDoTask task, ToDoTaskList taskList, User user) // Modify task in users taskList 
        {
            Guid taskId = task.Id;
            for(int i = 0; i < taskList.Tasks.Count; i++)
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
