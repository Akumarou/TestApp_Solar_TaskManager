using System;
using System.Collections.Generic;
using System.IO;

namespace TestApp
{
    class Program
    {
        static void Main()
        {
            ITaskManager taskManager = new TaskManager();
            Console.WriteLine("Initialize by empty constructor\nTask List:\n" +
                taskManager.ToString());

            taskManager.addNewTask("fuck", DateTime.Parse("2020/02/15"));
            Console.WriteLine("Adding singular value\nTask List:\n" +
                taskManager.ToString());

            taskManager.addNewTasks(new List<Task> {
                new Task(DateTime.Parse("2020/02/14"), "fuck1"),
                new Task(DateTime.Parse("2020/02/13"), "fuck2"),
                new Task(DateTime.Parse("2020/02/12"), "fuck2"),
                new Task(DateTime.Parse("2020/02/15"), "fuck4")
                });
            Console.WriteLine("Adding pluar value \nTask List:\n" + 
                taskManager.ToString());

            taskManager.deleteTaskbyId(0);
            Console.WriteLine("Deleting by id \nTask List:\n" +
                taskManager.ToString());
            taskManager.deleteTasksbyTask("fuck2");
            Console.WriteLine("Deleting by task \nTask List:\n" +
                taskManager.ToString());
            taskManager.deleteTasksByDate(DateTime.Parse("2020/02/15"));
            Console.WriteLine("Deleting by date \nTask List:\n" +
                taskManager.ToString());

            taskManager = new TaskManager(new List<Task> {
                new Task(DateTime.Parse("2020/02/15"), "fuck"),
                new Task(DateTime.Parse("2020/02/14"), "fuck1"),
                new Task(DateTime.Parse("2020/02/13"), "fuck2"),
                new Task(DateTime.Parse("2020/02/12"), "fuck2"),
                new Task(DateTime.Parse("2020/02/18"), "fuck2"),
                new Task(DateTime.Parse("2020/02/15"), "fuck4")
                });
            Console.WriteLine("Initialize by NOT empty constructor\nTask List:\n" + 
                taskManager.ToString());

            taskManager.editById(0,"fuck000");
            Console.WriteLine("Edit by id\nTask List:\n" +
                taskManager.ToString());

            taskManager.editByTask("fuck1", new List<string> { "fuck111" });
            Console.WriteLine("Edit by singular task\nTask List:\n" +
                taskManager.ToString());

            taskManager.editByTask("fuck2", new List<string> { "fuck222" });
            Console.WriteLine("Edit by plural task\nTask List:\n" +
                taskManager.ToString()); 
            taskManager.editByTask("fuck2", new List<string> { "fuck222","fuck222" ,"fuck222" });
            Console.WriteLine("Edit by plural tasks\nTask List:\n" +
                taskManager.ToString());

            taskManager.editByDate(DateTime.Parse("2020/02/14"), new List<string> { "date" });
            Console.WriteLine("Edit by singular date\nTask List:\n" +
                taskManager.ToString());

            taskManager.editByDate(DateTime.Parse("2020/02/15"), new List<string> { "date111" });
            Console.WriteLine("Edit by plural date\nTask List:\n" +
                taskManager.ToString());

            taskManager.clearTaskManager();
            Console.WriteLine("Removing all data\nTask List:\n" +
                taskManager.ToString());
        }
    }
}
