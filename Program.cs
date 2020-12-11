using System;
using System.Collections.Generic;
using TestApp_Solar_TaskManager.classes.task_manager;
using TestApp_Solar_TaskManager.classes.UI_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.db_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.IO_impl;

namespace TestApp_Solar_TaskManager
{
    class Program
    {
        static void Main()
        {
            UI_impl UI = new UI_impl(new DB_impl(), new Task_manager_impl(), new ConsoleIO_impl());
            UI.start();
        }
    }
}
