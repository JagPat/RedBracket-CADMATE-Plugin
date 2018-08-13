using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
namespace RBAutocadPlugIn
{
    public class MessageController : BaseController
    {
        public override void Execute(Command command)
        {

            ConnectionCommand cmd = (ConnectionCommand)command;
            try
            {
                if (cmd.UserName == "")
                {
                    infoMessage = "UserName Required";
                    return;
                }
                if (cmd.Passwd == "")
                {
                    infoMessage = "Password Required";
                    return;
                }
                //if (cmd.DbName == "")
                //{
                //    infoMessage = "Database Name Required";
                //    return;
                //}
                if (cmd.Url == "")
                {
                    infoMessage = "Url Required";
                    return;
                }
                infoMessage = null;
                return;
            }
            catch ( Exception ex)
            {
                errorString = ex.Message;
                return;
            }
        }
      }
}

