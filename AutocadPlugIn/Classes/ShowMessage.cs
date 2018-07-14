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
            MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult InfoYNMess(string Message,string Header=null)
        {
            if (Header == null)
                Header = "System Message";

            return MessageBox.Show(Message, Header, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        public static void ErrorMess(string Message)
        {
            MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ValMess(string Message)
        {
            MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        public static DialogResult ValMessYN(string Message)
        {
            return MessageBox.Show(Message, "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
        }
    }
}
