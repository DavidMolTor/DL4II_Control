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
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.Generic;

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

            //Set the error handling call
            IControlMIDI.Instance.ErrorMessage  += HandleErrorMessage;

            //Set the alternative button event
            altButton.AltButtonPressed          += HandleAltButtonPressed;

            //Set the footswitch events
            footswitch_A.FootswitchPressed      += HandleFootswitchPressed;
            footswitch_A.FootswitchHold         += HandleFootswitchHold;
            footswitch_B.FootswitchPressed      += HandleFootswitchPressed;
            footswitch_B.FootswitchHold         += HandleFootswitchHold;
            footswitch_C.FootswitchPressed      += HandleFootswitchPressed;
            footswitch_C.FootswitchHold         += HandleFootswitchHold;

            //Set the tap and set footswitches
            footswitch_TAP.FootswitchPressed    += HandleFootswitchPressed;
            footswitch_SET.FootswitchPressed    += HandleFootswitchPressed;

            //Set the current preset configuration
            SetDevice(IControlConfig.Instance.GetPreset(IControlConfig.Instance.GetSelectedPreset()));

            //Initialize the device update timer
            Timer timerUpdateDevice     = new Timer(Constants.DEVICE_UPDATE_PERIOD);
            timerUpdateDevice.Elapsed   += TimerUpdateDevice_Elapsed;
            timerUpdateDevice.Enabled   = true;

            //Initialize the tempo blink task
            Task.Run(TempoBlinkTask);
        }

        //Current configuration structure
        bool bReload = false;
        DeviceConfig configDevice;

        //Current note subdivision variable
        int iCurrentSubdivision = 0;

        /*
        Sets the given configuration
        */
        private void SetDevice(DeviceConfig config)
        {
            //Set the reaload flag
            bReload = true;

            //Reset the current configuration
            configDevice = new DeviceConfig()
            {
                iDelaySelected  = -1,
                iDelayTime      = -1,
                iDelayNotes     = -1,
                iDelayRepeats   = -1,
                iDelayTweak     = -1,
                iDelayTweez     = -1,
                iDelayMix       = -1,
                iReverbSelected = -1,
                iReverbDecay    = -1,
                iReverbTweak    = -1,
                iReverbRouting  = -1,
                iReverbMix      = -1
            };

            //Get the currently selected preset
            int iPreset = IControlConfig.Instance.GetSelectedPreset();

            //Set the configurable elements
            textboxPreset.Text  = iPreset.ToString();
            textboxChannel.Text = IControlConfig.Instance.GetChannelMIDI().ToString();

            //Send the preset command if able
            IControlMIDI.Instance.AddCommand(ChannelCommand.ProgramChange, IControlConfig.Instance.GetChannelMIDI(), iPreset - 1);

            //Set the delay select control
            if (config.iDelaySelected < Constants.ALTDELAY_INITIAL)
            {
                knobDelaySelected.SetKnob(config.iDelaySelected, Constants.DICT_DELAY.Select(x => (int)x.Key).ToList(), false);
            }
            else
            {
                knobDelaySelected.SetKnob(config.iDelaySelected, Constants.DICT_LEGACY.Select(x => (int)x.Key).ToList(), false);
            }

            //Set all delay control knobs
            knobDelayTime.SetKnob(config.iDelayTime,        Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayRepeats.SetKnob(config.iDelayRepeats,  Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayTweak.SetKnob(config.iDelayTweak,      Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayTweez.SetKnob(config.iDelayTweez,      Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());
            knobDelayMix.SetKnob(config.iDelayMix,          Enumerable.Range(0, Constants.MAX_KNOB_VALUES).ToList());

            //Set the reverb select knob steps
            knobReverbSelected.SetKnob(config.iReverbSelected, Constants.DICT_REVERB.Select(x => (int)x.Key).ToList(), false);

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

            //Set the notes subdivision variable
            iCurrentSubdivision = config.iDelayNotes;
        }

        /*
        Device update timer elapsed function
        */
        private void TimerUpdateDevice_Elapsed(object source, ElapsedEventArgs e)
        {
            //Store the current configuration
            DeviceConfig configPrevious = new DeviceConfig(configDevice);

            //Set the new configuration parameters
            DeviceConfig configCurrent = new DeviceConfig()
            {
                iDelaySelected  = knobDelaySelected.Status,
                iDelayTime      = knobDelayTime.Status,
                iDelayNotes     = iCurrentSubdivision,
                iDelayRepeats   = knobDelayRepeats.Status,
                iDelayTweak     = knobDelayTweak.Status,
                iDelayTweez     = knobDelayTweez.Status,
                iDelayMix       = knobDelayMix.Status,
                iReverbSelected = knobReverbSelected.Status,
                iReverbDecay    = knobReverbDecay.Status,
                iReverbTweak    = knobReverbTweak.Status,
                iReverbRouting  = knobReverbRouting.Status,
                iReverbMix      = knobReverbMix.Status
            };

            //Get the selected MIDI channel
            int iChannel = IControlConfig.Instance.GetChannelMIDI();

            //Send the selected delay command if able
            if (configCurrent.iDelaySelected != configPrevious.iDelaySelected)
            {
                if (configCurrent.iDelaySelected == (int)DelayModels.Looper)
                {
                    //Enable the classic looper mode
                    IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.LooperMode, 64);
                }
                else
                {
                    //Disable the classic looper mode
                    IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.LooperMode, 0);

                    //Set the selected delay model
                    IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelaySelected, configCurrent.iDelaySelected);
                }
            }

            //Send the delay notes command if able
            if (configCurrent.iDelayNotes != configPrevious.iDelayNotes && configCurrent.iDelayTime == configPrevious.iDelayTime)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayNotes, configCurrent.iDelayNotes);

            //Send the delay time command if able
            if (configCurrent.iDelayTime != configPrevious.iDelayTime)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayTime, configCurrent.iDelayTime);

            //Send the delay repeats command if able
            if (configCurrent.iDelayRepeats != configPrevious.iDelayRepeats)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayRepeats, configCurrent.iDelayRepeats);

            //Send the delay tweak command if able
            if (configCurrent.iDelayTweak != configPrevious.iDelayTweak)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayTweak, configCurrent.iDelayTweak);

            //Send the delay tweez command if able
            if (configCurrent.iDelayTweez != configPrevious.iDelayTweez)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayTweez, configCurrent.iDelayTweez);

            //Send the delay mix command if able
            if (configCurrent.iDelayMix != configPrevious.iDelayMix)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.DelayMix, configCurrent.iDelayMix);

            //Send the reverb selected command if able
            if (configCurrent.iReverbSelected != configPrevious.iReverbSelected)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbSelected, configCurrent.iReverbSelected);

            //Send the reverb decay command if able
            if (configCurrent.iReverbDecay != configPrevious.iReverbDecay)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbDecay, configCurrent.iReverbDecay);

            //Send the reverb tweak command if able
            if (configCurrent.iReverbTweak != configPrevious.iReverbTweak)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbTweak, configCurrent.iReverbTweak);

            //Send the reverb routing command if able
            if (configCurrent.iReverbRouting != configPrevious.iReverbRouting)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbRouting, configCurrent.iReverbRouting);

            //Send the reverb mix command if able
            if (configCurrent.iReverbMix != configPrevious.iReverbMix)
                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.ReverbMix, configCurrent.iReverbMix);

            //Check the delay selector status and set the labels
            if (configCurrent.iDelaySelected < Constants.ALTDELAY_INITIAL)
            {
                string[] sParts = Constants.DICT_DELAY[(DelayModels)configCurrent.iDelaySelected].Split('|');
                Dispatcher.Invoke(new Action(() => labelDelayTweak.Content = sParts.First()));
                Dispatcher.Invoke(new Action(() => labelDelayTweez.Content = sParts.Last()));
            }
            else
            {
                string[] sParts = Constants.DICT_LEGACY[(LegacyModels)configCurrent.iDelaySelected].Split('|');
                Dispatcher.Invoke(new Action(() => labelDelayTweak.Content = sParts.First()));
                Dispatcher.Invoke(new Action(() => labelDelayTweez.Content = sParts.Last()));
            }

            //Set the reverb parameter labels
            Dispatcher.Invoke(new Action(() => labelReverbTweak.Content = Constants.DICT_REVERB[(ReverbModels)configCurrent.iReverbSelected]));
            switch ((ReverRouting)configCurrent.iReverbRouting)
            {
                case ReverRouting.ReverbDelay:
                    Dispatcher.Invoke(new Action(() => labelReverbRouting.Content = "REVERB ► DELAY"));
                    break;
                case ReverRouting.Parallel:
                    Dispatcher.Invoke(new Action(() => labelReverbRouting.Content = "REVERB  =  DELAY"));
                    break;
                case ReverRouting.DelayReverb:
                    Dispatcher.Invoke(new Action(() => labelReverbRouting.Content = "DELAY ► REVERB"));
                    break;
            }

            //Set the device configuration
            if (!bReload)
            {
                configDevice = new DeviceConfig(configCurrent);
            }
            else
            {
                bReload = false;
            }
        }

        /*
        Tempo blink task function
        */
        private void TempoBlinkTask()
        {
            while (true)
            {
                //Reset the footswitch blink status
                Dispatcher.Invoke(new Action(() => footswitch_TAP.SetStatus(FootswitchStatus.Off)));
                System.Threading.Thread.Sleep(Constants.DICT_SUBDIVISIONS[(TimeSubdivisions)iCurrentSubdivision]);

                //Set the footswitch blink status
                Dispatcher.Invoke(new Action(() => footswitch_TAP.SetStatus(FootswitchStatus.Red)));
                System.Threading.Thread.Sleep(Constants.FOOTSWITCH_BLINK_PERIOD);
            }
        }

        /*
        Sets the given message as an error
        */
        private void HandleErrorMessage(string sMessage)
        {
            Dispatcher.Invoke(new Action(() => labelError.Content = sMessage));
        }

        /*
        Alternative button pressed handler function
        */
        private void HandleAltButtonPressed(object sender, EventArgs e)
        {
            //Set the delay select knob list
            if (altButton.Status == AltButtonStatus.White)
            {
                int iSelected = Constants.DICT_DELAY.Keys.ToList().IndexOf((DelayModels)configDevice.iDelaySelected);
                knobDelaySelected.SetKnob((int)Constants.DICT_LEGACY.ElementAt(iSelected).Key, Constants.DICT_LEGACY.Select(x => (int)x.Key).ToList(), false);
            }
            else
            {
                int iSelected = Constants.DICT_LEGACY.Keys.ToList().IndexOf((LegacyModels)configDevice.iDelaySelected);
                knobDelaySelected.SetKnob((int)Constants.DICT_DELAY.ElementAt(iSelected).Key, Constants.DICT_DELAY.Select(x => (int)x.Key).ToList(), false);
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
            if (!footswitch_A.Blinking && !footswitch_B.Blinking && !footswitch_C.Blinking)
            {
                //Get the selected MIDI channel
                int iChannel = IControlConfig.Instance.GetChannelMIDI();

                //Check the type of footswitch
                switch (((Footswitch)sender).Name.Split('_').Last())
                {
                    case "A":
                    case "B":
                    case "C":
                        //Check the pressed button
                        switch (((Footswitch)sender).Status)
                        {
                            case FootswitchStatus.Off:
                                //Check the footswitch pressed
                                if (sender == footswitch_A)
                                {
                                    //Set the current configuration and store the selected preset
                                    DeviceConfig config = IControlConfig.Instance.GetPreset(1);
                                    IControlConfig.Instance.SaveSelectedPreset(1);
                                    SetDevice(config);
                                }
                                else if (sender == footswitch_B)
                                {
                                    //Set the current configuration and store the selected preset
                                    DeviceConfig config = IControlConfig.Instance.GetPreset(2);
                                    IControlConfig.Instance.SaveSelectedPreset(2);
                                    SetDevice(config);
                                }
                                else if (sender == footswitch_C)
                                {
                                    //Set the current configuration and store the selected preset
                                    DeviceConfig config = IControlConfig.Instance.GetPreset(3);
                                    IControlConfig.Instance.SaveSelectedPreset(3);
                                    SetDevice(config);
                                }
                                break;
                            case FootswitchStatus.Green:
                                //Send the preset bypass command
                                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.PresetBypass, 64);

                                //Set the footswitch status
                                ((Footswitch)sender).SetStatus(FootswitchStatus.Dim);
                                break;
                            case FootswitchStatus.Dim:
                                //Send the preset bypass command
                                IControlMIDI.Instance.AddCommand(ChannelCommand.Controller, iChannel, (int)SettingsCC.PresetBypass, 0);

                                //Set the footswitch status
                                ((Footswitch)sender).SetStatus(FootswitchStatus.Green);
                                break;
                        }

                        break;
                    case "TAP":
                        //Get the next note subdivision setting
                        List<int> listSubdivisions = Enum.GetValues(typeof(TimeSubdivisions)).Cast<int>().ToList();
                        int iDelayNotes = listSubdivisions.IndexOf(configDevice.iDelayNotes) + 1;

                        //Set the note subdivision variable
                        iCurrentSubdivision = iDelayNotes < listSubdivisions.Count ? iDelayNotes : 0;
                        break;
                    case "SET":
                        //Save the current preset
                        SaveCurrentPreset();
                        break;
                }
            }
        }

        /*
        Footswitch hold handler function
        */
        private void HandleFootswitchHold(object sender, EventArgs e)
        {
            SaveCurrentPreset();
        }

        /*
        Saves the current device configuration
        */
        private void SaveCurrentPreset()
        {
            int iPreset = 0;

            //Get the current selected preset
            Dispatcher.Invoke(new Action(() => iPreset = int.Parse(textboxPreset.Text)));

            //Check the preset number
            if (iPreset >= Constants.PRESET_COUNT_MIN && iPreset <= Constants.PRESET_COUNT_MAX)
            {
                IControlConfig.Instance.SavePreset(iPreset, configDevice);
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
                //Get the current selected preset
                if (int.TryParse(textboxPreset.Text, out int iPreset))
                {
                    //Check if the preset is outside bounds
                    iPreset = iPreset > Constants.PRESET_COUNT_MAX ? Constants.PRESET_COUNT_MAX : iPreset;
                    iPreset = iPreset < Constants.PRESET_COUNT_MIN ? Constants.PRESET_COUNT_MIN : iPreset;

                    //Save the selected preset
                    IControlConfig.Instance.SaveSelectedPreset(iPreset);

                    //Set the device configuration
                    DeviceConfig config = IControlConfig.Instance.GetPreset(iPreset);
                    SetDevice(config);
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
