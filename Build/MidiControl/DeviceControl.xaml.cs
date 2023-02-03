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
using System.Windows.Controls;

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

            DeviceConfig config = new DeviceConfig()
            {
                iDelaySelected  = 4,
                iDelayTime      = 0,
                iDelayRepeats   = 127,
                iDelayTweak     = 0,
                iDelayTweez     = 127,
                iDelayMix       = 0,
                iReverbSelected = 8,
                iReverbDecay    = 127,
                iReverbTweak    = 0,
                iReverbRouting  = 1,
                iReverbMix      = 127
            };
            
            IControlConfig.Instance.SavePreset(1, config);

            //Set the current preset configuration
            currentConfig = IControlConfig.Instance.GetPreset(IControlConfig.Instance.GetSelectedPreset());

            //Set the alternative button event
            altButton.AltButtonPressed += HandleAltButtonPressed;

            //Set the footswitch events
            footswitch_A.FootswitchPressed  += HandleFootswitchPressed;
            footswitch_A.FootswitchHold     += HandleFootswitchHold;
            footswitch_B.FootswitchPressed  += HandleFootswitchPressed;
            footswitch_B.FootswitchHold     += HandleFootswitchHold;
            footswitch_C.FootswitchPressed  += HandleFootswitchPressed;
            footswitch_C.FootswitchHold     += HandleFootswitchHold;

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
            //Set the delay select control
            if (currentConfig.iDelaySelected < Constants.ALTDELAY_INITIAL)
            {
                knobDelaySelect.SetKnob(currentConfig.iDelaySelected, Constants.LIST_DELAY.Select(x => (int)x).ToList(), false);
            }
            else
            {
                knobDelaySelect.SetKnob(currentConfig.iDelaySelected, Constants.LIST_LEGACY.Select(x => (int)x).ToList(), false);
            }

            //Set all delay control knobs
            knobDelayTime.SetKnob(currentConfig.iDelayTime,         Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayRepeats.SetKnob(currentConfig.iDelayRepeats,   Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayTweak.SetKnob(currentConfig.iDelayTweak,       Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayTweez.SetKnob(currentConfig.iDelayTweez,       Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayMix.SetKnob(currentConfig.iDelayMix,           Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());

            //Set the reverb select knob steps
            knobReverbSelect.SetKnob(currentConfig.iReverbSelected, Constants.LIST_REVERB.Select(x => (int)x).ToList(), false);

            //Set all delay control knobs
            knobReverbDecay.SetKnob(currentConfig.iReverbDecay,     Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobReverbTweak.SetKnob(currentConfig.iReverbTweak,     Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobReverbRouting.SetKnob(currentConfig.iReverbRouting, Enum.GetValues(typeof(ReverRouting)).Cast<ReverRouting>().Select(x => (int)x).ToList());
            knobReverbMix.SetKnob(currentConfig.iReverbMix,         Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());

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
            //Set the delay select knob list
            if (altButton.Status == AltButtonStatus.White)
            {
                knobDelaySelect.SetKnob(knobDelaySelect.Status, Constants.LIST_LEGACY.Select(x => (int)x).ToList(), false);
            }
            else
            {
                knobDelaySelect.SetKnob(knobDelaySelect.Status, Constants.LIST_DELAY.Select(x => (int)x).ToList(), false);
            }

            //Set the alternative button status
            altButton.SetStatus(altButton.Status == AltButtonStatus.White ? AltButtonStatus.Green : AltButtonStatus.White);
        }

        /*
        Footswitch pressed handler function
        */
        private void HandleFootswitchPressed(object sender, EventArgs e)
        {
            //Check if any footswitch is blinking
            if (!footswitch_A.Blinking && !footswitch_B.Blinking && !footswitch_C.Blinking && !footswitch_TAP.Blinking)
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
        }

        /*
        Footswitch hold handler function
        */
        private void HandleFootswitchHold(object sender, EventArgs e)
        {
            int iPreset = 0;

            //Get the current selected preset
            Dispatcher.Invoke(new Action(() => iPreset = int.Parse(textboxPreset.Text)));

            //Check the preset number
            if (iPreset >= Constants.PRESET_COUNT_MIN && iPreset <= Constants.PRESET_COUNT_MAX)
            {
                IControlConfig.Instance.SavePreset(iPreset, currentConfig);
            }
            else
            {
                Console.WriteLine("Error: Preset number outside bounds");
            }
        }

        /*
        Check if the input character is a number
        */
        private void TextBoxNumber_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text.ToCharArray()[0]);
        }
    }
}
