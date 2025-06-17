using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedBracketConnector.Exceptions
{
    public class ConnectionException : ApplicationException
    {
        public ConnectionException(string message) : base(message)
        {
        }
    }
}
