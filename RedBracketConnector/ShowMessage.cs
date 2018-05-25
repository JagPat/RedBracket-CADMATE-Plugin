using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedBracketConnector
{
    public static class ShowMessage
    {
        public static void InfoMess(string Message)
        {
            MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult InfoYNMess(string Message)
        {
            return MessageBox.Show(Message, "System Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
        }

        public static void ErrorMess(string Message)
        {
            MessageBox.Show(Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

    }
}
