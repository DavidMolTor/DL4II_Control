/*
MIDI CONTROL 2023

Functions.cs

- Description: Functions class
- Author: David Molina Toro
- Date: 02 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Windows;
using System.Windows.Media;

//MIDI libraries
using Sanford.Multimedia.Midi;

namespace MidiControl
{
    internal class Functions
    {
        //Midi output device object
        private static OutputDevice device = null;

        /*
        Initializes the output device
        */
        public static void SetDevice()
        {
            for (int i = 0; i < OutputDevice.DeviceCount; i++)
            {
                //Check the MIDI device name
                if (OutputDevice.GetDeviceCapabilities(i).name == Constants.DL4_PRODUCT_NAME)
                {
                    device = new OutputDevice(i);
                }
            }
        }

        /*
        Sends the given command
        */
        public static bool SendCommand(ChannelCommand iType, int iChannel, int iCommand, int iValue = 0)
        {
            try
            {
                Console.WriteLine("Send to channel {0}: {1}, {2}, {3}", iChannel, iType, iCommand, iValue);

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
                Console.WriteLine("Error: Could not send the channel command");
                return false;
            }
        }

        /*
        Returns the children of the given object with the given name
        */
        public static T FindChild<T>(DependencyObject parent, string sChildName) where T : DependencyObject
        {
            //Create a new found child object
            T foundChild = default;

            //Look for the child inside the given object
            int iChildrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < iChildrenCount; i++)
            {
                //Get the current child
                var child = VisualTreeHelper.GetChild(parent, i);

                //Check for the child type
                if (!(child is T))
                {
                    //Repeat the process until a child is found
                    foundChild = FindChild<T>(child, sChildName);

                    //Break if the child is found
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(sChildName))
                {
                    //Search for the child
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == sChildName)
                    {
                        //The child has been found by name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    //The child has been found the first time
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
