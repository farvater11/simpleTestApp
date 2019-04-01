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

        static void Main(string[] args)
        {
        labelStart:
            bool newUser; // Mark for registration action
            currentList = null;
            Console.Clear();
            Console.WriteLine("==Task-list system==\n====================");

        labelAfterStart:

        //Remake to oneLine-code or reccursion
        labelNotCorrectChoose:
            Console.Write("LoginIn[1] or Register[2] ");
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
            Console.Write("\nDo you want to craate? (Yes/No): "); string answer = Console.ReadLine();
            if (answer.Equals("Yes"))
            {
                Console.Write("Title: "); string answerTitle = Console.ReadLine();
                currentUser = AddToUserListOfList(answerTitle, currentUser); // Add new List to ListOfList and refresh currentUser link 
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
            Console.Write("\nDo you want to choose some taskList? (%Title%): "); string answerTitle1 = Console.ReadLine();
            currentList = FindList(answerTitle1, currentUser);
            if(currentList != null)
                Console.WriteLine("Choosen list has name: \"" + currentList.Title + "\"; and id: " + currentList.Id);
            else
                Console.WriteLine("List with name: \"" + answerTitle1 + "\" not found");

            //Delete tasklist  using name 
            Console.Write("\nDo you want to delete some taskList ? (skip if don`t) (%Title%): "); string answer2 = Console.ReadLine();
            if(answer2.Equals(""))
                Console.WriteLine("");
            else if (DeleteTaskList(FindList(answer2, currentUser), currentUser)) // If deleted was correctly
            {
                Console.WriteLine("List with name " + answer2 + " has been succesfully deleted"); 
                currentUser = UserLab.Get().GetUser(currentUser.Id); // Refresh current user
                if(currentList != null)
                {
                    if (answer2 == currentList.Title) // If deleted list with current name has been deleted, refresh current list 
                        currentList = null;
                }
            }
            else
                Console.WriteLine("Err");

            //Adding new task in current list
            if (currentList != null)
            {
                Console.Write("\nDo you want to add some task in current list ? (skip if don`t)(%Text%)"); string answer3 = Console.ReadLine();
                if (answer3.Equals(""))
                    Console.WriteLine("");
                else
                {
                    currentUser = AddTask(answer3, currentList, currentUser);
                    currentList = FindList(currentList.Title, currentUser);
                }
            }

            //See all tasks in list 
            Console.Write("\nDo you want to see tasks in current list ? (Yes/No): "); string answer4 = Console.ReadLine();
            if (answer1.Equals("Yes"))
            {
                List<ToDoTask> list = FindList(currentList.Title, currentUser).Tasks; // Debug. Get  current . Getting count of tasks

                Console.WriteLine("Total count of tasks " + list.Count);
                Console.WriteLine(GetAllTeskInList(currentList, currentUser));
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
                allTitles += item.Title + "\n";

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
        static User AddTask(string text, ToDoTaskList taskList, User user) // Add task into tasklist 
        {
            ToDoTask task = new ToDoTask(text);

            ToDoTaskList newList = taskList;
            newList.Tasks.Add(task);

            //Insert in user list in correct index. Replace
            user.ListOfLists[user.ListOfLists.IndexOf(taskList)] = newList;

            UserLab.Get().ModifyUser(user);
            return UserLab.Get().GetUser(user.Id);
        }
        static string GetAllTeskInList(ToDoTaskList taskList, User user) //Finding task in list
        {
            //Remake from string to stringBuilder
            string result = "";
            List<ToDoTask> list = FindList(taskList.Title, currentUser).Tasks; // Debug. Get  current 
            foreach (var item in list)
                result += "Text: \"" + item.Text + "\" id:" + item.Text + "\n";
            return result;
        }
    } 
}
