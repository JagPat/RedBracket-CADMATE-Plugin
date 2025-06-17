using System;
using System.Diagnostics; 
namespace RBAutocadPlugIn
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
           catch  (Exception ex)
           {
               errorString = ex.Message;
               return;
           }

       }


    }
}
