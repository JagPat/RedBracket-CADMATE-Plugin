using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CryptoGraphy
{
    public partial class Dycript : Form
    {
        public Dycript()
        {
            InitializeComponent();
        }

        private void Dycript_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists(lblFilePath.Text))
                {

                    string encryptedText = File.ReadAllText(lblFilePath.Text);
                    string PlainText = "";
                    string[] ET = encryptedText.Split(' ');
                    for (int i = 0; i < ET.Length; i++)
                    {
                        byte[] et = System.Convert.FromBase64String(ET[i]);
                        PlainText = PlainText + Environment.NewLine + Environment.NewLine + System.Text.Encoding.ASCII.GetString(et);
                    }

                    if (File.Exists(Path.Combine(Path.GetDirectoryName(lblFilePath.Text), "PT_" + Path.GetFileName(lblFilePath.Text))))
                    {
                        File.Delete(Path.Combine(Path.GetDirectoryName(lblFilePath.Text), "PT_" + Path.GetFileName(lblFilePath.Text)));
                    }

                    File.AppendAllText(Path.Combine(Path.GetDirectoryName(lblFilePath.Text), "PT_" + Path.GetFileName(lblFilePath.Text)), PlainText);
                    
                    //byte[] et = System.Convert.FromBase64String(encryptedText);
                    //string PlainText = System.Text.Encoding.ASCII.GetString(et);
                    //if(File.Exists(Path.Combine(Path.GetDirectoryName(lblFilePath.Text), "PT_" + Path.GetFileName(lblFilePath.Text))))
                    //{
                    //    File.Delete(Path.Combine(Path.GetDirectoryName(lblFilePath.Text), "PT_" + Path.GetFileName(lblFilePath.Text)));
                    //}
                    //File.AppendAllText(Path.Combine(Path.GetDirectoryName(lblFilePath.Text),"PT_"+ Path.GetFileName(lblFilePath.Text) ), PlainText);
                    MessageBox.Show("File created.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch (Exception E)
            {
                MessageBox.Show(E.Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog objofd = new OpenFileDialog();

            objofd.ShowDialog();
            if (objofd.FileName.Trim().Length > 0)
            {
                lblFilePath.Text = objofd.FileName;
            }
        }
    }
}
