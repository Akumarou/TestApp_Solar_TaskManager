using System;
using System.Collections.Generic;

namespace TestApp_Solar_TaskManager
{
    interface IUI
    {
        bool addNewTask(string v, DateTime dateTime);
        bool addNewTasks(List<Task> lists);
        bool clearTaskManager();
        bool deleteTaskbyId(int v);
        bool deleteTasksByDate(DateTime dateTime);
        bool deleteTasksbyTask(string v);
        bool editByDate(DateTime dateTime, List<string> lists);
        bool editById(int v1, string v2);
        bool editByTask(string v, List<string> lists);
        DateTime getDateFromUser();
        int getIntFromUser();
        string getStringFromUser();
        bool print(string toPrint);
        bool printTasks();
        bool setNewTaskManager(ITaskManager tm);
    }
}