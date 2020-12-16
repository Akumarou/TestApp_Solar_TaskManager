using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ProcessToFindInTable : Exception
    {
        public ProcessToFindInTable()
        {
        }

        public ProcessToFindInTable(string message) : base(message)
        {
        }

        public ProcessToFindInTable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessToFindInTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}