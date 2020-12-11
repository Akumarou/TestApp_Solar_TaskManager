using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using TestApp_Solar_TaskManager.classes.db_connect;
using TestApp_Solar_TaskManager.classes.task_manager;

namespace TestApp_Solar_TaskManager.classes.UI_impl.db_impl
{
    class DB_impl
    {
        private DB_Executor DB;
        private string mainTable;
        public string MainTable
        {
            get { return mainTable; }
            set { mainTable = value; }
        }

        public DB_impl()
        {
            this.DB = new DB_Executor();
            this.mainTable = "";
        }

        public List<string[]> getTablesDataGrid()
        {
            List<string[]> result = new List<string[]>();
            List<string> temp = DB.getTables();
            for (int i = 0; i < temp.Count; i++)
                result.Add(new string[] { i.ToString(), temp[i] });
            return result;
        }
        public List<string> getTables()
        {
            return DB.getTables();
        }

        public bool addTable(string name)
        {
            if (DB.getTables().Contains(name)) return false;
            string query = "CREATE TABLE "+ name + " (task_id INT NOT NULL PRIMARY KEY IDENTITY, "+
                "task_text VARCHAR(MAX) NULL, task_date DATE NULL, task_completion BIT NULL)";
            DB.executeQuery(query, false);
            return true;
        }
        public bool dropTable(string name)
        {
            if (DB.getTables().Contains(name))
                DB.executeQuery("DROP TABLE "+name, false);
            return true;
        }

        public Task_manager_impl getAllTasks()
        {
            if (mainTable.Equals(""))
                throw new Exception();
            Task_manager_impl tm = new Task_manager_impl();
            DB.executeQuery("SELECT * FROM " + mainTable,true)
                .ForEach(e => tm.addTask(int.Parse(e[0]), e[1], DateTime.Parse(e[2]), bool.Parse(e[3])));            
            return tm;
        }
        public bool updateTasks(Task_manager_impl tm)
        {

            List<string> query = new List<string>();

            for(int i = 0; i < tm.getTasks().Count; i++)
            {
                if (tm.getTasks()[i].Task_id == -1)
                    query.Add("INSERT INTO "+mainTable+" VALUES('"+ tm.getTasks()[i].Task_text+ "','"
                        + tm.getTasks()[i].Task_date.ToString()+ "',"+ (tm.getTasks()[i].Task_completion?1:0)+ ")");
                else
                    query.Add("UPDATE " + mainTable + " SET task_text = '"+ tm.getTasks()[i].Task_text 
                        + "', task_date = '" + tm.getTasks()[i].Task_date.ToString() 
                        + "', task_completion = " + (tm.getTasks()[i].Task_completion ? 1 : 0)
                        + " WHERE task_id = "+ tm.getTasks()[i].Task_id + ";");
            }
            query.ForEach(e=>DB.executeQuery(e,false));

            return true;
        }

        internal bool deleteTasks(Task_manager_impl tm)
        {
            List<string> query = new List<string>();

            for (int i = 0; i < tm.getTasks().Count; i++)
                query.Add("DELETE FROM " + mainTable + " WHERE task_id = " + tm.getTasks()[i].Task_id + ";");
            query.ForEach(e => DB.executeQuery(e, false));

            return true;
        }

    }
}
