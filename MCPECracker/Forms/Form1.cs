using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCPECracker.Forms
{
    public partial class Form1 : Form
    {
        private string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "Windows.ApplicationModel.Store.dll");
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 4;
            if (File.Exists(path))
            {
                label.Text = "Taking ownership...";
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo("cmd.exe", $"/c takeown /f \"{path}\" && icacls \"{path}\" /grant *S-1-3-4:F /t /c /l")
                };
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                progressBar1.Value = 1;

                try
                {
                    label.Text = "Killing processes...";
                    List<Process> processes = FileUtil.WhoIsLocking(path);

                    foreach (Process proc in processes)
                    {
                        Console.WriteLine(proc);
                        proc.Kill();
                    }
                    progressBar1.Value = 2;
                }
                catch
                {
                    label.Text = "There is an error while killing the processes!";
                    close.Show();
                    return;
                }

                try
                {
                    label.Text = "Done killing processes, now removing the file...";
                    File.Delete(path);
                    progressBar1.Value = 3;
                }
                catch
                {
                    label.Text = $"Fail removing the file: \"{path}\"!";
                    close.Show();
                    return;
                }
            }

            try
            {
                label.Text = "File removed or not exist, replacing it with a new one...";
                byte[] file;
                if (Environment.Is64BitOperatingSystem) file = Properties.Resources.Windows_ApplicationModel_Store;
                else file = Properties.Resources.Windows_ApplicationModel_Store_x86;
                File.WriteAllBytes(path, file);
                progressBar1.Value = 4;
            }
            catch
            {
                label.Text = $"Fail replacing the file: \"{path}\"!";
                close.Show();
                return;
            }
            label.Text = "Successfully cracked your MCPE, relaunch Minecraft for Windows to play the full version!";
            close.Show();
        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
