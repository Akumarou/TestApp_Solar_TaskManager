using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TestApp_Solar_TaskManager.classes.task_manager;
using TestApp_Solar_TaskManager.classes.UI_impl.db_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.IO_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.menus;
using TestApp_Solar_TaskManager.classes.exceptions;

namespace TestApp_Solar_TaskManager.classes.UI_impl
{
    class UI_impl
    {
        DB_impl DB;
        Task_manager_impl tm;
        ConsoleIO_impl IO;

        public UI_impl(DB_impl dB, Task_manager_impl tm, ConsoleIO_impl iO)
        {
            DB = dB ?? throw new ArgumentNullException(nameof(dB));
            this.tm = tm ?? throw new ArgumentNullException(nameof(tm));
            IO = iO ?? throw new ArgumentNullException(nameof(iO));
        }
        public void start()
        {
            getMainMenu();
        }

        public void getMainMenu()
        {
            IO.clear();
            string menu = "Для начала работы выберите действие:\n"
                + "1)Создать новый список задач\n"
                + "2)Открыть список задач\n"
                + "[Выйти - esc]";
            IO.print("Добро пожаловать!\n" + menu);
            ConsoleKeyInfo cki;
            int answer = -1;
            do
            {
                cki = IO.getKeyFromUser();
                if (cki.Key == ConsoleKey.Escape) break;
                bool v = int.TryParse(cki.Key.ToString().Substring(1), out answer);
                if (!v || answer < 1 || answer > 2)
                { IO.clear(); IO.print("Ошибка! Неверное значение.\n" + menu); }
            } while (answer < 1 || answer > 2);
            switch (answer)
            {
                case 1:
                    createNewTable();
                    break;
                case 2:
                    openTable();
                    break;
                default:
                    closeApp();
                    break;
            }
        }

        private void closeApp()
        {
            Process.GetCurrentProcess().Kill();
        }

        public void createNewTable()
        {
            try
            {
                dbManagement.createNewTable(DB, IO);
            }
            catch (ProcessToShowTable)
            {
                showTable();
            }
            catch (ReturnToTableMenu)
            {
                getTableMenu();
            }
            catch (ReturnToMainMenu)
            {
                getMainMenu();
            }

        }

        public void openTable()
        {
            try
            {
                dbManagement.openTable(DB, IO);
            }
            catch (ReturnToTableMenu)
            {
                getTableMenu();
            }
            catch (ReturnToMainMenu)
            {
                getMainMenu();
            }
        }

        public void deleteTable()
        {
            try
            {
                dbManagement.deleteTable(DB, IO);
            }
            catch (ReturnToTableMenu)
            {
                getTableMenu();
            }
            catch (ReturnToMainMenu)
            {
                getMainMenu();
            }
        }

        private void getTableMenu()
        {
            try
            {
                tableManagement.getTableMenu(DB, IO);
            }
            catch (ProcessToDeleteTable)
            {
                deleteTable();
            }
            catch (ReturnToMainMenu)
            {
                getMainMenu();
            }
            catch (ProcessToShowTable)
            {
                showTable();
            }
            catch (ProcessToEditTable)
            {
                editTable();
            }
        }

        private void showTable()
        {
            try
            {
                tableManagement.showTable(DB,tm, IO);
            }
            catch (ProcessToSortTable)
            {
                sortTable();
            }
            catch (ProcessToFindInTable)
            {
                findInTable();
            }
            catch (ReturnToTableMenu)
            {
                getTableMenu();
            }
        }
        private void editTable()
        {
            try
            {
                tableManagement.editTable(DB, IO);
            }
            catch (ProcessToAddTaskToTable)
            {
                addTaskToTable();
            }
            catch (ProcessToEditTasksAtTable)
            {
                editTasksAtTable();
            }
            catch (ProcessToDeleteTasksAtTable)
            {
                deleteTasksFromTable();
            }
            catch (ReturnToTableMenu)
            {
                getTableMenu();
            }
        }
        private void addTaskToTable()
        {
            try
            {
                tableManagement.addTaskToTable(DB,tm, IO);
            }
            catch (ProcessToAddTaskToTable)
            {
                addTaskToTable();
            }
            catch (ProcessToShowTable)
            {
                showTable();
            }
        }

        private void sortTable()
        {
            try
            {
                tableManagement.sortTable(DB, tm, IO);
            }
            catch (ProcessToShowTable)
            {
                showTable();
            }
        }



        private void deleteTasksFromTable()
        {
            try
            {
                tableManagementDelete.deleteTasksFromTable(DB,tm, IO);
            }
            catch (ProcessToShowTable)
            {
                showTable();
            }
            catch (ProcessToEditTable)
            {
                editTable();
            }
            catch (ReturnToTableMenu)
            {
                getTableMenu();
            }
        }

        private void editTasksAtTable()
        {
            try
            {
                tableManagementEdit.editTasksAtTable(DB, tm, IO);
            }
            catch (ProcessToShowTable)
            {
                showTable();
            }
            catch (ProcessToEditTable)
            {
                editTable();
            }
            catch (ReturnToTableMenu)
            {
                getTableMenu();
            }
        }

        private void findInTable()
        {
            try
            {
                tableManagementFind.findInTable(DB, tm, IO);
            }
            catch (ProcessToShowTable)
            {
                showTable();
            }
        }
        
    }
}
