using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutocadPlugIn
{
    public static class ShowMessage
    {
        public static void InfoMess(string Message)
        {
            Helper.HideProgressBar();
            MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult InfoYNMess(string Message, string Header = null)
        {
            DialogResult DR = new DialogResult();
            Helper.HideProgressBar();
            if (Header == null)
                Header = "System Message";

            DR = MessageBox.Show(Message, Header, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            Helper.ShowProgressBar();
            return DR;
        }

        public static void ErrorMess(string Message)
        {
            Helper.HideProgressBar();
            //MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            frmError objfrmError = new frmError("Unknown Error", Message);
            objfrmError.ShowDialog();
            Helper.ShowProgressBar();

        }
        public static void ErrorMessUD(string Message)
        {
            Helper.HideProgressBar();
            MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            frmError objfrmError = new frmError(Message, Message);
            //// objfrmError.ShowDialog();
            Helper.ShowProgressBar();

        }
        public static void ErrorMess(string Message, string TMessage)
        {
            Helper.HideProgressBar();
            frmError objfrmError = new frmError(Message, TMessage);
            objfrmError.ShowDialog();
            Helper.ShowProgressBar();
            // MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ErrorMess(string Message, RestSharp.RestResponse restResponse)
        {
            string TMessage = restResponse.ErrorMessage + "\n\n" +
                             restResponse.ResponseUri + "\n\n" +
                             restResponse.Content;
            if (restResponse.StatusCode == 0)
            {
                Message = "No internet.\n Please check your internet connection.";
            }
            else if (restResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Message = "Unauthorize access.";
            }
            ErrorMess(Message, TMessage);

        }
        public static void ValMess(string Message)
        {
            Helper.HideProgressBar();
            MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Helper.ShowProgressBar();
        }
        public static DialogResult ValMessYN(string Message)
        {
            DialogResult DR = new DialogResult();
            Helper.HideProgressBar();
            DR = MessageBox.Show(Message, "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            Helper.ShowProgressBar();
            return DR;
        }
    }
}
