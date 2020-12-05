using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp_Solar_TaskManager
{
    class Task : ITask
    {
        private DateTime date;
        public DateTime Date   // property
        {
            get { return date; }   // get method
            set { date = value; }  // set method
        }
        private string the_task;
        public string The_task   // property
        {
            get { return the_task; }   // get method
            set { the_task = value; }  // set method
        }

        public Task()
        {
            DateTime.TryParse("0000/00/00", out DateTime date);
        }

        public Task(DateTime date, string the_task)
        {
            this.date = date;
            this.the_task = the_task ?? throw new ArgumentNullException(nameof(the_task));
        }
        public Task(string the_task, DateTime date)
        {
            this.date = date;
            this.the_task = the_task ?? throw new ArgumentNullException(nameof(the_task));
        }
        public Task(string the_task)
        {
            this.date = DateTime.UtcNow;
            this.the_task = the_task ?? throw new ArgumentNullException(nameof(the_task));
        }
        public Task(DateTime date)
        {
            this.date = date;
            this.the_task = "";
        }



        public override string ToString()
        {
            return "{" + date.ToString().Substring(0, 10) + "}: "+the_task;
        }
    }
}
