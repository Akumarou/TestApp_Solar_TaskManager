using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ProcessToShowTable : Exception
    {
        public ProcessToShowTable()
        {
        }

        public ProcessToShowTable(string message) : base(message)
        {
        }

        public ProcessToShowTable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessToShowTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}