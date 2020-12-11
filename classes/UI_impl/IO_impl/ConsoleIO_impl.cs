using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TestApp_Solar_TaskManager.classes.UI_impl.IO_impl
{
    class ConsoleIO_impl
    {

        public bool clear() {
            Console.Clear();
            return true;
        }
        public bool print()
        {
            Console.WriteLine();
            return true;
        }
        public bool print(string toPrint)
        {
            Console.WriteLine(toPrint);
            return true;
        }
        public bool printTable(string[] header,List<string[]> rows)
        {
            var table = new ConsoleTable(header);
            rows.ForEach(e=>table.AddRow(e));
            table.Options.EnableCount = false;
            print(table.ToString());

            return true;
        }
        public string getStringFromUser()
        {
            return Console.ReadLine();
        }
        
        public int getIntFromUser()
        {
            bool v = int.TryParse(Console.ReadLine(), out int num);
            if (!v) throw new Exception();
            return num;
        }
        public ConsoleKeyInfo getKeyFromUser()
        {
            return Console.ReadKey(); 
        }
        public DateTime getDateFromUser()
        {
            string date_string = Console.ReadLine();
            date_string = date_string.Replace(".", "/").Replace(",", "/")
                .Replace(":", "/").Replace(";", "/").Replace("-", "/");
            if (new Regex(@"\d{2,2}/\d{2,2}/\d{4,4}").IsMatch(date_string))
                date_string = date_string.Substring(6) + "/" + date_string.Substring(3, 2) + "/" + date_string.Substring(0, 2);
            DateTime date = new DateTime();
            if (!(new Regex(@"\d{4,4}/\d{2,2}/\d{2,2}").IsMatch(date_string))
                || !DateTime.TryParse(date_string, out date))
            {
                print("Ошибка! Неверный формат даты");
                getDateFromUser();
            }
            return date;
        }
    }
}
