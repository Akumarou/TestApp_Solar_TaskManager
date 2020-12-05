using System;

namespace TestApp_Solar_TaskManager
{
    interface ITask
    {
        DateTime Date { get; set; }
        string The_task { get; set; }

        string ToString();
    }
}