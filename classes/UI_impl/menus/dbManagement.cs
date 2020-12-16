using System;
using TestApp_Solar_TaskManager.classes.UI_impl.db_impl;
using TestApp_Solar_TaskManager.classes.UI_impl.IO_impl;
using TestApp_Solar_TaskManager.classes.exceptions;

namespace TestApp_Solar_TaskManager.classes.UI_impl.menus
{
    class dbManagement
    {
        public static void createNewTable(DB_impl DB, ConsoleIO_impl IO)
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
                    if (cki.Key == ConsoleKey.Escape) { throw new ProcessToShowTable(); }
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
                throw new ReturnToTableMenu();
            }
            else
            {
                IO.clear();
                IO.print("Ошибка в работе приложения. \n"
                    + "Нажмите любую клавишу для перехода в главное меню.");
                IO.getKeyFromUser();
                throw new ReturnToTableMenu();
            }
        }

        public static void deleteTable(DB_impl DB, ConsoleIO_impl IO)
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
                    throw new ReturnToMainMenu();
                }
                IO.print("Список " + DB.MainTable + " успешно удален.\n"
                    + "Нажмите любую клавишу для перехода в главное меню.");
                DB.MainTable = "";
                IO.getKeyFromUser();
                throw new ReturnToMainMenu();
            }
            else throw new ReturnToTableMenu();
        }

        public static void openTable(DB_impl DB, ConsoleIO_impl IO)
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
                if (cki.Key == ConsoleKey.Escape) throw new ReturnToMainMenu();
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
            throw new ReturnToTableMenu();
        }
    }
}
