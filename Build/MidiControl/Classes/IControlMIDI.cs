/*
MIDI CONTROL 2023

IControlMIDI.cs

- Description: MIDI control instance
- Author: David Molina Toro
- Date: 05 - 02 - 2023
- Version: 0.1
*/

using System;

//MIDI libraries
using Sanford.Multimedia.Midi;

namespace MidiControl
{
    public sealed class IControlMIDI
    {
        private static volatile IControlMIDI instance = null;
        private static readonly object padlock = new object();

        /*
        Instance object for the singleton instantiation
        */
        public static IControlMIDI Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new IControlMIDI();
                        }
                    }
                }

                return instance;
            }
        }

        //Midi output device object
        private OutputDevice device = null;

        /*
        Initializes the output device
        */
        public bool ConnectDevice()
        {
            bool bResult = false;
            for (int i = 0; i < OutputDevice.DeviceCount; i++)
            {
                //Check the MIDI device name
                if (OutputDevice.GetDeviceCapabilities(i).name == Constants.DL4_PRODUCT_NAME)
                {
                    //Check if the device is already connected
                    if (device == null)
                    {
                        device = new OutputDevice(i);
                    }

                    bResult = true;
                }
                else if (i == OutputDevice.DeviceCount - 1)
                {
                    device = null;
                }
            }

            return bResult;
        }

        /*
        Sends the given command
        */
        public bool SendCommand(ChannelCommand iType, int iChannel, int iCommand, int iValue = 0)
        {
            try
            {
                //Build the channel command
                ChannelMessageBuilder builder = new ChannelMessageBuilder()
                {
                    MidiChannel = iChannel - 1,
                    Command     = iType,
                    Data1       = iCommand,
                    Data2       = iValue
                };
                builder.Build();

                //Send the channel command
                device.Send(builder.Result);

                return true;
            }
            catch
            {
                Console.WriteLine("Error: Could not send the command to channel {0}: {1}, {2}", iChannel, iCommand, iValue);
                return false;
            }
        }
    }
}
