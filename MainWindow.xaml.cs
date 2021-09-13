using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

            UnrealPakLocation.Text = UPH.GetUnrealPakLocation();
        }

        private void BrowseForPakButton_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            dialog.Title = "Select Pak";
            dialog.Filter = "Unreal Pak (*.pak)|*.pak";

            dialog.FileOk += (object sender, System.ComponentModel.CancelEventArgs e) =>
            {
                ChosenPakFile.Text = dialog.FileName;
            };

            dialog.ShowDialog();



        }

        private void ExtractButton_Click(object sender, RoutedEventArgs e)
        {
            UPH.SelectedUnrealPak = ChosenPakFile.Text;
            UPH.OutputDirectory = OutputFolderText.Text;

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
            UPH.proc.Kill();
        }
    }
}
