/*
MIDI CONTROL 2023

ControlMain.cs

- Description: Contains the application entry point
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Windows.Forms;

namespace MidiControl
{
    internal static class ControlMain
    {
        [STAThread]
        static void Main()
        {
            //Set the application layout
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Load the main configuration
            IControlConfig.Instance.LoadConfig();

            //Run the main application
            Application.Run(new MainWindow());
        }
    }
}
