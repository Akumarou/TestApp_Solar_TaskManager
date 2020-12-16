using System;
using System.Text.RegularExpressions;
using TestApp_Solar_TaskManager.classes.task_manager;
using TestApp_Solar_TaskManager.classes.UI_impl.db_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.IO_impl;
using TestApp_Solar_TaskManager.classes.exceptions;

namespace TestApp_Solar_TaskManager.classes.UI_impl.menus
{
    class tableManagementEdit
    {
        public static void editTasksAtTable(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Выберите действие:\n"
                + "1)Редактировать по номеру\n"
                + "2)Редактировать по тексту\n"
                + "3)Редактировать по дате\n"
                + "4)Редактировать по статусу\n"
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
                        editTasksAtTableById(DB,tm,IO);
                    }
                    catch (ProcessToShowTable)
                    {
                        throw new ProcessToShowTable();
                    }
                    catch (ReturnToTableMenu)
                    {
                        throw new ReturnToTableMenu();
                    }

                    break;
                case 2:
                    try
                    {
                        editTasksAtTableByText(DB, tm, IO);
                    }
                    catch (ProcessToShowTable)
                    {
                        throw new ProcessToShowTable();
                    }
                    catch (ReturnToTableMenu)
                    {
                        throw new ReturnToTableMenu();
                    }
                    break;
                case 3:
                    try
                    {
                        editTasksAtTableByDate(DB, tm, IO);
                    }
                    catch (ProcessToShowTable)
                    {
                        throw new ProcessToShowTable();
                    }
                    catch (ReturnToTableMenu)
                    {
                        throw new ReturnToTableMenu();
                    }
                    break;
                case 4:
                    try
                    {
                        editTasksAtTableByStatus(DB, tm, IO);
                    }
                    catch (ProcessToShowTable)
                    {
                        throw new ProcessToShowTable();
                    }
                    catch (ReturnToTableMenu)
                    {
                        throw new ReturnToTableMenu();
                    }
                    break;
                default:
                    throw new ProcessToEditTable();
                    break;
            }
        }
        public static void editSingleTask(Task_impl task, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Задание: " + task.Task_text);
            IO.print("Введите новое: ");
            string text = IO.getStringFromUser();
            IO.print("Дата задания: " + task.Task_date.ToString().Substring(0, 10));
            IO.print("Введите новую дату: ");
            DateTime time = IO.getDateFromUser();
            IO.print("Статус задания: " + (task.Task_completion ? "[X]" : "[ ]"));
            string phrase = "Введите новый статус:\n"
                + "1)Выполнено\n"
                + "2)Не выполнено";
            IO.print(phrase);
            ConsoleKeyInfo cki;
            int status = -1;
            do
            {
                cki = IO.getKeyFromUser();
                if (cki.Key == ConsoleKey.Escape) break;
                bool v = int.TryParse(cki.Key.ToString().Substring(1), out status);
                if (!v || status < 1 || status > 2)
                { IO.clear(); IO.print("Ошибка! Неверное значение.\n" + phrase); }
            } while (status < 1 || status > 2);

            tm.changeTask(task, text, time, status == 1 ? true : false);
        }

        public static void editTasksAtTableByStatus(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Редактировать:\n"
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
                    tm.getTasks().ForEach(e=>editSingleTask(e,tm, IO));
                    DB.updateTasks(tm);
                    IO.clear();
                    IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    throw new ReturnToTableMenu();
                    break;
                case 2:
                    tm = DB.getAllTasks();
                    tm.findTasksByCompletion(false);
                    tm.getTasks().ForEach(e => editSingleTask(e, tm, IO));
                    DB.updateTasks(tm);
                    IO.clear();
                    IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    throw new ReturnToTableMenu();
                    break;
                default:
                    throw new ReturnToTableMenu();
                    break;
            }
        }

        public static void editTasksAtTableByDate(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Список " + DB.MainTable);
            tm = DB.getAllTasks();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Для редактирования по дате введите дату\n"
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
                        IO.print("Ошибка! Неверный формат даты\nДля редактирования по дате введите дату\n"
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
            tm.getTasks().ForEach(e => editSingleTask(e, tm, IO));
            DB.updateTasks(tm);
            IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            throw new ReturnToTableMenu();
        }

        public static void editTasksAtTableByText(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Список " + DB.MainTable);
            tm = DB.getAllTasks();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Для редактирования по тексту введите текст\n"
                + "[Назад - esc]");
            string text;
            ConsoleKeyInfo cki;
            while (true)
            {
                text = "";
                cki = IO.getKeyFromUser();
                while (cki.Key != ConsoleKey.Enter)
                {
                    if (cki.Key == ConsoleKey.Escape) { throw new ProcessToShowTable(); }
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
            tm.getTasks().ForEach(e => editSingleTask(e, tm, IO));
            DB.updateTasks(tm);
            IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            throw new ReturnToTableMenu();
        }

        public static void editTasksAtTableById(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Список " + DB.MainTable);
            tm = DB.getAllTasks();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Выберите № задачи для редактирования:\n[Назад - esc]");
            int countTasks = tm.getTasks().Count;
            ConsoleKeyInfo cki;
            int id = -1;
            do
            {
                string int_ans = "";
                cki = IO.getKeyFromUser();
                while (cki.Key != ConsoleKey.Enter)
                {
                    if (cki.Key == ConsoleKey.Escape) { throw new ReturnToTableMenu(); }
                    int_ans += cki.KeyChar;
                    cki = IO.getKeyFromUser();
                }
                bool v = int.TryParse(int_ans, out id);

                if (!v || id < 0)
                {
                    id = -1; IO.clear();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Ошибка! Неверное значение.\n");
                    IO.print("Выберите № задачи для редактирования:\n[Назад - esc]");
                }

                tm.findTaskById(id);
                if (tm.getTasks().Count == 0)
                {
                    tm = DB.getAllTasks();
                    id = -1; IO.clear();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Ошибка! Такого номера задания не существует.\n");
                    IO.print("Выберите № задачи для редактирования:\n[Назад - esc]");
                }
            } while (id < 0);
            tm.getTasks().ForEach(e=>editSingleTask(e,tm, IO));
            DB.updateTasks(tm);
            IO.clear();
            IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            throw new ReturnToTableMenu();
        }
    }
}
