using System;
using System.Diagnostics;
using CADController.Commands;
namespace CADController.Controllers
{
   public class HelpController:BaseController
    {

       public override void Execute(Command command)
       {

           HelpCommand cmd = (HelpCommand)command;
           Process openFile = new Process();
           try
           {
               openFile.StartInfo.FileName = cmd.HelpFilePath;
               openFile.Start();
               openFile.Close();
           }
           catch (ArasConnector.Exceptions.ConnectionException ex)
           {
               errorString = ex.Message;
               return;
           }

       }


    }
}
