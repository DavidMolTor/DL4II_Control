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
