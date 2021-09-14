using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UPaT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UnrealPakHelper UPH = new UnrealPakHelper();

        

        public MainWindow()
        {
            InitializeComponent();


            UPH.UPOutput += (string s) => {
                UpdateCMDOutputTextBox(s);
            };
            try
            {
                //Populate Drop Down
                foreach(string s in UPH.GetUnrealPakLocation())
                {
                    var ns = s.Remove(0, s.IndexOf("UE") + 3); //Remove everything before UE
                    ns = ns.Remove(ns.IndexOf(@"\"));
                    UnrealVersionSelectorDropdown.Items.Add(ns);
                }
                UnrealVersionSelectorDropdown.SelectedIndex = 0;

                ChosenUnrealPakExeFile.Text = UPH.FoundUpakExeLocations[UnrealVersionSelectorDropdown.SelectedIndex]; //Updating the Text Input

                UPH.SelectedUnrealPakExe = UPH.FoundUpakExeLocations[UnrealVersionSelectorDropdown.SelectedIndex]; // Updating the variable behind-the-scenes to tell UnrealPak what Exe to use.
            }
            catch (Exception e) {

                UnrealVersionSelectorDropdown.Items.Add("No UE Versions");
                UnrealVersionSelectorDropdown.SelectedIndex = 0;
            }
        }

        private void BrowseForPakButton_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            dialog.Title = "Select Pak";
            dialog.Filter = "Unreal Pak (*.pak)|*.pak";
            

            dialog.FileOk += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                ChosenPakFile.Text = dialog.FileName;

                string ParentDir = "..\\" + dialog.FileName;

                // this.Background = new ImageBrush(new BitmapImage(new Uri(@"C:\Program Files (x86)\Steam\steamapps\common\Halo The Master Chief Collection\mcc\Content\Splash\Splash.bmp"))); // new BitmapImage(new Uri(@"C:\Program Files (x86)\Steam\steamapps\common\Halo The Master Chief Collection\mcc\Content\Splash\Splash.bmp", UriKind.Absolute));
            };

            dialog.ShowDialog();
        }

        private void BrowseForUnrealPakExeButton_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            dialog.Title = "Select UnrealPak.exe";
            dialog.Filter = "Unreal Pak Executable (UnrealPak.exe)|UnrealPak.exe";

            
            dialog.FileOk += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                ChosenUnrealPakExeFile.Text = dialog.FileName;
            };

            
            dialog.ShowDialog();
        }

        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {

            UPH.SelectedUnrealPakExe = ChosenUnrealPakExeFile.Text;
            UPH.SelectedUnrealPak = ChosenPakFile.Text;
            UPH.OutputDirectory = OutputFolderText.Text;

            if (UPH.SelectedUnrealPak == "" || UPH.OutputDirectory == "" || UPH.SelectedUnrealPakExe == "")
            {
                CMDOutputText.AppendText("Game Pak, UnrealPak.exe or Output Directory is empty.\n");
                return;
            }

            UPH.UnpackUnrealPak();
        }

        private void BrowseForOutputFolder_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog FBD = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            
            if ((bool)FBD.ShowDialog())
            {
                OutputFolderText.Text = FBD.SelectedPath;
                
            }
        }

        private void UpdateCMDOutputTextBox(string append)
        {
            Dispatcher.Invoke(new Action(() => {
                CMDOutputText.AppendText(append + "\n");
                CMDOutputText.ScrollToEnd();
            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //Wait, my comments are out of order.
            if (UPH.UPT_IsRunning) //But check it's running first
            {
                UPH.proc?.Kill(); //Kill the UnrealPak process if it's running
            }
         
        }

        private void GitHubLinkBTN_Click(object sender, RoutedEventArgs e)
        {

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "https://github.com/Rhyce/UPaT",
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        private void UnrealVersionSelectorDropdown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChosenUnrealPakExeFile.Text = UPH.FoundUpakExeLocations[UnrealVersionSelectorDropdown.SelectedIndex];//Updating the Text Input
            UPH.SelectedUnrealPakExe = UPH.FoundUpakExeLocations[UnrealVersionSelectorDropdown.SelectedIndex];// Updating the variable behind-the-scenes to tell UnrealPak what Exe to use.
        }
    }
}
