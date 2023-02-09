/*
MIDI CONTROL 2023

Footswitch.cs

- Description: Footswitch element class
- Author: David Molina Toro
- Date: 03 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Threading;
using System.Threading.Tasks;
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

        //Blinking status variable
        public bool Blinking { get; private set; }

        /*
        Sets the footswitch status
        */
        public void SetStatus(FootswitchStatus status)
        {
            //Set the current status
            Status = status;

            //Set the proper image
            switch (Status)
            {
                case FootswitchStatus.Off:
                    imageFootswitch.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot None.png"));
                    break;
                case FootswitchStatus.Green:
                    imageFootswitch.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Green.png"));
                    break;
                case FootswitchStatus.Dim:
                    imageFootswitch.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Dim.png"));
                    break;
                case FootswitchStatus.Red:
                    imageFootswitch.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Red.png"));
                    break;
            }
        }

        //Footswitch pressed event
        public EventHandler FootswitchPressed;

        //Footswitch hold event
        public EventHandler FootswitchHold;

        //Hold status variable
        private bool bHolding = false;

        /*
        Mouse press function for footswitch element
        */
        private void Footswitch_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            //Make the footswitch capture the mouse
            imageFootswitch.CaptureMouse();

            //Check the current status
            if (Status == FootswitchStatus.Green)
            {
                //Reset the holding status
                bHolding = true;

                //Start the hold task
                Task.Run(() => Footswitch_MouseHold(sender, e));
            }
        }

        /*
        Mouse hold thread for footswitch element
        */
        public void Footswitch_MouseHold(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Check for cancellation
            long iTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            while (bHolding && DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond - iTimestamp < Constants.FOOTSWITCH_HOLD_TIME)
            {
                Task.Delay(10);
            }

            //Check if the footswitch has been held
            if (bHolding)
            {
                //Invoke the footswitch hold event
                FootswitchHold?.Invoke(this, EventArgs.Empty);

                //Set the blinking status
                Blinking = true;

                //Start the blinking task
                for (int i = 0; i < Constants.FOOTSWITCH_BLINK_COUNT; i++)
                {
                    Thread.Sleep(Constants.FOOTSWITCH_BLINK_PERIOD);
                    Dispatcher.Invoke(new Action(() => SetStatus(FootswitchStatus.Off)));
                    Thread.Sleep(Constants.FOOTSWITCH_BLINK_PERIOD);
                    Dispatcher.Invoke(new Action(() => SetStatus(FootswitchStatus.Green)));
                }

                //Reset the blinking status
                Blinking = false;
            }
        }

        /*
        Mouse release function for footswitch element
        */
        private void Footswitch_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Check if the footswitch is currently blinking
            if (!Blinking)
            {
                //Check the holding status
                if (bHolding)
                {
                    //Cancel the holding thread
                    bHolding = false;

                    //Send the footswitch pressed event
                    FootswitchPressed?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    //Send the footswitch pressed event
                    FootswitchPressed?.Invoke(this, EventArgs.Empty);
                }
            }

            //Release the mouse
            imageFootswitch.ReleaseMouseCapture();
        }
    }
}
