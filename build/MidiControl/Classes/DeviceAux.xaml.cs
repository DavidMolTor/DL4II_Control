/*
MIDI CONTROL 2023

DeviceAux.cs

- Description: Auxiliar device layout
- Author: David Molina Toro
- Date: 06 - 02 - 2023
- Version: 1.7
*/

using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace MidiControl
{
    public partial class DeviceAux : UserControl
    {
        /*
        Public constructor
        */
        public DeviceAux()
        {
            InitializeComponent();
        }

        /*
        Information button click function
        */
        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/DavidMolTor/DL4II_Control");
        }

        /*
        Information button mouse enter function
        */
        private void ButtonInfo_MouseEnter(object sender, MouseEventArgs e)
        {
            textInfo.Visibility = Visibility.Visible;
        }

        /*
        Information button mouse leave function
        */
        private void ButtonInfo_MouseLeave(object sender, MouseEventArgs e)
        {
            textInfo.Visibility = Visibility.Hidden;
        }
    }
}
