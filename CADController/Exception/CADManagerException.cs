using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CADController.Exception
{
  public  class CADManagerException:ApplicationException
    {
        public CADManagerException(string message): base(message)
        {
        }
    }
}
