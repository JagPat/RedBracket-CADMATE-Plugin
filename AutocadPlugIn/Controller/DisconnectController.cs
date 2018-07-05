using System; 
 
  
using System.Windows.Forms;


namespace AutocadPlugIn
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
            catch (Exception ex)
            {
                errorString = ex.Message;
                return;
            }
         

        }
    }
}
