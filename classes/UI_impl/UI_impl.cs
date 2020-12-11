using System;
using System.Text.RegularExpressions;
using TestApp_Solar_TaskManager.classes.task_manager;
using TestApp_Solar_TaskManager.classes.UI_impl.db_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.IO_impl;

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
        public void createNewTable()
        {
            IO.clear();
            IO.print("Для создания нового списка введите его название:\n"
                + "[Назад - esc]");
            string name;
            while (true)
            {
                name = "";
                ConsoleKeyInfo cki = IO.getKeyFromUser();
                while (cki.Key != ConsoleKey.Enter)
                {
                    if (cki.Key == ConsoleKey.Escape) { showTable(); }
                    if ((cki.KeyChar > '/' && cki.KeyChar < ':') || (cki.KeyChar > '@' && cki.KeyChar < '[')
                        || (cki.KeyChar > '`' && cki.KeyChar < '{') || cki.KeyChar == '-' || cki.KeyChar == '_')
                        name += cki.KeyChar;
                    cki = IO.getKeyFromUser();
                }
                if (DB.getTables().Contains(name))
                    IO.print("Ошибка! Такой список уже существует.\n" +
                        "Введите новое название:");
                else break;
            }
            bool v = DB.addTable(name);
            if (v)
            {
                DB.MainTable = name;
                getTableMenu();
            }
            else
            {
                IO.clear();
                IO.print("Ошибка в работе приложения. \n"
                    + "Нажмите любую клавишу для перехода в главное меню.");
                IO.getKeyFromUser();
                getMainMenu();
            }
        }

        public void openTable()
        {
            IO.clear();
            var tables = DB.getTablesDataGrid();
            IO.print("Выберите № списка:\n[Назад - esc]");
            IO.printTable(new string[] { "№", "Список" }, tables);
            int countTables = tables.Count;
            ConsoleKeyInfo cki;
            int answer = -1;
            do
            {
                cki = IO.getKeyFromUser();
                if (cki.Key == ConsoleKey.Escape) getMainMenu();
                bool v = int.TryParse(cki.Key.ToString().Substring(1), out answer);
                if (!v || answer < 0 || answer >= countTables)
                {
                    answer = -1; IO.clear();
                    IO.print("Ошибка! Неверное значение.\n");
                    IO.print("Выберите № списка:\n[Назад - esc]");
                    IO.printTable(new string[] { "№", "Список" }, tables);
                }
            } while (answer < 0 || answer >= countTables);
            DB.MainTable = DB.getTables()[answer];
            getTableMenu();
        }
        public void closeApp()
        {
            Environment.Exit(0);
        }

        private void getTableMenu()
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
                    showTable();
                    break;
                case 2:
                    editTable();
                    break;
                case 3:
                    deleteTable();
                    break;
                default:
                    getMainMenu();
                    break;
            }
        }
        private void showTable()
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
                    sortTable();
                    break;
                case 2:
                    findInTable();
                    break;
                default:
                    getTableMenu();
                    break;
            }
        }
        private void editTable()
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
                    addTaskToTable();
                    break;
                case 2:
                    editTasksAtTable();
                    break;
                case 3:
                    deleteTasksFromTable();
                    break;
                default:
                    getTableMenu();
                    break;
            }
        }

        private void deleteTasksFromTable()
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
                    deleteTasksFromTableById();
                    break;
                case 2:
                    deleteTasksFromTableByText();
                    break;
                case 3:
                    deleteTasksFromTableByDate();
                    break;
                case 4:
                    deleteTasksFromTableByStatus();
                    break;
                default:
                    editTable();
                    break;
            }
        }

        private void deleteTasksFromTableByStatus()
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
                    getTableMenu();
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
                    getTableMenu();
                    break;
                default:
                    getTableMenu();
                    break;
            }

        }

        private void deleteTasksFromTableByDate()
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
                        if (cki.Key == ConsoleKey.Escape) { showTable(); }
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
            getTableMenu();
        }

        private void deleteTasksFromTableByText()
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
                    if (cki.Key == ConsoleKey.Escape) { showTable(); }
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
            getTableMenu();
        }

        private void deleteTasksFromTableById()
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
                    if (cki.Key == ConsoleKey.Escape) { getTableMenu(); }
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
            getTableMenu();
        }

        private void editTasksAtTable()
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
                    editTasksAtTableById();
                    break;
                case 2:
                    editTasksAtTableByText();
                    break;
                case 3:
                    editTasksAtTableByDate();
                    break;
                case 4:
                    editTasksAtTableByStatus();
                    break;
                default:
                    editTable();
                    break;
            }
        }
        private void editSingleTask(Task_impl task)
        {
            IO.clear();
            IO.print("Задание: " + task.Task_text);
            IO.print("Введите новое: ");
            string text = IO.getStringFromUser();
            IO.print("Дата задания: " + task.Task_date.ToString().Substring(0,10));
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

            tm.changeTask(task, text, time, status == 1 ? true:false);
        }

        private void editTasksAtTableByStatus()
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
                    tm.getTasks().ForEach(editSingleTask);
                    DB.updateTasks(tm);
                    IO.clear();
                    IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    getTableMenu();
                    break;
                case 2:
                    tm = DB.getAllTasks();
                    tm.findTasksByCompletion(false);
                    tm.getTasks().ForEach(editSingleTask);
                    DB.updateTasks(tm);
                    IO.clear();
                    IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    getTableMenu();
                    break;
                default:
                    getTableMenu();
                    break;
            }
        }

        private void editTasksAtTableByDate()
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
                        if (cki.Key == ConsoleKey.Escape) { showTable(); }
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
            tm.getTasks().ForEach(editSingleTask);
            DB.updateTasks(tm);
            IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            getTableMenu();
        }

        private void editTasksAtTableByText()
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
                    if (cki.Key == ConsoleKey.Escape) { showTable(); }
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
            tm.getTasks().ForEach(editSingleTask);
            DB.updateTasks(tm);
            IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            getTableMenu();
        }

        private void editTasksAtTableById()
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
                    if (cki.Key == ConsoleKey.Escape) { getTableMenu(); }
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
            tm.getTasks().ForEach(editSingleTask);
            DB.updateTasks(tm);
            IO.clear();
            IO.print("Редактирование выполнено успешно. Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            getTableMenu();
        }

        private void addTaskToTable()
        {
            IO.clear();
            IO.print("Добавление новой задачи\n[Назад - esc]\nВведите задание:");
            string text = "";
            ConsoleKeyInfo cki = IO.getKeyFromUser();
            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.Escape) { showTable(); }
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
                    if (cki.Key == ConsoleKey.Escape) { showTable(); }
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
            addTaskToTable();
        }

        private void deleteTable()
        {
            IO.clear();
            IO.print("Внимание! Удаление списка задач\n"
                + "Нажмите [Enter] для подтверждения");
            if (IO.getKeyFromUser().Key == ConsoleKey.Enter)
            {
                IO.clear();
                bool v = DB.dropTable(DB.MainTable);
                if (!v)
                {
                    IO.print("Ошибка работы приложения!\n"
                    + "Нажмите любую клавишу для перехода в главное меню.");
                    IO.getKeyFromUser();
                    getMainMenu();
                }
                IO.print("Список " + DB.MainTable + " успешно удален.\n"
                    + "Нажмите любую клавишу для перехода в главное меню.");
                DB.MainTable = "";
                IO.getKeyFromUser();
                getMainMenu();
            }
            else getTableMenu();
        }
        private void findInTable()
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
            switch (answer)
            {
                case 1:
                    findInTableByText();
                    break;
                case 2:
                    findInTableByDate();
                    break;
                case 3:
                    findInTableByCompletion();
                    break;
                default:
                    showTable();
                    break;
            }
        }

        private void findInTableByCompletion()
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
                    showTable();
                    break;
                case 2:
                    IO.print("Поиск не выполненных задач");
                    tm.findTasksByCompletion(false);
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    showTable();
                    break;
                default:
                    showTable();
                    break;
            }
        }

        private void findInTableByDate()
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
                    if (cki.Key == ConsoleKey.Escape) { showTable(); }
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
            showTable();
        }

        private void findInTableByText()
        {
            IO.clear();
            IO.print("Для поиска по тексту введите текст\n"
                + "[Назад - esc]");
            string text = "";
            ConsoleKeyInfo cki = IO.getKeyFromUser();
            while (cki.Key != ConsoleKey.Enter)
            {
                if (cki.Key == ConsoleKey.Escape) { showTable(); }
                text += cki.KeyChar;
                cki = IO.getKeyFromUser();
            }

            IO.print("Поиск по тексту - " + text);
            tm.findTasksByText(text);
            IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
            IO.print("Нажмите любую клавишу для возврата.");
            IO.getKeyFromUser();
            showTable();
        }

        private void sortTable()
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
                    showTable();
                    break;
                case 2:
                    IO.print("Сортировка по дате");
                    tm.sortByDate();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    showTable();
                    break;
                case 3:
                    IO.print("Сортировка по статусу");
                    tm.sortByCompletion();
                    IO.printTable(new string[] { "id", "[X]/[ ]", "Дата", "Задание" }, tm.getTaskManagerDataGrid());
                    IO.print("Нажмите любую клавишу для возврата.");
                    IO.getKeyFromUser();
                    showTable();
                    break;
                default:
                    showTable();
                    break;
            }
        }
    }
}
