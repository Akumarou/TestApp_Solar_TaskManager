using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ProcessToDeleteTasksAtTable : Exception
    {
        public ProcessToDeleteTasksAtTable()
        {
        }

        public ProcessToDeleteTasksAtTable(string message) : base(message)
        {
        }

        public ProcessToDeleteTasksAtTable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessToDeleteTasksAtTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}