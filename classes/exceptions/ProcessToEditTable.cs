using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ProcessToEditTable : Exception
    {
        public ProcessToEditTable()
        {
        }

        public ProcessToEditTable(string message) : base(message)
        {
        }

        public ProcessToEditTable(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProcessToEditTable(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}