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
        private string storePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "Windows.ApplicationModel.Store.dll");
        private string gamingServicesPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "GamingServices.dll");
        
        public Form1()
        {
            InitializeComponent();
        }

        private bool ProcessDllFile(string dllPath, byte[] x64Resource, byte[] x86Resource, ref int progressValue)
        {
            if (File.Exists(dllPath))
            {
                label.Text = $"Taking ownership of {Path.GetFileName(dllPath)}...";
                Process process = new Process
                {
                    StartInfo = new ProcessStartInfo("cmd.exe", $"/c takeown /f \"{dllPath}\" && icacls \"{dllPath}\" /grant *S-1-3-4:F /t /c /l")
                };
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
                progressBar1.Value = ++progressValue;

                try
                {
                    label.Text = $"Killing processes locking {Path.GetFileName(dllPath)}...";
                    List<Process> processes = FileUtil.WhoIsLocking(dllPath);

                    foreach (Process proc in processes)
                    {
                        Console.WriteLine(proc);
                        proc.Kill();
                    }
                    progressBar1.Value = ++progressValue;
                }
                catch
                {
                    label.Text = $"There is an error while killing the processes for {Path.GetFileName(dllPath)}!";
                    close.Show();
                    return false;
                }

                try
                {
                    label.Text = $"Done killing processes, now removing {Path.GetFileName(dllPath)}...";
                    File.Delete(dllPath);
                    progressBar1.Value = ++progressValue;
                }
                catch
                {
                    label.Text = $"Fail removing the file: \"{dllPath}\"!";
                    close.Show();
                    return false;
                }
            }

            // Only replace if we have the resource
            if (x64Resource != null || x86Resource != null)
            {
                try
                {
                    label.Text = $"Replacing {Path.GetFileName(dllPath)} with a new one...";
                    byte[] file;
                    if (Environment.Is64BitOperatingSystem) file = x64Resource;
                    else file = x86Resource;
                    
                    if (file != null)
                    {
                        File.WriteAllBytes(dllPath, file);
                    }
                    progressBar1.Value = ++progressValue;
                }
                catch
                {
                    label.Text = $"Fail replacing the file: \"{dllPath}\"!";
                    close.Show();
                    return false;
                }
            }
            
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Each DLL requires 4 steps: takeown, kill processes, delete, replace
            const int stepsPerDll = 4;
            const int numberOfDlls = 2;
            progressBar1.Maximum = stepsPerDll * numberOfDlls;
            int progressValue = 0;
            
            // Process Windows Store DLL (legacy support for older Minecraft versions)
            if (!ProcessDllFile(storePath, 
                Properties.Resources.Windows_ApplicationModel_Store, 
                Properties.Resources.Windows_ApplicationModel_Store_x86, 
                ref progressValue))
            {
                return;
            }

            // Process Gaming Services DLL (new SDK method for current Minecraft versions)
            // NOTE: GamingServices resources are not yet available. See GAMING_SERVICES_SETUP.md
            // for instructions on adding the cracked Gaming Services DLL files.
            // Until resources are added, this will only remove the existing GamingServices.dll
            // but will not replace it with a cracked version.
            if (!ProcessDllFile(gamingServicesPath, 
                null, // Properties.Resources.GamingServices when available
                null, // Properties.Resources.GamingServices_x86 when available
                ref progressValue))
            {
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
