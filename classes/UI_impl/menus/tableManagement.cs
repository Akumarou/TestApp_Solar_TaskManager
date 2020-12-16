using System;
using System.Text.RegularExpressions;
using TestApp_Solar_TaskManager.classes.task_manager;
using TestApp_Solar_TaskManager.classes.UI_impl.db_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.IO_impl;
using TestApp_Solar_TaskManager.classes.exceptions;

namespace TestApp_Solar_TaskManager.classes.UI_impl.menus
{
    class tableManagement
    {
        public static void getTableMenu(DB_impl DB, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Список " + DB.MainTable + "\n"
                + "Выберите действие:\n"
                + "1)Показать список задач\n"
                + "2)Редактировать список задач\n"
                + "3)Удалить список задач\n"
                + "[Выйти в главное меню - esc]";
            IO.print(menu);
            ConsoleKeyInfo cki;
            int answer = -1;
            do
            {
                cki = IO.getKeyFromUser();
                if (cki.Key == ConsoleKey.Escape) break;
                bool v = int.TryParse(cki.Key.ToString().Substring(1), out answer);
                if (!v || answer < 1 || answer > 3)
                { IO.clear(); IO.print("Ошибка! Неверное значение.\n" + menu); }
            } while (answer < 1 || answer > 3);
            switch (answer)
            {
                case 1:
                    throw new ProcessToShowTable();
                    break;
                case 2:
                    throw new ProcessToEditTable();
                    break;
                case 3:
                    throw new ProcessToDeleteTable();
                    break;
                default:
                    throw new ReturnToMainMenu();
                    break;
            }
        }
        public static void showTable(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Список " + DB.MainTable);
            tm = DB.getAllTasks();
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            string menu = "Выберите действие:\n"
                + "1)Отсортировать\n"
                + "2)Найти\n"
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
            switch (answer)
            {
                case 1:
                    throw new ProcessToSortTable();
                    break;
                case 2:
                    throw new ProcessToFindInTable();
                    break;
                default:
                    throw new ReturnToTableMenu();
                    break;
            }
        }
        public static void editTable(DB_impl DB, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Редактирование списка:\n"
                + "1)Добавить задание\n"
                + "2)Редактировать задания\n"
                + "3)Удалить задания\n"
                + "[Назад - esc]";
            IO.print(menu);
            ConsoleKeyInfo cki;
            int answer = -1;
            do
            {
                cki = IO.getKeyFromUser();
                if (cki.Key == ConsoleKey.Escape) break;
                bool v = int.TryParse(cki.Key.ToString().Substring(1), out answer);
                if (!v || answer < 1 || answer > 3)
                { IO.clear(); IO.print("Ошибка! Неверное значение.\n" + menu); }
            } while (answer < 1 || answer > 3);
            switch (answer)
            {
                case 1:
                    throw new ProcessToAddTaskToTable();
                    break;
                case 2:
                    throw new ProcessToEditTasksAtTable();
                    break;
                case 3:
                    throw new ProcessToDeleteTasksAtTable();
                    break;
                default:
                    throw new ReturnToTableMenu();
                    break;
            }
        }
        public static void addTaskToTable(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Добавление новой задачи\n[Назад - esc]\nВведите задание:");
            string text = "";
            ConsoleKeyInfo cki = IO.getKeyFromUser();
            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.Escape) { throw new ProcessToShowTable(); }
                text += cki.KeyChar;
                cki = IO.getKeyFromUser();
            }
            IO.print(text + "\nВведите дату:");
            string date_string;
            DateTime date = DateTime.Now;
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
                    IO.print("Ошибка! Неверный формат даты\nВведите дату");
                }
                else break;
            }
            tm = new Task_manager_impl();
            tm.addTask(text, date);
            IO.getKeyFromUser();
            DB.updateTasks(tm);
            IO.clear();
            IO.print("Успешно добавлено");
            IO.getKeyFromUser();
            throw new ProcessToAddTaskToTable();
        }
        public static void sortTable(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Выберите действие:\n"
                + "1)Отсортировать по тексту\n"
                + "2)Отсортировать по дате\n"
                + "3)Отсортировать по статусу\n"
                + "[Назад - esc]";
            IO.print(menu);
            ConsoleKeyInfo cki;
            int answer = -1;
            do
            {
                cki = IO.getKeyFromUser();
                if (cki.Key == ConsoleKey.Escape) break;
                bool v = int.TryParse(cki.Key.ToString().Substring(1), out answer);
                if (!v || answer < 1 || answer > 3)
                { IO.clear(); IO.print("Ошибка! Неверное значение.\n" + menu); }
            } while (answer < 1 || answer > 3);
            IO.clear();
            switch (answer)
            {
                case 1:
                    IO.print("Сортировка по тексту");
                    tm.sortByText();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    throw new ProcessToShowTable();
                    break;
                case 2:
                    IO.print("Сортировка по дате");
                    tm.sortByDate();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    throw new ProcessToShowTable();
                    break;
                case 3:
                    IO.print("Сортировка по статусу");
                    tm.sortByCompletion();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    throw new ProcessToShowTable();
                    break;
                default:
                    throw new ProcessToShowTable();
                    break;
            }
        }
    }
}
