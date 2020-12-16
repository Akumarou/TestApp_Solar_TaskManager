using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ProcessToEditTasksAtTable : Exception
    {
        public ProcessToEditTasksAtTable()
        {
        }

        public ProcessToEditTasksAtTable(string message) : base(message)
        {
        }

        public ProcessToEditTasksAtTable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessToEditTasksAtTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}