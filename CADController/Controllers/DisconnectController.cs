using System;
using ArasConnector;

using ArasConnector.Exceptions;
using CADController.Controllers;
using CADController.Commands;
using System.Windows.Forms;


namespace CADController.Controllers
{
    public class DisconnectController : BaseController
    {
        public override void Execute(Command command)
        {
           
            DisconnectCommand cmd = (DisconnectCommand)command;
            try
            {
                //objConnector.Disconnect();
                           
                return;
            }
            catch (ConnectionException ex)
            {
                errorString = ex.Message;
                return;
            }
         

        }
    }
}
