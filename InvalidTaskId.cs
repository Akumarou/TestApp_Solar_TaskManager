using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{
    [Serializable]
    class InvalidTaskId : Exception
    {
        public InvalidTaskId()
        {

        }

        public InvalidTaskId(string name)
            : base(String.Format("Invalid Student Name: {0}", name))
        {

        }

    }
}
