/*
MIDI CONTROL 2023

AltButton.cs

- Description: Alternative button element class
- Author: David Molina Toro
- Date: 03 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MidiControl
{
    public partial class AltButton : UserControl
    {
        /*
        Public constructor
        */
        public AltButton()
        {
            InitializeComponent();
        }

        //Current alternative button status variable
        public AltButtonStatus Status = AltButtonStatus.White;

        /*
        Sets the alternative button status
        */
        public void SetStatus(AltButtonStatus status)
        {
            //Set the current status
            Status = status;

            //Set the proper image
            switch (status)
            {
                case AltButtonStatus.White:
                    imageAlt.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Alt White.png"));
                    break;
                case AltButtonStatus.Green:
                    imageAlt.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Alt Green.png"));
                    break;
            }
        }

        //Alternative button pressed event
        public EventHandler AltButtonPressed;

        /*
        Button clicked event handler
        */
        private void ButtonAlt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            AltButtonPressed?.Invoke(this, EventArgs.Empty);
        }
    }
}
