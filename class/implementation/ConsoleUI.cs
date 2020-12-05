using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestApp_Solar_TaskManager
{
    class ConsoleUI : IUI
    {
        private ITaskManager tm;

        public ConsoleUI(ITaskManager tm)
        {
            this.tm = tm;
        }
        public bool setNewTaskManager(ITaskManager tm)
        {
            this.tm = tm;
            return true;
        }

        public DateTime getDateFromUser()
        {
            Console.Write("Введите дату: ");
            string date_string = Console.ReadLine();
            date_string = date_string.Replace(".", "/").Replace(",", "/")
                .Replace(":", "/").Replace(";", "/").Replace("-", "/");
            if (new Regex(@"\d{2,2}/\d{2,2}/\d{4,4}").IsMatch(date_string))
                date_string = date_string.Substring(6) + "/" + date_string.Substring(3, 5) + "/" + date_string.Substring(0, 2);
            DateTime date = new DateTime();
            if (!(new Regex(@"\d{4,4}/\d{2,2}/\d{2,2}").IsMatch(date_string))
                || !DateTime.TryParse(date_string, out date))
            {
                Console.WriteLine("Неверный формат даты");
                getDateFromUser();
            }
            return date;
        }

        public int getIntFromUser()
        {
            Console.Write("Введите число: ");
            bool v = int.TryParse(Console.ReadLine(), out int num);
            while (!v)
            {
                Console.Write("Неверное значение.\nВведите число: ");
                v = int.TryParse(Console.ReadLine(), out num);
            }
            return num;
        }

        public string getStringFromUser()
        {
            Console.Write("Введите значение: ");
            return Console.ReadLine();
        }

        public bool printTasks()
        {
            Console.WriteLine("Ваш список задач:\n" + tm.ToString());
            return true;
        }

        public bool print(string toPrint)
        {
            Console.WriteLine(toPrint);
            return true;
        }

        public bool addNewTask(string v, DateTime dateTime)
        {
            tm.addNewTask(v, dateTime);
            return true;
        }


        public bool addNewTasks(List<Task> lists)
        {
            tm.addNewTasks(lists);
            return true;
        }


        public bool deleteTaskbyId(int v)
        {
            tm.deleteTaskbyId(v);
            return true;
        }

        public bool deleteTasksbyTask(string v)
        {
            tm.deleteTasksbyTask(v);
            return true;
        }

        public bool deleteTasksByDate(DateTime dateTime)
        {
            tm.deleteTasksByDate(dateTime);
            return true;
        }

        public bool editById(int v1, string v2)
        {
            tm.editById(v1, v2);
            return true;
        }

        public bool editByTask(string v, List<string> lists)
        {
            tm.editByTask(v, lists);
            return true;
        }

        public bool editByDate(DateTime dateTime, List<string> lists)
        {
            tm.editByDate(dateTime, lists);
            return true;
        }

        public bool clearTaskManager()
        {
            tm.clearTaskManager();
            return true;
        }
    }
}
