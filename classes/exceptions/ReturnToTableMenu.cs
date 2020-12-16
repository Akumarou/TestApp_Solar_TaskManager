using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ReturnToTableMenu : Exception
    {
        public ReturnToTableMenu()
        {
        }

        public ReturnToTableMenu(string message) : base(message)
        {
        }

        public ReturnToTableMenu(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReturnToTableMenu(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}