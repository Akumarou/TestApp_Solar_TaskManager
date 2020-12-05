using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp_Solar_TaskManager
{
    class TaskManager : ITaskManager
    {
        private List<Task> tasks;

        public TaskManager()
        {
            this.tasks = new List<Task>();
        }

        public TaskManager(List<Task> tasks)
        {
            this.tasks = tasks;
        }

        public Task getTaskById(int id)
        {
            Task temp = new Task();
            try
            {
                temp = tasks[id];
            }
            catch (Exception)
            {

                throw new InvalidTaskId();
            }
            return temp;
        }
        public List<Task> getTasksByTask(string task)
        {
            List<Task> temp = new List<Task>();
            for (int i = 0; i < tasks.Count; i++)
                if (tasks[i].The_task.Contains(task))
                    temp.Add(tasks[i]);
            return temp;
        }
        public List<Task> getTasksByDate(DateTime time)
        {
            List<Task> temp = new List<Task>();
            for (int i = 0; i < tasks.Count; i++)
                if (tasks[i].Date.Equals(time))
                    temp.Add(tasks[i]);
            return temp;
        }
        public bool addNewTask(string task, DateTime time)
        {
            tasks.Add(new Task(time, task));
            return true;
        }
        public bool addNewTasks(List<Task> newTasks)
        {
            tasks.AddRange(newTasks);
            return true;
        }
        public List<Task> deleteTasksByDate(DateTime date)
        {
            List<Task> temp = getTasksByDate(date);
            temp.ForEach(e => tasks.Remove(e));
            return temp;
        }
        public List<Task> deleteTasksbyTask(string task)
        {
            List<Task> temp = getTasksByTask(task);
            temp.ForEach(e => tasks.Remove(e));
            return temp;
        }
        public Task deleteTaskbyId(int id)
        {
            Task temp = getTaskById(id);
            tasks.Remove(temp);
            return temp;
        }

        public bool editById(int id, string task, DateTime time)
        {
            Task temp = getTaskById(id);
            temp.Date = time;
            temp.The_task = task;
            return true;
        }
        public bool editById(int id, DateTime time)
        {
            Task temp = getTaskById(id);
            temp.Date = time;
            return true;
        }
        public bool editById(int id, string task)
        {
            Task temp = getTaskById(id);
            temp.The_task = task;
            return true;
        }
        public bool editByTask(string old_task, List<string> task, List<DateTime> time)
        {
            List<Task> temp = getTasksByTask(old_task);
            if (temp.Count >= task.Count && temp.Count >= time.Count)
            {
                for (int i = 0; i < task.Count; i++)
                {
                    temp[i].The_task = task[i];
                }

                for (int i = 0; i < time.Count; i++)
                {
                    temp[i].Date = time[i];
                }
            }
            return true;
        }
        public bool editByTask(string old_task, List<string> task)
        {
            List<Task> temp = getTasksByTask(old_task);
            if (temp.Count >= task.Count)
                for (int i = 0; i < task.Count; i++)
                {
                    temp[i].The_task = task[i];
                }

            return true;
        }
        public bool editByTask(string old_task, List<DateTime> time)
        {
            List<Task> temp = getTasksByTask(old_task);
            if (temp.Count >= time.Count)
                for (int i = 0; i < time.Count; i++)
                {
                    temp[i].Date = time[i];
                }
            return true;
        }
        public bool editByDate(DateTime old_time, List<string> task, List<DateTime> time)
        {
            List<Task> temp = getTasksByDate(old_time);
            if (temp.Count >= task.Count && temp.Count >= time.Count)
            {
                for (int i = 0; i < task.Count; i++)
                {
                    temp[i].The_task = task[i];
                }

                for (int i = 0; i < time.Count; i++)
                {
                    temp[i].Date = time[i];
                }
            }
            return true;
        }
        public bool editByDate(DateTime old_time, List<DateTime> time)
        {
            List<Task> temp = getTasksByDate(old_time);
            if (temp.Count >= time.Count)
                for (int i = 0; i < time.Count; i++)
                {
                    temp[i].Date = time[i];
                }
            return true;
        }
        public bool editByDate(DateTime old_time, List<string> task)
        {
            List<Task> temp = getTasksByDate(old_time);
            if (temp.Count >= task.Count)
                for (int i = 0; i < task.Count; i++)
                {
                    temp[i].The_task = task[i];
                }

            return true;
        }

        public List<Task> clearTaskManager()
        {
            List<Task> temp = tasks;
            tasks = new List<Task>();
            return temp;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            if (tasks != null)
                for (int i = 0; i < tasks.Count; i++)
                    result.Append(i + ") " + tasks[i].ToString() + "\n");
            return result.ToString().Equals("") ? "Список задач пуст" : result.ToString();
        }
    }
}
