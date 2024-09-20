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
            //Try connecting for the first time
            ConnectDevice();

            //Initialize the device connection timer
            Timer timerConnectionDevice     = new Timer(Constants.DEVICE_CONNECTION_PERIOD);
            timerConnectionDevice.Elapsed   += TimerConnection_Elapsed;
            timerConnectionDevice.Enabled   = true;

            //Initialize the send commands timer
            Timer timerSendCommands         = new Timer(Constants.DEVICE_MIDI_PERIOD);
            timerSendCommands.Elapsed       += TimerSendCommands_Elapsed;
            timerSendCommands.Enabled       = true;

            //Initialize the send commands timer
            Timer timerErrorManagement      = new Timer(Constants.DEVICE_ERROR_PERIOD);
            timerErrorManagement.Elapsed    += TimerErrorManagement_Elapsed;
            timerErrorManagement.Enabled    = true;
        }

        /*
        Deinitializes the MIDI control system
        */
        public void Deinitialize()
        {
            DisconnectDevice();
        }

        //Midi output device objects
        object lockDevice   = new object();
        OutputDevice device = null;

        /*
        Initializes the output device
        */
        public void ConnectDevice()
        {
            lock (lockDevice)
            {
                for (int i = 0; i < OutputDevice.DeviceCount; i++)
                {
                    //Check the MIDI device name
                    if (OutputDevice.GetDeviceCapabilities(i).name == IControlConfig.Instance.GetDeviceMIDI())
                    {
                        //Check if the device is already connected
                        if (device == null)
                        {
                            device = new OutputDevice(i);
                            device.Reset();

                            ErrorMessage?.Invoke("NONE");
                        }

                        break;
                    }
                    else if (i == OutputDevice.DeviceCount - 1)
                    {
                        DisconnectDevice();

                        ErrorMessage?.Invoke("NO DEVICE CONNECTED");
                    }
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

        /*
        Device connection timer elapsed function
        */
        private void TimerConnection_Elapsed(object source, ElapsedEventArgs e)
        {
            ConnectDevice();
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
                Data1       = iCommand,
                Data2       = iValue
            };
            builder.Build();

            //Add the command to the queue
            listCommands.Add(builder.Result);
        }

        /*
        Removes all commands from the queue
        */
        public void RemoveCommands()
        {
            lock (lockDevice)
            {
                listCommands.Clear();
            }
        }

        /*
        Send commands timer elapsed function
        */
        private void TimerSendCommands_Elapsed(object source, ElapsedEventArgs e)
        {
            //Check if there is any command to send
            if (listCommands.Count > 0)
            {
                try
                {
                    lock (lockDevice)
                    {
                        if (device != null)
                        {
                            device.Send(listCommands[0]);
                        }

                        listCommands.RemoveAt(0);
                    }
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
        }

        //Pending command errors objects
        public event StringMessage ErrorMessage;
        public delegate void StringMessage(string sMessage);
        List<ChannelMessage> listErrors = new List<ChannelMessage>();

        /*
        Error management timer elapsed function
        */
        private void TimerErrorManagement_Elapsed(object source, ElapsedEventArgs e)
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
        }
    }
}
