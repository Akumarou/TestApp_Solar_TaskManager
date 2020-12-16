using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ProcessToSortTable : Exception
    {
        public ProcessToSortTable()
        {
        }

        public ProcessToSortTable(string message) : base(message)
        {
        }

        public ProcessToSortTable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessToSortTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}