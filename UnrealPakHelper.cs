using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UPaT
{
    class UnrealPakHelper
    {
        public string SelectedUnrealPak { get; set; }

        public string OutputDirectory { get; set; }

        public string SelectedUnrealPakExe { get; set; }

        string BaseUnrealEngineLocation = "C:\\Program Files\\Epic Games";

        public delegate void CMDUnrealPakOutput(string Output);
        public CMDUnrealPakOutput UPOutput;

        public System.Diagnostics.Process proc = new System.Diagnostics.Process();
        public string GetUnrealPakLocation()
        {
            string[] locations = Directory.GetFiles(BaseUnrealEngineLocation, "UnrealPak.exe", SearchOption.AllDirectories);

            SelectedUnrealPakExe = locations[1];
            return locations[1];
        }

        public bool UnpackUnrealPak()
        {
            string startupArgs = $" -extract \"{SelectedUnrealPak}\" \"{OutputDirectory}\"";
            UPOutput(SelectedUnrealPakExe + startupArgs);

            System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo(SelectedUnrealPakExe, startupArgs);
            
            procInfo.RedirectStandardOutput = true;
            //must be false
            procInfo.UseShellExecute = false;

            procInfo.CreateNoWindow = true;

            proc.StartInfo = procInfo;

            proc.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler((sender, e) => {
                UPOutput(e.Data);
            });

            proc.Start();

            proc.BeginOutputReadLine();
            
            return true;
        }

    }
}
