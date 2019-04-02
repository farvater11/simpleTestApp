using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleTestApp.Model;
using SimpleTestApp.Control;

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
                currentUser = MainControl.UserRegistration();
                if(currentUser != null) 
                    Console.WriteLine("\nYou are successfully registered!");
                else
                    Console.WriteLine("\nUser already does exist");
            }
            // LoginIn
            else
            {
                currentUser = MainControl.UserLoginIn();
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
                currentUser = MainControl.AddToUserListOfList(answer, currentUser); // Add new List to ListOfList and refresh currentUser link 
                Console.WriteLine("Task List \"" + currentUser.ListOfLists.Last().Title + "\" has been created ");
            }

            //See all tasklists
            Console.Write("\nDo you want to see all your taskLists? (Yes/No): "); string answer1 = Console.ReadLine();
            if (answer1.Equals("Yes"))
            {
                Console.WriteLine("Total count of lists " + currentUser.ListOfLists.Count + " :");
                Console.WriteLine(MainControl.GetTitlesOfAllTaskLists(currentUser));
            }

            //Choose some taskList by name
            Console.Write("\nDo you want to choose some taskList? (skip if don`t)(%Title%): "); string answerTitle1 = Console.ReadLine();
            if (!answerTitle1.Equals(""))
            {
                currentList = MainControl.FindList(answerTitle1, currentUser);
                if (currentList != null)
                    Console.WriteLine("Choosen list has name: \"" + currentList.Title + "\"; and id: " + currentList.Id);
                else
                    Console.WriteLine("List with name: \"" + answerTitle1 + "\" not found");
            }

            //Delete tasklist  using name 
            Console.Write("\nDo you want to delete some taskList ? (skip if don`t) (%Title%): "); string answer2 = Console.ReadLine();
            if (!answer2.Equals(""))
            {
                if (MainControl.DeleteTaskList(MainControl.FindList(answer2, currentUser), currentUser)) // If deleted was correctly
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

                    currentUser = MainControl.ModifyUserTaskLists(currentList, currentUser);
                    currentList = MainControl.FindList(currentList.Title, currentUser); // Refresh. Now not useful, but in future can be useful 

                    Console.WriteLine("You modified list with name \"" + currentList.Title + "\"; and id: " + currentList.Id);
                }


                //Adding new task in current list
                Console.Write("\nDo you want to add some task in current list ? (skip if don`t)(%Title%): "); string answer3 = Console.ReadLine();
                if (!answer3.Equals(""))
                {
                    Console.Write("Input new task: "); string answer7 = Console.ReadLine();
                    currentUser = MainControl.AddTask(answer3, answer7, currentList, currentUser);
                    currentList = MainControl.FindList(currentList.Title, currentUser);
                    ToDoTask task = MainControl.FindTask(answer3, currentList);
                    Console.WriteLine("\nIn tasklist \"" + currentList.Title + "\" succesfully has been added new task \"" + task.Title + "\"");
                }


                //See all tasks in list 
                Console.Write("\nDo you want to see tasks in current list ? (Yes/No): "); string answer4 = Console.ReadLine();
                if (answer4.Equals("Yes"))
                {
                    List<ToDoTask> list = MainControl.FindList(currentList.Title, currentUser).Tasks; // Debug. Get  current . Getting count of tasks

                    Console.WriteLine("Total count of tasks " + list.Count + " :");
                    Console.WriteLine(MainControl.GetAllTaskInList(currentList, currentUser));
                }


                //Choose some task in current tasklist
                Console.Write("\nDo you want to choose task in current list ? (skip if don`t)(%Title%): "); string answer6 = Console.ReadLine();
                if (!answer6.Equals(""))
                {
                    currentTask = MainControl.FindTask(answer6, currentList);
                    if (currentTask != null)
                        Console.WriteLine("Choosen task has title: \"" + currentTask.Title + "\"; and id: " + currentTask.Id);
                    else
                        Console.WriteLine("Task with name: \"" + answer6 + "\" not found");
                }

                //Delete task in current taskList using name 
                Console.Write("\nDo you want to delete some task in current taskList ? (skip if don`t) (%Title%): "); string answer8 = Console.ReadLine();
                if (!answer8.Equals(""))
                {
                    ToDoTask taskForDelete = MainControl.FindTask(answer8, currentList);
                    if(taskForDelete != null)
                    {
                        if (MainControl.DeleteTask(taskForDelete, currentList, currentUser)) // If deleted was correctly
                        {
                            Console.WriteLine("Task with name \"" + answer8 + "\" has been succesfully deleted");
                            currentUser = UserLab.Get().GetUser(currentUser.Id); // Refresh current user
                            currentList = MainControl.FindList(currentList.Title, UserLab.Get().GetUser(currentUser.Id));
                            currentTask = MainControl.FindTask(answer8, currentList);
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

                        currentUser = MainControl.ModifyTask(currentTask, currentList, currentUser);
                        currentList = MainControl.FindList(currentList.Title, currentUser);
                        currentTask = MainControl.FindTask(currentTask.Title, currentList); // Refresh. Now not useful, but in future can be useful 

                        Console.WriteLine("You modified task with name \"" + currentTask.Title + "\"; and id: " + currentTask.Id);
                    }
                }
            }
            
            





            goto labelAfterStart;
            Console.ReadKey();
        }

    } 
}
