using System;
using System.Collections.Generic;
using System.IO;

namespace TestApp_Solar_TaskManager
{
    class Program
    {
        static void Main()
        {
            IUI UI = new ConsoleUI(new TaskManager());
            UI.print("Инициализация пустого конструктора");
            UI.printTasks();

            UI.addNewTask("Задача 1", DateTime.Parse("2020/02/15"));
            UI.print("Добавление задачи");
            UI.printTasks();

            UI.addNewTasks(new List<Task> {
                            new Task(DateTime.Parse("2020/02/14"), "Задача 2"),
                            new Task(DateTime.Parse("2020/02/13"), "Задача 3"),
                            new Task(DateTime.Parse("2020/02/12"), "Задача 4"),
                            new Task(DateTime.Parse("2020/02/15"), "Задача 4")
                            });
            UI.print("Добавление набора задач");
            UI.printTasks();

            UI.deleteTaskbyId(0);
            UI.print("Уделние по id");
            UI.printTasks();
            UI.deleteTasksbyTask("Задача 3");
            UI.print("Уделние по тексту задачи");
            UI.printTasks();
            UI.deleteTasksByDate(DateTime.Parse("2020/02/15"));
            UI.print("Уделние по дате");
            UI.printTasks();

            UI.setNewTaskManager(new TaskManager(new List<Task> {
                            new Task(DateTime.Parse("2020/02/11"), "Задача 1"),
                            new Task(DateTime.Parse("2020/02/14"), "Задача 2"),
                            new Task(DateTime.Parse("2020/02/14"), "Задача 3"),
                            new Task(DateTime.Parse("2020/02/13"), "Задача 3"),
                            new Task(DateTime.Parse("2020/02/17"), "Задача 3"),
                            new Task(DateTime.Parse("2020/02/20"), "Задача 4"),
                            new Task(DateTime.Parse("2020/02/15"), "Задача 5"),
                            new Task(DateTime.Parse("2020/02/21"), "Задача 6"),
                            new Task(DateTime.Parse("2020/02/21"), "Задача 7")
                            }));

            UI.print("Инициализация конструктора с набором задач");
            UI.printTasks();

            UI.editById(0, "Задача с id");
            UI.print("Редактирование по id");
            UI.printTasks();

            UI.editByTask("Задача 2", new List<string> { "Задача с текстом - 1 вхождение" });
            UI.print("Редактирование по тексту задачи (1 вхождение, 1 изменение)");
            UI.printTasks();

            UI.editByTask("Задача 3", new List<string> { "Задача с текстом - несколько вхождений, 1 изменение" });
            UI.print("Редактирование по тексту задачи (несколько вхождений, 1 изменение)");
            UI.printTasks();

            UI.editByTask("Задача 3", new List<string> { "Задача с текстом - несколько вхождений, несколько изменений",
                                                         "Задача с текстом - несколько вхождений, несколько изменений"});
            UI.print("Редактирование по тексту задачи (несколько вхождений, несколько изменений)");
            UI.printTasks();

            UI.editByDate(DateTime.Parse("2020/02/20"), new List<string> { "Задача с датой - 1 вхождение, 1 изменение" });
            UI.print("Редактирование по дате (1 вхождение, 1 изменение)");
            UI.printTasks();

            UI.editByDate(DateTime.Parse("2020/02/15"), new List<string> { "Задача с датой - несколько вхождений, 1 изменение" });
            UI.print("Редактирование по дате (несколько вхождений, 1 изменение)");
            UI.printTasks();

            UI.editByDate(DateTime.Parse("2020/02/21"), new List<string> { "Задача с датой - несколько вхождений, несколько изменений",
                                                                           "Задача с датой - несколько вхождений, несколько изменений" });
            UI.print("Редактирование по дате (несколько вхождений, несколько изменений)");
            UI.printTasks();

            UI.clearTaskManager();
            UI.print("Очистка списка задач");
            UI.printTasks();

            Console.ReadKey();
        }
    }
}
