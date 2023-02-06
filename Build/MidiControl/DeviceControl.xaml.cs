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
using System.Timers;
using System.Windows.Controls;

//MIDI libraries
using Sanford.Multimedia.Midi;

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

            //Set the current preset configuration
            configCurrent = IControlConfig.Instance.GetPreset(IControlConfig.Instance.GetSelectedPreset());

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
            SetConfiguration(configCurrent);

            //Initialize the device connection timer
            Timer timerConnection   = new Timer(Constants.CONNECTION_PERIOD);
            timerConnection.Elapsed += TimerConnection_Elapsed;
            timerConnection.Enabled = true;

            //Initialize the device update timer
            Timer timerUpdateDevice     = new Timer(Constants.DEVICE_UPDATE_PERIOD);
            timerUpdateDevice.Elapsed   += TimerUpdateDevice_Elapsed;
            timerUpdateDevice.Enabled   = true;
        }

        //Current configuration structure
        DeviceConfig configCurrent = new DeviceConfig();

        /*
        Sets the given configuration
        */
        private void SetConfiguration(DeviceConfig config)
        {
            //Set the configurable elements
            textboxChannel.Text = IControlConfig.Instance.GetChannelMIDI().ToString();
            textboxPreset.Text  = IControlConfig.Instance.GetSelectedPreset().ToString();

            //Set the delay select control
            if (config.iDelaySelected < Constants.ALTDELAY_INITIAL)
            {
                knobDelaySelected.SetKnob(config.iDelaySelected, Constants.LIST_DELAY.Select(x => (int)x).ToList(), false);
            }
            else
            {
                knobDelaySelected.SetKnob(config.iDelaySelected, Constants.LIST_LEGACY.Select(x => (int)x).ToList(), false);
            }

            //Set all delay control knobs
            knobDelayTime.SetKnob(config.iDelayTime,        Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayRepeats.SetKnob(config.iDelayRepeats,  Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayTweak.SetKnob(config.iDelayTweak,      Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayTweez.SetKnob(config.iDelayTweez,      Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayMix.SetKnob(config.iDelayMix,          Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());

            //Set the reverb select knob steps
            knobReverbSelected.SetKnob(config.iReverbSelected, Constants.LIST_REVERB.Select(x => (int)x).ToList(), false);

            //Set the reverb routing knob steps
            knobReverbRouting.SetKnob(config.iReverbRouting, Enum.GetValues(typeof(ReverRouting)).Cast<ReverRouting>().Select(x => (int)x).ToList());

            //Set all delay control knobs
            knobReverbDecay.SetKnob(config.iReverbDecay,    Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobReverbTweak.SetKnob(config.iReverbTweak,    Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobReverbMix.SetKnob(config.iReverbMix,        Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());

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
            if (config.iDelaySelected < Constants.ALTDELAY_INITIAL)
            {
                altButton.SetStatus(AltButtonStatus.White);
            }
            else
            {
                altButton.SetStatus(AltButtonStatus.Green);
            }
        }

        /*
        Device connection timer elapsed function
        */
        private void TimerConnection_Elapsed(object source, ElapsedEventArgs e)
        {
            //Dispatcher.Invoke(new Action(() => this.IsEnabled = IControlMIDI.Instance.ConnectDevice()));
        }

        /*
        Device update timer elapsed function
        */
        private void TimerUpdateDevice_Elapsed(object source, ElapsedEventArgs e)
        {
            UpdateDevice(false);
        }

        /*
        Updates the device status
        */
        private void UpdateDevice(bool bForce)
        {
            //Store the current configuration
            DeviceConfig configPrevious = new DeviceConfig(configCurrent);

            //Set the new configuration parameters
            configCurrent.iDelaySelected    = knobDelaySelected.Status;
            configCurrent.iDelayTime        = knobDelayTime.Status;
            configCurrent.iDelayRepeats     = knobDelayRepeats.Status;
            configCurrent.iDelayTweak       = knobDelayTweak.Status;
            configCurrent.iDelayTweez       = knobDelayTweez.Status;
            configCurrent.iDelayMix         = knobDelayMix.Status;
            configCurrent.iReverbSelected   = knobReverbSelected.Status;
            configCurrent.iReverbDecay      = knobReverbDecay.Status;
            configCurrent.iReverbTweak      = knobReverbTweak.Status;
            configCurrent.iReverbRouting    = knobReverbRouting.Status;
            configCurrent.iReverbMix        = knobReverbMix.Status;

            //Get the channel selected
            int iChannel = IControlConfig.Instance.GetChannelMIDI();

            //Send the selected delay command if able
            if (configCurrent.iDelaySelected != configPrevious.iDelaySelected || bForce)
            {
                if (configCurrent.iDelaySelected == (int)DelayModels.Looper)
                {
                    //Enable the classic looper mode
                    IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.LooperMode, 64);
                }
                else
                {
                    //Disable the classic looper mode
                    IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.LooperMode, 0);

                    //Set the selected delay model
                    IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelaySelected, configCurrent.iDelaySelected);
                }
            }

            //Send the delay time command if able
            if (configCurrent.iDelayTime != configPrevious.iDelayTime || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayTime, configCurrent.iDelayTime);

            //Send the delay repeats command if able
            if (configCurrent.iDelayRepeats != configPrevious.iDelayRepeats || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayRepeats, configCurrent.iDelayRepeats);

            //Send the delay tweak command if able
            if (configCurrent.iDelayTweak != configPrevious.iDelayTweak || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayTweak, configCurrent.iDelayTweak);

            //Send the delay tweez command if able
            if (configCurrent.iDelayTweez != configPrevious.iDelayTweez || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayTweez, configCurrent.iDelayTweez);

            //Send the delay mix command if able
            if (configCurrent.iDelayMix != configPrevious.iDelayMix || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayMix, configCurrent.iDelayMix);

            //Send the reverb selected command if able
            if (configCurrent.iReverbSelected != configPrevious.iReverbSelected || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbSelected, configCurrent.iReverbSelected);

            //Send the reverb decay command if able
            if (configCurrent.iReverbDecay != configPrevious.iReverbDecay || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbDecay, configCurrent.iReverbDecay);

            //Send the reverb tweak command if able
            if (configCurrent.iReverbTweak != configPrevious.iReverbTweak || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbTweak, configCurrent.iReverbTweak);

            //Send the reverb routing command if able
            if (configCurrent.iReverbRouting != configPrevious.iReverbRouting || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbRouting, configCurrent.iReverbRouting);

            //Send the reverb mix command if able
            if (configCurrent.iReverbMix != configPrevious.iReverbMix || bForce)
                IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbMix, configCurrent.iReverbMix);
        }

        /*
        Alternative button pressed handler function
        */
        private void HandleAltButtonPressed(object sender, EventArgs e)
        {
            //Set the delay select knob list
            if (altButton.Status == AltButtonStatus.White)
            {
                int iSelected = Constants.LIST_DELAY.IndexOf((DelayModels)configCurrent.iDelaySelected);
                knobDelaySelected.SetKnob((int)Constants.LIST_LEGACY[iSelected], Constants.LIST_LEGACY.Select(x => (int)x).ToList(), false);
                configCurrent.iDelaySelected = (int)Constants.LIST_LEGACY[iSelected];
            }
            else
            {
                int iSelected = Constants.LIST_LEGACY.IndexOf((LegacyModels)configCurrent.iDelaySelected);
                knobDelaySelected.SetKnob((int)Constants.LIST_DELAY[iSelected], Constants.LIST_DELAY.Select(x => (int)x).ToList(), false);
                configCurrent.iDelaySelected = (int)Constants.LIST_DELAY[iSelected];
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
                //Get the channel selected
                int iChannel = IControlConfig.Instance.GetChannelMIDI();

                //Check the pressed button
                switch (((Footswitch)sender).Status)
                {
                    case FootswitchStatus.Off:
                        //Check the footswitch pressed
                        if (sender == footswitch_A)
                        {
                            //Send the preset change command
                            IControlMIDI.Instance.SendCommand(ChannelCommand.ProgramChange, iChannel, 0);

                            //Set the current configuration and store the selected preset
                            configCurrent = IControlConfig.Instance.GetPreset(1);
                            IControlConfig.Instance.SaveSelectedPreset(1);
                        }
                        else if (sender == footswitch_B)
                        {
                            //Send the preset change command
                            IControlMIDI.Instance.SendCommand(ChannelCommand.ProgramChange, iChannel, 1);

                            //Set the current configuration and store the selected preset
                            configCurrent = IControlConfig.Instance.GetPreset(2);
                            IControlConfig.Instance.SaveSelectedPreset(2);
                        }
                        else if (sender == footswitch_C)
                        {
                            //Send the preset change command
                            IControlMIDI.Instance.SendCommand(ChannelCommand.ProgramChange, iChannel, 2);

                            //Set the current configuration and store the selected preset
                            configCurrent = IControlConfig.Instance.GetPreset(3);
                            IControlConfig.Instance.SaveSelectedPreset(3);
                        }

                        //Update the device settings
                        SetConfiguration(configCurrent);
                        break;
                    case FootswitchStatus.Green:
                        //Send the preset bypass command
                        IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.PresetBypass, 64);

                        //Set the footswitch status
                        ((Footswitch)sender).SetStatus(FootswitchStatus.Dim);
                        break;
                    case FootswitchStatus.Dim:
                        //Send the preset bypass command
                        IControlMIDI.Instance.SendCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.PresetBypass, 0);

                        //Set the footswitch status
                        ((Footswitch)sender).SetStatus(FootswitchStatus.Green);
                        break;
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
                IControlConfig.Instance.SavePreset(iPreset, configCurrent);
            }
            else
            {
                Console.WriteLine("Error: Preset number outside bounds");
            }
        }

        /*
        Channel text changed event handler
        */
        private void TextboxChannel_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check if the channel text is empty
            if (!string.IsNullOrEmpty(textboxChannel.Text))
            {
                int iChannel = 0;

                //Get the current selected MIDI channel
                Dispatcher.Invoke(new Action(() => iChannel = int.Parse(textboxChannel.Text)));

                //Save the selected MIDI channel
                IControlConfig.Instance.SaveChannelMIDI(iChannel);
            }
        }

        /*
        Preset text changed event handler
        */
        private void TextboxPreset_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check if the preset text is empty
            if (!string.IsNullOrEmpty(textboxPreset.Text))
            {
                int iPreset;

                //Get the current selected preset
                bool bParse = int.TryParse(textboxPreset.Text, out iPreset);

                if (bParse)
                {
                    //Check if the preset is outside bounds
                    iPreset = iPreset > Constants.PRESET_COUNT_MAX ? Constants.PRESET_COUNT_MAX : iPreset;
                    iPreset = iPreset < Constants.PRESET_COUNT_MIN ? Constants.PRESET_COUNT_MIN : iPreset;

                    //Save the delected preset and update the device
                    IControlConfig.Instance.SaveSelectedPreset(iPreset);
                    configCurrent = IControlConfig.Instance.GetPreset(iPreset);
                    SetConfiguration(configCurrent);
                }
                else
                {
                    Console.WriteLine("Error: Preset number outside bounds");
                }
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
