using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ProcessToAddTaskToTable : Exception
    {
        public ProcessToAddTaskToTable()
        {
        }

        public ProcessToAddTaskToTable(string message) : base(message)
        {
        }

        public ProcessToAddTaskToTable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessToAddTaskToTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}