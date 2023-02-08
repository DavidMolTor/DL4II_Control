/*
MIDI CONTROL 2023

IControlMIDI.cs

- Description: MIDI control instance
- Author: David Molina Toro
- Date: 05 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Timers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        /*
        Initializes the MIDI control system
        */
        public void Initialize()
        {
            //Try conencting for the first time
            ConnectDevice(this, null);

            //Initialize the device connection timer
            Timer timerConnection   = new Timer(Constants.DEVICE_CONNECTION_PERIOD);
            timerConnection.Elapsed += ConnectDevice;
            timerConnection.Enabled = true;

            //Start the command sending task
            Task.Run(SendCommands);

            //Start the error management task
            Task.Run(ErrorManagement);
        }

        /*
        Deinitializes the MIDI control system
        */
        public void Deinitialize()
        {
            DisconnectDevice();
        }

        //Midi output device object
        OutputDevice device = null;

        /*
        Initializes the output device
        */
        public void ConnectDevice(object sender, ElapsedEventArgs e)
        {
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
                }
                else if (i == OutputDevice.DeviceCount - 1)
                {
                    DisconnectDevice();
                }
            }
        }

        /*
        Deinitializes the output device
        */
        public void DisconnectDevice()
        {
            if (device != null)
            {
                device.Dispose();
                device = null;
            }
        }

        //MIDI commands queue object
        List<ChannelMessage> listCommands = new List<ChannelMessage>();

        /*
        Adds the given command to the queue
        */
        public void AddCommand(ChannelCommand iType, int iChannel, int iCommand, int iValue = 0)
        {
            //Create a message from the given parameters
            ChannelMessageBuilder builder = new ChannelMessageBuilder()
            {
                MidiChannel = iChannel - 1,
                Command     = iType,
                Data1       = (int)iCommand,
                Data2       = iValue
            };
            builder.Build();

            //Add the command to the queue
            listCommands.Add(builder.Result);
        }

        /*
        Sends the pending commands
        */
        private void SendCommands()
        {
            while (true)
            {
                //Check if there is any command to send
                if (listCommands.Count > 0)
                {
                    try
                    {
                        device.Send(listCommands[0]);
                        listCommands.RemoveAt(0);
                    }
                    catch (NullReferenceException)
                    {
                        lock (listErrors)
                        {
                            listErrors.Add(listCommands[0]);
                            listCommands.RemoveAt(0);
                        }
                    }
                }

                //Wait for some time until the next loop
                System.Threading.Thread.Sleep(Constants.DEVICE_MIDI_PERIOD);
            }
        }

        //Pending command errors objects
        public event StringMessage ErrorMessage;
        public delegate void StringMessage(string sMessage);
        List<ChannelMessage> listErrors = new List<ChannelMessage>();

        /*
        Error management task
        */
        private void ErrorManagement()
        {
            while (true)
            {
                lock (listErrors)
                {
                    //Check if there are any pending errors
                    if (listErrors.Count > 0)
                    {
                        //Check the type of command
                        switch (listErrors[0].Command)
                        {
                            case ChannelCommand.ProgramChange:
                                ErrorMessage?.Invoke("PRESET SELECT COMMAND");
                                break;
                            case ChannelCommand.Controller:
                                ErrorMessage?.Invoke(Regex.Replace(((SettingsCC)listErrors[0].Data1).ToString(), "([a-z])([A-Z])", "$1 $2").ToUpper() + " COMMAND");
                                break;
                            default:
                                ErrorMessage?.Invoke("UNKNOWN ERROR");
                                break;
                        }

                        //Remove all similar errors
                        listErrors.RemoveAll(x => x.Command == listErrors[0].Command && x.Data1 == listErrors[0].Data1);
                    }
                    else
                    {
                        ErrorMessage?.Invoke("NONE");
                    }
                }

                //Wait for some time until the next loop
                System.Threading.Thread.Sleep(Constants.DEVICE_ERROR_PERIOD);
            }
        }
    }
}
