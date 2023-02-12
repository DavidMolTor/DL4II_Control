/*
MIDI CONTROL 2023

IControlConfig.cs

- Description: Configuration control instance
- Author: David Molina Toro
- Date: 02 - 02 - 2023
- Version: 0.1
*/

using System.Configuration;

namespace MidiControl
{
    public sealed class IControlConfig
    {
        private static volatile IControlConfig instance = null;
        private static readonly object padlock = new object();

        /*
        Instance object for the singleton instantiation
        */
        public static IControlConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new IControlConfig();
                        }
                    }
                }

                return instance;
            }
        }

        //Main configuration object
        Configuration configMain;

        /*
        Load the application configuration file
        */
        public bool LoadConfig()
        {
            configMain = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            return true;
        }

        /*
        Returns the configured MIDI device name
        */
        public string GetDeviceMIDI()
        {
            return configMain.AppSettings.Settings["DEVICE_MIDI"].Value;
        }

        /*
        Saves the configured MIDI device name
        */
        public bool SaveDeviceMIDI(string sDeviceMIDI)
        {
            try
            {
                configMain.AppSettings.Settings["DEVICE_MIDI"].Value = sDeviceMIDI;
                configMain.Save(ConfigurationSaveMode.Minimal, false);
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }

            return true;
        }

        /*
        Returns the configured MIDI channel
        */
        public int GetChannelMIDI()
        {
            return int.Parse(configMain.AppSettings.Settings["CHANNEL_MIDI"].Value);
        }

        /*
        Saves the configured MIDI channel
        */
        public bool SaveChannelMIDI(int iChannelMIDI)
        {
            try
            {
                configMain.AppSettings.Settings["CHANNEL_MIDI"].Value = iChannelMIDI.ToString();
                configMain.Save(ConfigurationSaveMode.Minimal, false);
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }

            return true;
        }

        /*
        Returns the selected preset
        */
        public int GetSelectedPreset()
        {
            return int.Parse(configMain.AppSettings.Settings["SELECTED_PRESET"].Value);
        }

        /*
        Saves the selected preset
        */
        public bool SaveSelectedPreset(int iSelectedPreset)
        {
            try
            {
                configMain.AppSettings.Settings["SELECTED_PRESET"].Value = iSelectedPreset.ToString();
                configMain.Save(ConfigurationSaveMode.Minimal, false);
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }

            return true;
        }

        /*
        Returns the configuration for the given preset
        */
        public DeviceConfig GetPreset(int iPreset)
        {
            return new DeviceConfig(configMain.AppSettings.Settings["PRESET_" + iPreset].Value);
        }

        /*
        Saves the configuration to the given preset
        */
        public bool SavePreset(int iPreset, DeviceConfig config)
        {
            try
            {
                configMain.AppSettings.Settings["PRESET_" + iPreset].Value = config.GetBytes();
                configMain.Save(ConfigurationSaveMode.Minimal, false);
            }
            catch (ConfigurationErrorsException)
            {
                return false;
            }

            return true;
        }
    }
}
