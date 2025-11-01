using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCPECracker
{
    public partial class Form1 : Form
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9A1-45fd-A8CF-72F04E6BDE8F}");

        private bool IsRunAsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public Form1()
        {
            InitializeComponent();

            if (!IsRunAsAdministrator())
            {
                Process process = new Process()
                {
                    StartInfo =
                    {
                        FileName = Assembly.GetExecutingAssembly().Location,
                        UseShellExecute = true,
                        Verb ="runas",
                    }
                };
                process.Start();

                Environment.Exit(0);
            }
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                MessageBox.Show("You can only start one instance at a time", Assembly.GetExecutingAssembly().GetName().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            else
            {
                mutex.ReleaseMutex();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            mainPanel.Controls.Clear();
            Form form = new Forms.Form1
            {
                TopLevel = false,
                AutoScroll = true,
                FormBorderStyle = FormBorderStyle.None,
            };
            mainPanel.Controls.Add(form);
            form.Show();
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            string storePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "Windows.ApplicationModel.Store.dll");
            string gamingServicesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GamingServices.dll");
            
            // Revert Store DLL
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo("cmd.exe", $"/c sfc /scanfile={storePath}")
            };
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            
            // Revert Gaming Services DLL
            Process process2 = new Process
            {
                StartInfo = new ProcessStartInfo("cmd.exe", $"/c sfc /scanfile={gamingServicesPath}")
            };
            process2.StartInfo.CreateNoWindow = true;
            process2.Start();
            process2.WaitForExit();
            
            MessageBox.Show($"Revert all changes for both DLLs:\n\"{storePath}\"\n\"{gamingServicesPath}\"");
        }
    }
}
