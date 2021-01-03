using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DeployToLinux
{
    public partial class mainForm : Form
    {
        String preview = null;
        public mainForm()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            var sampleFilePath = @"Data\SmapleConfig.txt";
            if (!File.Exists(sampleFilePath))
            {
                MessageBox.Show("Sample config file is not available");
                Application.Exit();
            }

            var tbs = new List<TextBox> { tbAppBinaryHome, tbAppDescription, tbAppDll, tbAppFolderName, tbAppName, tbIPAddress, tbIPPort, };
            if (CheckTextBoxValue(tbs))
            {
                MessageBox.Show("All entries is required");
                return;
            }

            string text = File.ReadAllText(sampleFilePath);
            preview = text.Replace("{{home}}", tbAppBinaryHome.Text)
                .Replace("{{ip}}", tbIPAddress.Text)
                .Replace("{{port}}", tbIPPort.Text)
                .Replace("{{appname}}", tbAppName.Text.ToLower().Trim().Replace(" ", "_"))
                .Replace("{{Description}}", tbAppDescription.Text)
                .Replace("{{foldername}}", tbAppFolderName.Text)
                .Replace("{{appdll}}", tbAppDll.Text);
            
            var createFile = MessageBox.Show(preview, "Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (createFile == DialogResult.Yes)
            {
                string strPath = Environment.GetFolderPath(
                         System.Environment.SpecialFolder.DesktopDirectory);

                if (!Directory.Exists(Path.Combine(strPath, $@"ConfigFolder")))
                {
                    Directory.CreateDirectory(Path.Combine(strPath, $@"ConfigFolder"));
                }

                string createFilePath = Path.Combine(strPath, $@"ConfigFolder\{tbAppName.Text.ToLower().Trim().Replace(" ", "_")}.txt");
                FileStream fs = new FileStream(createFilePath, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(preview);
                sw.Flush();
                sw.Close();
                MessageBox.Show("Completed");
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            MessageBox.Show(preview);
        }

        bool CheckTextBoxValue(List<TextBox> textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
