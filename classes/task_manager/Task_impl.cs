using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp_Solar_TaskManager.classes.task_manager
{
    class Task_impl
    {
        private int task_id;
        public int Task_id   
        {
            get { return task_id; }  
        }
        private string task_text;
        public string Task_text
        {
            get { return task_text; }
            set { task_text = value ?? throw new ArgumentNullException(nameof(value));}
        }
        private DateTime task_date;
        public DateTime Task_date
        {
            get { return task_date; }  
            set { task_date = value; }  
        }
        private bool task_completion;
        public bool Task_completion
        {
            get { return task_completion; }
            set { task_completion = value; }
        }

        public Task_impl()
        {
            this.task_id = -1;
            this.task_text = "";
            this.task_date = DateTime.Now;
            this.task_completion = false;
        }
        public Task_impl(string task_text, DateTime task_date)
        {
            this.task_id = -1;
            this.task_text = task_text ?? throw new ArgumentNullException(nameof(task_text));
            this.task_date = task_date;
            this.task_completion = false;
        }
        public Task_impl(int task_id, string task_text, DateTime task_date, bool task_completion)
        {
            this.task_id = task_id;
            this.task_text = task_text ?? throw new ArgumentNullException(nameof(task_text));
            this.task_date = task_date;
            this.task_completion = task_completion;
        }
    }
}
