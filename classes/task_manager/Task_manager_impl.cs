using System;
using System.Collections.Generic;

namespace TestApp_Solar_TaskManager.classes.task_manager
{
    class Task_manager_impl
    {
        private List<Task_impl> tasks;

        public Task_manager_impl()
        {
            this.tasks = new List<Task_impl>();
        }

        public Task_manager_impl(List<Task_impl> tasks)
        {
            this.tasks = tasks ?? throw new ArgumentNullException(nameof(tasks));
        }

        public bool addTask(string task, DateTime time)
        {
            tasks.Add(new Task_impl(task, time));
            return true;
        }
        public bool addTask(int id,string task, DateTime time,bool complete)
        {
            tasks.Add(new Task_impl(id,task, time,complete));
            return true;
        }

        public bool changeTask(Task_impl task, string new_task, DateTime new_time, bool new_status)
        {
            if (!tasks.Contains(task)) return false;
            task.Task_text = new_task;
            task.Task_date = new_time;
            task.Task_completion = new_status;
            return true;
        }
        public bool findTasksByCompletion(bool compl)
        {
            List<Task_impl> temp = new List<Task_impl>();
            for (int i = 0; i < tasks.Count; i++)
                if (tasks[i].Task_completion == compl)
                    temp.Add(tasks[i]);
            tasks = temp;
            return true;
        }
        public bool findTasksByText(string text)
        {
            List<Task_impl> temp = new List<Task_impl>();
            for (int i = 0; i < tasks.Count; i++)
                if (tasks[i].Task_text.Contains(text))
                    temp.Add(tasks[i]);
            tasks = temp;
            return true;
        }
        public bool findTasksByDate(DateTime date)
        {
            List<Task_impl> temp = new List<Task_impl>();
            for (int i = 0; i < tasks.Count; i++)
                if (tasks[i].Task_date.Equals(date))
                    temp.Add(tasks[i]);
            tasks = temp;
            return true;
        }


        public bool sortByText()
        {
            tasks.Sort((l1, l2) => l1.Task_text.CompareTo(l2.Task_text));
            return true;
        }
        public bool sortByDate()
        {
            tasks.Sort((l1, l2) => l1.Task_date.CompareTo(l2.Task_date));
            return true;
        }
        public bool sortByCompletion()
        {
            tasks.Sort((l2, l1) => l1.Task_completion.CompareTo(l2.Task_completion));
            return true;
        }


        public List<string[]> getTaskManagerDataGrid()
        {
            List<string[]> result = new List<string[]>();
            tasks.ForEach(e=>result.Add(new string[]{ e.Task_id.ToString(), (e.Task_completion ? "[X]" : "[ ]"), e.Task_date.ToString().Substring(0,10), e.Task_text }));
            return result;
        }

        public List<Task_impl> getTasks()
        {
            return tasks;
        }

        internal void findTaskById(int v)
        {
            List<Task_impl> temp = new List<Task_impl>();
            for (int i = 0; i < tasks.Count; i++)
                if (tasks[i].Task_id.Equals(v))
                    temp.Add(tasks[i]);
            tasks = temp;
        }
    }
}
