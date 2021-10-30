using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.ExceptionHandling
{
    [Serializable]
    public class CustomeException : System.Exception
    {
        public readonly string Messages;
        public CustomeException(string messages) 
        {
            this.Messages = messages;
        }
    }
}
