using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ProcessToDeleteTable : Exception
    {
        public ProcessToDeleteTable()
        {
        }

        public ProcessToDeleteTable(string message) : base(message)
        {
        }

        public ProcessToDeleteTable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessToDeleteTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}