using System;
using System.Text.RegularExpressions;
using TestApp_Solar_TaskManager.classes.task_manager;
using TestApp_Solar_TaskManager.classes.UI_impl.db_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.IO_impl;
using TestApp_Solar_TaskManager.classes.exceptions;

namespace TestApp_Solar_TaskManager.classes.UI_impl.menus
{
    class tableManagementDelete
    {
        public static void deleteTasksFromTable(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Выберите действие:\n"
                + "1)Удаление по номеру\n"
                + "2)Удаление по тексту\n"
                + "3)Удаление по дате\n"
                + "4)Удаление по статусу\n"
                + "[Назад - esc]";
            IO.print(menu);
            ConsoleKeyInfo cki;
            int answer = -1;
            do
            {
                cki = IO.getKeyFromUser();
                if (cki.Key == ConsoleKey.Escape) break;
                bool v = int.TryParse(cki.Key.ToString().Substring(1), out answer);
                if (!v || answer < 1 || answer > 4)
                { IO.clear(); IO.print("Ошибка! Неверное значение.\n" + menu); }
            } while (answer < 1 || answer > 4);
            IO.clear();
            switch (answer)
            {
                case 1:
                    try
                    {
                        deleteTasksFromTableById(DB,tm, IO);
                    }
                    catch (ProcessToShowTable)
                    {
                        throw new ProcessToShowTable();
                    }
                    catch (ProcessToEditTable)
                    {
                        throw new ProcessToEditTable();
                    }
                    catch (ReturnToTableMenu)
                    {
                        throw new ReturnToTableMenu();
                    }
                    break;
                case 2:
                    try
                    {
                        deleteTasksFromTableByText(DB, tm, IO);
                    }
                    catch (ProcessToShowTable)
                    {
                        throw new ProcessToShowTable();
                    }
                    catch (ProcessToEditTable)
                    {
                        throw new ProcessToEditTable();
                    }
                    break;
                case 3:
                    try
                    {
                        deleteTasksFromTableByDate(DB, tm, IO);
                    }
                    catch (ProcessToShowTable)
                    {
                        throw new ProcessToShowTable();
                    }
                    catch (ProcessToEditTable)
                    {
                        throw new ProcessToEditTable();
                    }
                    break;
                case 4:
                    try
                    {
                        deleteTasksFromTableByStatus(DB, tm, IO);
                    }
                    catch (ProcessToShowTable)
                    {
                        throw new ProcessToShowTable();
                    }
                    catch (ProcessToEditTable)
                    {
                        throw new ProcessToEditTable();
                    }
                    break;
                default:
                    throw new ProcessToEditTable();
                    break;
            }
        }

        public static void deleteTasksFromTableByStatus(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Удалить:\n"
                + "1)Выполненные\n"
                + "2)Не выполненные\n"
                + "[Назад - esc]";
            IO.print(menu);
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
            IO.clear();
            switch (answer)
            {
                case 1:
                    tm = DB.getAllTasks();
                    tm.findTasksByCompletion(true);
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Удаление выполненных задач. Для подтверждения нажмите Enter");
                    if (IO.getKeyFromUser().Key == ConsoleKey.Enter)
                    {
                        DB.deleteTasks(tm);
                        IO.print("Удаление выполнено успешно. Нажмите любую клавишу для возврата.");
                        IO.getKeyFromUser();
                    }
                    throw new ReturnToTableMenu();
                    break;
                case 2:
                    tm = DB.getAllTasks();
                    tm.findTasksByCompletion(false);
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Удаление не выполненных задач. Для подтверждения нажмите Enter");
                    if (IO.getKeyFromUser().Key == ConsoleKey.Enter)
                    {
                        DB.deleteTasks(tm);
                        IO.print("Удаление выполнено успешно. Нажмите любую клавишу для возврата.");
                        IO.getKeyFromUser();
                    }
                    throw new ReturnToTableMenu();
                    break;
                default:
                    throw new ReturnToTableMenu();
                    break;
            }

        }

        public static void deleteTasksFromTableByDate(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Список " + DB.MainTable);
            tm = DB.getAllTasks();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Для удаления по дате введите дату\n"
                + "[Назад - esc]");
            string date_string;
            ConsoleKeyInfo cki;
            DateTime date = DateTime.Now;
            while (true)
            {
                while (true)
                {
                    date_string = "";
                    cki = IO.getKeyFromUser();
                    while (cki.Key != ConsoleKey.Enter)
                    {
                        if (cki.Key == ConsoleKey.Escape) { throw new ProcessToShowTable(); }
                        date_string += cki.KeyChar;
                        cki = IO.getKeyFromUser();
                    }

                    date_string = date_string.Replace(".", "/").Replace(",", "/")
                        .Replace(":", "/").Replace(";", "/").Replace("-", "/");
                    if (new Regex(@"\d{2,2}/\d{2,2}/\d{4,4}").IsMatch(date_string))
                        date_string = date_string.Substring(6) + "/" + date_string.Substring(3, 2) + "/" + date_string.Substring(0, 2);

                    date = new DateTime();
                    if (!(new Regex(@"\d{4,4}/\d{2,2}/\d{2,2}").IsMatch(date_string))
                        || !DateTime.TryParse(date_string, out date))
                    {
                        IO.clear();
                        IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                        IO.print("Ошибка! Неверный формат даты\nДля удаления по дате введите дату\n"
                        + "[Назад - esc]");
                    }
                    else break;
                }
                tm.findTasksByDate(date);
                if (tm.getTasks().Count == 0)
                {
                    tm = DB.getAllTasks();
                    IO.clear();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Не найдено совпадений! Введите дату еще раз!");
                }
                else break;
            }

            IO.clear();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Удаление задач. Для подтверждения нажмите Enter");
            if (IO.getKeyFromUser().Key == ConsoleKey.Enter)
            {
                DB.deleteTasks(tm);
                IO.print("Удаление выполнено успешно. Нажмите любую клавишу для возврата.");
                IO.getKeyFromUser();
            }
            throw new ReturnToTableMenu();
        }


        public static void deleteTasksFromTableByText(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Список " + DB.MainTable);
            tm = DB.getAllTasks();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Для удаления по тексту введите текст\n"
                + "[Назад - esc]");
            string text;
            ConsoleKeyInfo cki;
            while (true)
            {
                text = "";
                cki = IO.getKeyFromUser();
                while (cki.Key != ConsoleKey.Enter)
                {
                    if (cki.Key == ConsoleKey.Escape)
                    {
                        throw new ProcessToShowTable();
                    }
                    text += cki.KeyChar;
                    cki = IO.getKeyFromUser();
                }
                tm.findTasksByText(text);
                if (tm.getTasks().Count == 0)
                {
                    tm = DB.getAllTasks();
                    IO.clear();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Не найдено совпадений! Введите текст еще раз!");
                }
                else break;
            }
            IO.clear();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Удаление задач. Для подтверждения нажмите Enter");
            if (IO.getKeyFromUser().Key == ConsoleKey.Enter)
            {
                DB.deleteTasks(tm);
                IO.print("Удаление выполнено успешно. Нажмите любую клавишу для возврата.");
                IO.getKeyFromUser();
            }
            throw new ReturnToTableMenu();
        }

        public static void deleteTasksFromTableById(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Список " + DB.MainTable);
            tm = DB.getAllTasks();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Выберите № задачи для удаления:\n[Назад - esc]");
            int countTasks = tm.getTasks().Count;
            ConsoleKeyInfo cki;
            int id = -1;
            do
            {
                string int_ans = "";
                cki = IO.getKeyFromUser();
                while (cki.Key != ConsoleKey.Enter)
                {
                    if (cki.Key == ConsoleKey.Escape)
                    {
                        throw new ReturnToTableMenu();
                    }
                    int_ans += cki.KeyChar;
                    cki = IO.getKeyFromUser();
                }
                bool v = int.TryParse(int_ans, out id);

                if (!v || id < 0)
                {
                    id = -1; IO.clear();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Ошибка! Неверное значение.\n");
                    IO.print("Выберите № задачи для удаления:\n[Назад - esc]");
                }

                tm.findTaskById(id);
                if (tm.getTasks().Count == 0)
                {
                    tm = DB.getAllTasks();
                    id = -1; IO.clear();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Ошибка! Такого номера задания не существует.\n");
                    IO.print("Выберите № задачи для удаления:\n[Назад - esc]");
                }
            } while (id < 0);
            IO.clear();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Удаление задачи. Для подтверждения нажмите Enter");
            if (IO.getKeyFromUser().Key == ConsoleKey.Enter)
            {
                DB.deleteTasks(tm);
                IO.print("Удаление выполнено успешно. Нажмите любую клавишу для возврата.");
                IO.getKeyFromUser();
            }
            throw new ReturnToTableMenu();
        }
    }
}
