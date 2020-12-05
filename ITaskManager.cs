using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    interface ITaskManager
    {
        public Task getTaskById(int id);
        public List<Task> getTasksByTask(string task);
        public List<Task> getTasksByDate(DateTime time);

        public bool addNewTask(string task, DateTime time);
        public bool addNewTasks(List<Task> newTasks);

        public List<Task> deleteTasksByDate(DateTime date);
        public List<Task> deleteTasksbyTask(string task);
        public Task deleteTaskbyId(int id);

        public bool editById(int id, string task, DateTime time);
        public bool editById(int id, DateTime time);
        public bool editById(int id, string task);

        public bool editByTask(string old_task, List<string> task, List<DateTime> time);
        public bool editByTask(string old_task, List<string> task);
        public bool editByTask(string old_task, List<DateTime> time);

        public bool editByDate(DateTime old_time, List<string> task, List<DateTime> time);
        public bool editByDate(DateTime old_time, List<DateTime> time);
        public bool editByDate(DateTime old_time, List<string> task);

        public List<Task> clearTaskManager();

    }
}
