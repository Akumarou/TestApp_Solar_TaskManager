using System;
using System.Runtime.Serialization;

namespace TestApp_Solar_TaskManager.classes.exceptions
{
    [Serializable]
    internal class ReturnToMainMenu : Exception
    {
        public ReturnToMainMenu()
        {
        }

        public ReturnToMainMenu(string message) : base(message)
        {
        }

        public ReturnToMainMenu(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReturnToMainMenu(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}