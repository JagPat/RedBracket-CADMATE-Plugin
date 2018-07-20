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
                Helper.dtFileType = new System.Data.DataTable();
                Helper.dtFileStatus = new System.Data.DataTable();
                Helper.dtProjectDetail = new System.Data.DataTable();
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
