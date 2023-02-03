/*
MIDI CONTROL 2023

Footswitch.cs

- Description: Footswitch element class
- Author: David Molina Toro
- Date: 03 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MidiControl
{
    public partial class Footswitch : UserControl
    {
        /*
        Public constructor
        */
        public Footswitch()
        {
            InitializeComponent();
        }

        //Current footswitch status variable
        public FootswitchStatus Status = FootswitchStatus.Off;

        /*
        Sets the footswitch status
        */
        public void SetStatus(FootswitchStatus status)
        {
            Status = status;

            switch (status)
            {
                case FootswitchStatus.Off:
                    imageFootswitch.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot None.png"));
                    break;
                case FootswitchStatus.Green:
                    imageFootswitch.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Green.png"));
                    break;
                case FootswitchStatus.Red:
                    imageFootswitch.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Red.png"));
                    break;
            }
        }

        //Footswitch pressed event
        public EventHandler FootswitchPressed;

        /*
        Button clicked event handler
        */
        private void ButtonFootswitch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FootswitchPressed?.Invoke(this, e);
        }
    }
}
