/*
MIDI CONTROL 2023

DeviceControl.cs

- Description: Device control panel for DL4
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MidiControl
{
    public partial class DeviceControl : UserControl
    {
        /*
        Public constructor
        */
        public DeviceControl()
        {
            InitializeComponent();

            //Set the configurable elements
            textboxChannel.Text = IControlConfig.Instance.GetChannelMIDI().ToString();
            textboxPreset.Text  = IControlConfig.Instance.GetSelectedPreset().ToString();

            //Set the current preset configuration
            currentConfig = IControlConfig.Instance.GetPreset(IControlConfig.Instance.GetSelectedPreset());

            //Set the alternative button event
            altButton.AltButtonPressed += HandleAltButtonPressed;

            //Set the footswitch events
            footswitch_A.FootswitchPressed += HandleFootswitchPressed;
            footswitch_B.FootswitchPressed += HandleFootswitchPressed;
            footswitch_C.FootswitchPressed += HandleFootswitchPressed;

            //Update the device controls
            UpdateDevice();
        }

        //Current configuration structure
        DeviceConfig currentConfig = new DeviceConfig();

        /*
        Public constructor
        */
        private void UpdateDevice()
        {            
            //Set the delay select knob steps
            if (currentConfig.iDelaySelected < Constants.ALTDELAY_INITIAL)
            {
                knobDelaySelect.listSteps = Enum.GetValues(typeof(DelayModels)).Cast<DelayModels>().Select(x => (int)x).ToList();
            }
            else
            {
                knobDelaySelect.listSteps = Enum.GetValues(typeof(LegacyModels)).Cast<LegacyModels>().Select(x => (int)x).ToList();
            }

            //Set the reverb select knob steps
            knobReverbSelect.listSteps = Enum.GetValues(typeof(ReverbModels)).Cast<ReverbModels>().Select(x => (int)x).ToList();

            //Reset all footswitches
            footswitch_A.SetStatus(FootswitchStatus.Off);
            footswitch_B.SetStatus(FootswitchStatus.Off);
            footswitch_C.SetStatus(FootswitchStatus.Off);

            //Set the active footswitch
            switch (IControlConfig.Instance.GetSelectedPreset() % 3)
            {
                case 0:
                    footswitch_C.SetStatus(FootswitchStatus.Green);
                    break;
                case 1:
                    footswitch_A.SetStatus(FootswitchStatus.Green);
                    break;
                case 2:
                    footswitch_B.SetStatus(FootswitchStatus.Green);
                    break;
            }

            //Set the alternative button status
            if (currentConfig.iDelaySelected < Constants.ALTDELAY_INITIAL)
            {
                altButton.SetStatus(AltButtonStatus.White);
            }
            else
            {
                altButton.SetStatus(AltButtonStatus.Green);
            }
        }

        /*
        Alternative button pressed handler function
        */
        private void HandleAltButtonPressed(object sender, EventArgs e)
        {
            altButton.SetStatus(altButton.Status == AltButtonStatus.White ? AltButtonStatus.Green : AltButtonStatus.White);
        }

        /*
        Footswitch pressed handler function
        */
        private void HandleFootswitchPressed(object sender, EventArgs e)
        {
            //Check the pressed button
            if (((Footswitch)sender).Status != FootswitchStatus.Green)
            {
                //Reset all footswitches
                footswitch_A.SetStatus(FootswitchStatus.Off);
                footswitch_B.SetStatus(FootswitchStatus.Off);
                footswitch_C.SetStatus(FootswitchStatus.Off);

                //Set the selected one
                ((Footswitch)sender).SetStatus(FootswitchStatus.Green);
            }
        }

        /*
        Check if the input character is a number
        */
        private void TextBoxNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text.ToCharArray()[0]);
        }

        /*
        Saves the current device configuration
        */
        private void ButtonSavePreset_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int iPreset = int.Parse(textboxPreset.Text);
            if (iPreset >= Constants.PRESET_COUNT_MIN && iPreset <= Constants.PRESET_COUNT_MAX)
            {
                IControlConfig.Instance.SavePreset(iPreset, currentConfig);
            }
            else
            {
                Console.WriteLine("Error: Preset number outside bounds");
            }
        }
    }
}
