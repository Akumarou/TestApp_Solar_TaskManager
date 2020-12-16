using System;
using System.Text.RegularExpressions;
using TestApp_Solar_TaskManager.classes.task_manager;
using TestApp_Solar_TaskManager.classes.UI_impl.db_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.IO_impl;
using TestApp_Solar_TaskManager.classes.exceptions;

namespace TestApp_Solar_TaskManager.classes.UI_impl.menus
{
    class tableManagementFind
    {
        public static void findInTable(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Выберите действие:\n"
                + "1)Найти по тексту\n"
                + "2)Найти по дате\n"
                + "3)Найти по статусу\n"
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
            try
            {
                switch (answer)
                {
                    case 1:
                        findInTableByText(DB,tm,IO);
                        break;
                    case 2:
                        findInTableByDate(DB, tm, IO);
                        break;
                    case 3:
                        findInTableByCompletion(DB, tm, IO);
                        break;
                    default:
                        throw new ProcessToShowTable();
                        break;
                }
            }
            catch (ProcessToShowTable)
            {
                throw new ProcessToShowTable();
            }
            
        }

        public static void findInTableByCompletion(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            string menu = "Найти:\n"
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
                    IO.print("Поиск выполненных задач");
                    tm.findTasksByCompletion(true);
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    throw new ProcessToShowTable();
                    break;
                case 2:
                    IO.print("Поиск не выполненных задач");
                    tm.findTasksByCompletion(false);
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

        public static void findInTableByDate(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Для поиска по дате введите дату\n"
                + "[Назад - esc]");
            string date_string;
            ConsoleKeyInfo cki;
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
                    IO.print("Ошибка! Неверный формат даты\nДля поиска по дате введите дату\n"
                    + "[Назад - esc]");
                }
                else break;
            }

            IO.print("Поиск по дате - " + date.ToString().Substring(0, 10));
            tm.findTasksByDate(date);
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            throw new ProcessToShowTable();
        }

        public static void findInTableByText(DB_impl DB, Task_manager_impl tm, ConsoleIO_impl IO)
        {
            IO.clear();
            IO.print("Для поиска по тексту введите текст\n"
                + "[Назад - esc]");
            string text = "";
            ConsoleKeyInfo cki = IO.getKeyFromUser();
            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.Escape) { throw new ProcessToShowTable(); }
                text += cki.KeyChar;
                cki = IO.getKeyFromUser();
            }

            IO.print("Поиск по тексту - " + text);
            tm.findTasksByText(text);
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            throw new ProcessToShowTable();
        }
    }
}
