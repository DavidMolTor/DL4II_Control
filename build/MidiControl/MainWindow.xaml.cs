/*
MIDI CONTROL 2023

MainWindow.cs

- Description: Main window control
- Author: David Molina Toro
- Date: 15 - 12 - 2023
- Version: 1.7
*/

using System.Windows;

namespace MidiControl
{
    public partial class MainWindow : Window
    {
        /*
        Public constructor
        */
        public MainWindow()
        {
            //Load the main configuration
            IControlConfig.Instance.LoadConfig();

            //Set the MIDI control system
            IControlMIDI.Instance.Initialize();

            InitializeComponent();
        }

        /*
        Window closing event handler
        */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Reset the MIDI control system
            IControlMIDI.Instance.Deinitialize();
        }
    }
}
