using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector.Exceptions
{
    public class ConnectionException : ApplicationException
    {
        public ConnectionException(string message): base(message)
        {
        }
    }
}
