using System;
using System.Collections.Generic;

namespace TestApp_Solar_TaskManager
{
    interface ITaskManager
    {
        bool addNewTask(string task, DateTime time);
        bool addNewTasks(List<Task> newTasks);
        List<Task> clearTaskManager();
        Task deleteTaskbyId(int id);
        List<Task> deleteTasksByDate(DateTime date);
        List<Task> deleteTasksbyTask(string task);
        bool editByDate(DateTime old_time, List<DateTime> time);
        bool editByDate(DateTime old_time, List<string> task);
        bool editByDate(DateTime old_time, List<string> task, List<DateTime> time);
        bool editById(int id, DateTime time);
        bool editById(int id, string task);
        bool editById(int id, string task, DateTime time);
        bool editByTask(string old_task, List<DateTime> time);
        bool editByTask(string old_task, List<string> task);
        bool editByTask(string old_task, List<string> task, List<DateTime> time);
        Task getTaskById(int id);
        List<Task> getTasksByDate(DateTime time);
        List<Task> getTasksByTask(string task);
        string ToString();
    }
}