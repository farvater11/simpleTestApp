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
            int currentMenuLevel = 0;
            while (true)
            {
                switch (currentMenuLevel)
                {
                    case 0:
                        bool result = false;
                        do
                        {
                            currentList = null;
                            currentTask = null;
                            currentUser = null;
                            Console.Clear();

                            HeadPartView();

                            Console.Write("LoginIn[1] or Register[2]: "); string answer0 = Console.ReadLine();
                            if (answer0.Equals("1"))
                                result = RegLogAction(false);
                            else if (answer0.Equals("2"))
                                result = RegLogAction(true);
                            else
                                result = false;
                        }
                        while (!result);
                        currentMenuLevel = 1;
                        break;
                    case 1:
                        do
                        {
                            HeadPartView();
                            //If currentUser doesn`t have listOfLists. 
                            if (currentUser.ListOfLists.Count == 0)
                                Console.WriteLine("||Info: You don`t have no one TaskList. ");
                            Console.WriteLine("  ============="+currentMenuLevel+ "/4=============  ");
                            Console.WriteLine("||Create new taskList.......(1)||");
                            Console.WriteLine("||See all your taskLists....(2)||");
                            Console.WriteLine("||Choose some taskList......(3)||");
                            Console.WriteLine("||Delete some taskList......(4)||");
                            Console.WriteLine("||Log out...................(0)||");
                            Console.WriteLine("  =============================  ");
                            Console.Write("Choose action: "); string answer1 = Console.ReadLine();
                            Console.WriteLine();
                            try
                            {
                                switch (Convert.ToInt32(answer1))
                                {
                                    case 1:
                                        CreateTaskList();
                                        break;
                                    case 2:
                                        SeeAlltasklists();
                                        break;
                                    case 3:
                                        currentMenuLevel = ChooseTaskListByName(currentMenuLevel);
                                        break;
                                    case 4:
                                        DeleteTaskListByName();
                                        break;
                                    case 0:
                                        currentMenuLevel = 0;
                                        currentUser = null;
                                        break;
                                }
                            }
                            catch(System.FormatException ex)
                            {
                                Console.WriteLine(ex.Message + "\nPlease input correct number");
                            }
                            finally
                            {
                                ExitPartView();
                            }
                        }
                        while (currentMenuLevel.Equals(1));
                        break;
                    case 2:
                        do
                        {
                            HeadPartView();
                            Console.WriteLine("  =============" + currentMenuLevel + "/4=============  ");
                            Console.WriteLine("||Rename current list.......(1)||");
                            Console.WriteLine("||Add task in list..........(2)||");
                            Console.WriteLine("||See tasks in list.........(3)||");
                            Console.WriteLine("||Choose task in list.......(4)||");
                            Console.WriteLine("||Delete some task in task..(5)||");
                            Console.WriteLine("||Return....................(0)||");
                            Console.WriteLine("  =============================  ");
                            Console.Write("Choose action: "); string answer2 = Console.ReadLine();
                            Console.WriteLine();
                            try
                            {
                                switch (Convert.ToInt32(answer2))
                                {
                                    case 1:
                                        RenameCurrentList();
                                        break;
                                    case 2:
                                        AddTaskInCurrentList();
                                        break;
                                    case 3:
                                        SeeAllTasksInCurrentList();
                                        break;
                                    case 4:
                                        currentMenuLevel = ChooseTaskInCurrentList(currentMenuLevel);
                                        break;
                                    case 5:
                                        DeleteTaskInCurrentListByName();
                                        break;
                                    case 0:
                                        currentMenuLevel = 1;
                                        currentList = null;
                                        break;
                                }
                            }
                            catch (System.FormatException ex)
                            {
                                Console.WriteLine(ex.Message + "\n\nPlease input correct number");
                            }
                            finally
                            {
                                ExitPartView();
                            }    
                        }
                        while (currentMenuLevel.Equals(2));
                        break;
                    case 3:
                        do
                        {
                            HeadPartView();
                            Console.WriteLine("  =============" + currentMenuLevel + "/4=============  ");
                            Console.WriteLine("||Modify current task.......(1)||");
                            Console.WriteLine("||See current task..........(2)||");
                            Console.WriteLine("||Return....................(0)||");
                            Console.WriteLine("  =============================  ");
                            Console.Write("Choose action: "); string answer3 = Console.ReadLine();
                            Console.WriteLine();
                            try
                            {
                                switch (Convert.ToInt32(answer3))
                                {
                                    case 1:
                                        ModifyCurrentTask();
                                        break;
                                    case 2:
                                        SeeCurrentTasks();
                                        break;
                                    case 0:
                                        currentMenuLevel = 2;
                                        currentTask = null;
                                        break;
                                }
                            }
                            catch (System.FormatException ex)
                            {
                                Console.WriteLine(ex.Message + "\n\nPlease input correct number");
                            }
                            finally
                            {
                                ExitPartView();
                            }
                        }
                        while (currentMenuLevel.Equals(3));
                        break;
                }
            }
        }

        private static void ExitPartView() //Info before returning 
        {
            Console.Write("\nFor returning press any key...\n\n"); Console.ReadKey();
        }

        private static void HeadPartView() // CLEAR CONSOLE and Print head 
        {
            Console.Clear();
            Console.WriteLine("\n  ===Hello " +
                (currentUser != null ? currentUser.Username : "Guest") + "!" +
                (currentList != null ? ("===" + currentList.Title + "/" +
                    (currentTask != null ? (currentTask.Title) : "")) : ""
            ) + "===  ");
        }

        private static void SeeCurrentTasks()//See Task
        {
            Console.WriteLine("Title: " + currentTask.Title);
            Console.WriteLine("Task: " + currentTask.Text);
            Console.WriteLine("Status: " + (currentTask.IsCompleted ? "Completed" : "Not completed"));
        }

        private static void ModifyCurrentTask()//Modify current task
        {
            Console.Write("Current title: \"" + currentTask.Title + "\". Input new title: "); string answerTitle = Console.ReadLine();
            if(!answerTitle.Equals("")) currentTask.Title = answerTitle;
            Console.Write("Current task: \"" + currentTask.Text + "\". Input new task: "); string answerTask = Console.ReadLine();
            if (!answerTask.Equals("")) currentTask.Text = answerTask;
            Console.Write("Current status: \"" + (currentTask.IsCompleted ? "Completed" : "Not completed") + "\". Input new status (+/~): "); string answerCompleted = Console.ReadLine();
            if (!answerCompleted.Equals(""))  currentTask.IsCompleted = (answerCompleted.Equals("+") ? true : false);

            currentUser = MainControl.ModifyTask(currentTask, currentList, currentUser);
            currentList = MainControl.FindList(currentList.Title, currentUser);
            currentTask = MainControl.FindTask(currentTask.Title, currentList); // Refresh. Now not useful, but in future can be useful 

            Console.WriteLine("You modified task with name \"" + currentTask.Title + "\"; and id: " + currentTask.Id);
        }

        private static void DeleteTaskInCurrentListByName()//Delete task in current taskList using name
        {
            Console.Write("Delete some task in current taskList ? (skip if don`t) (%Title%): "); string answer15 = Console.ReadLine();
            if (!answer15.Equals(""))
            {
                ToDoTask taskForDelete = MainControl.FindTask(answer15, currentList);
                if (taskForDelete != null)
                {
                    if (MainControl.DeleteTask(taskForDelete, currentList, currentUser)) // If deleted was correctly
                    {
                        Console.WriteLine("Task with name \"" + answer15 + "\" has been succesfully deleted");
                        currentUser = UserLab.Get().GetUser(currentUser.Id); // Refresh current user
                        currentList = MainControl.FindList(currentList.Title, UserLab.Get().GetUser(currentUser.Id));
                        currentTask = MainControl.FindTask(answer15, currentList);
                    }
                    else
                        Console.WriteLine("Err");
                }
                else
                    Console.WriteLine("Task with name: \"" + answer15 + "\" not found");
            }
        }

        private static int ChooseTaskInCurrentList(int currentMenuLevel) //Choose some task in current tasklist
        {
            Console.Write("Choose task in current list ? (skip if don`t)(%Title%): "); string answer14 = Console.ReadLine();
            if (!answer14.Equals(""))
            {
                currentTask = MainControl.FindTask(answer14, currentList);
                if (currentTask != null)
                {
                    Console.WriteLine("Choosen task has title: \"" + currentTask.Title + "\"; and id: " + currentTask.Id);
                    currentMenuLevel = 3;
                }
                else
                    Console.WriteLine("Task with name: \"" + answer14 + "\" not found");
            }

            return currentMenuLevel;
        }

        private static void SeeAllTasksInCurrentList() //See all tasks in list 
        {
            List<ToDoTask> list = MainControl.FindList(currentList.Title, currentUser).Tasks; // Debug. Get  current . Getting count of tasks

            Console.WriteLine("Total count of tasks " + list.Count + " :");
            Console.WriteLine(MainControl.GetAllTaskInList(currentList, currentUser));
        }

        private static void AddTaskInCurrentList()//Adding new task in current list
        {
            Console.Write("Add new task in current list ? (%TitleTask%): "); string answer12 = Console.ReadLine();
            if (!answer12.Equals(""))
            {
                Console.Write("Input new task: "); string answerNewTask = Console.ReadLine();
                currentUser = MainControl.AddTask(answer12, answerNewTask, currentList, currentUser);
                currentList = MainControl.FindList(currentList.Title, currentUser);
                ToDoTask task = MainControl.FindTask(answer12, currentList);
                Console.WriteLine("\nIn tasklist \"" + currentList.Title + "\" succesfully has been added new task \"" + task.Title + "\"");
            }
        }

        private static void RenameCurrentList()//Modify tasklist
        {
            Console.Write("Current title: \"" + currentList.Title + "\"; Input new title: "); string answerTitle = Console.ReadLine();
            currentList.Title = answerTitle;

            currentUser = MainControl.ModifyUserTaskLists(currentList, currentUser);
            currentList = MainControl.FindList(currentList.Title, currentUser); // Refresh.

            Console.WriteLine("You modified list with name \"" + currentList.Title + "\"; and id: " + currentList.Id);
        }

        private static void DeleteTaskListByName()//Delete tasklist  using name 
        {
            Console.Write("Delete some taskList ?  (%Title%): "); string answer14 = Console.ReadLine();
            if (!answer14.Equals(""))
            {
                if (MainControl.DeleteTaskList(MainControl.FindList(answer14, currentUser), currentUser)) // If deleted was correctly
                {
                    Console.WriteLine("List with name \"" + answer14 + "\" has been succesfully deleted");
                    currentUser = UserLab.Get().GetUser(currentUser.Id); // Refresh current user
                    if (currentList != null)
                    {
                        if (answer14 == currentList.Title) // If deleted list with current name has been deleted, refresh current list 
                            currentList = null;
                    }
                }
                else
                    Console.WriteLine("Err");
            }
            else
                Console.WriteLine("List with name: \"" + answer14 + "\" not found");
        }

        private static int ChooseTaskListByName(int currentMenuLevel) //Choose some taskList by name
        {
            Console.Write("Choose some taskList? (%Title%): "); string answer13 = Console.ReadLine();
            if (!answer13.Equals(""))
            {
                currentList = MainControl.FindList(answer13, currentUser);
                if (currentList != null)
                {
                    Console.WriteLine("Choosen list has name: \"" + currentList.Title + "\"; and id: " + currentList.Id);
                    currentMenuLevel = 2;
                }
                else
                    Console.WriteLine("List with name: \"" + answer13 + "\" not found");
            }
            return currentMenuLevel;
        }

        private static void SeeAlltasklists()//See all tasklists
        {
            Console.WriteLine("Total count of lists " + currentUser.ListOfLists.Count + " :");
            Console.WriteLine(MainControl.GetTitlesOfAllTaskLists(currentUser));
        }
            
        private static void CreateTaskList()//Create new list
        {
            Console.Write("Create new taskList? (%Title%): "); string answer11 = Console.ReadLine();
            if (!answer11.Equals(""))
            {
                currentUser = MainControl.AddToUserListOfList(answer11, currentUser); // Add new List to ListOfList and refresh currentUser link 
                Console.WriteLine("Task List \"" + currentUser.ListOfLists.Last().Title + "\" has been created ");
            }
        }

        private static bool RegLogAction(bool newUser) //Registration or loginin user
        {
            if (newUser)
            {
                Console.Write("Username: "); string username = Console.ReadLine();
                Console.Write("First name: "); string firstName = Console.ReadLine();
                Console.Write("Last name: "); string lastName = Console.ReadLine();
                Console.Write("Password: "); string password = Console.ReadLine();

                currentUser = MainControl.UserRegistration(username, firstName, lastName, password);
                if (currentUser != null)
                    Console.WriteLine("\nYou are successfully registered!");
                else
                    Console.WriteLine("\nUser already does exist");
                Console.ReadKey();
                return false;
            }
            // LoginIn
            else
            {
                Console.Write("Username: "); string username = Console.ReadLine();
                Console.Write("Password: "); string password = Console.ReadLine();
                currentUser = MainControl.UserLoginIn(username, password);
                if (currentUser == null)
                {
                    Console.Write("\n User not founded ");
                    Console.ReadKey();
                    return false;
                }
                return true;
            }
        }
    } 
}


