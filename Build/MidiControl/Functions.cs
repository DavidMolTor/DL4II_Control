/*
MIDI CONTROL 2023

Functions.cs

- Description: Functions class
- Author: David Molina Toro
- Date: 02 - 02 - 2023
- Version: 0.1
*/

using System.Linq;
using System.Windows;
using System.Windows.Media;

//MIDI libraries
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Multimedia;

namespace MidiControl
{
    internal class Functions
    {
        /*
        Sends the given command
        */
        public bool SendCommand(CommandType iType, int iChannel, int iCommand, int iValue)
        {
            //Check if the device is connected
            if (OutputDevice.GetAll().Any(x => x.Name == Constants.DL4_PRODUCT_NAME))
            {
                using (OutputDevice device = OutputDevice.GetByName(Constants.DL4_PRODUCT_NAME))
                {
                    switch (iType)
                    {
                        case CommandType.ProgramChange:
                            ProgramChangeEvent programChange = new ProgramChangeEvent()
                            {
                                Channel         = (FourBitNumber)(iChannel - 1),
                                ProgramNumber   = (SevenBitNumber)iCommand
                            };

                            device.SendEvent(programChange);
                            break;
                        case CommandType.ControlChange:
                            ControlChangeEvent controlChange = new ControlChangeEvent()
                            {
                                Channel         = (FourBitNumber)(iChannel - 1),
                                ControlNumber   = (SevenBitNumber)iCommand,
                                ControlValue    = (SevenBitNumber)iValue
                            };

                            device.SendEvent(controlChange);
                            break;
                    }
                }

                return true;
            }
            else
            {
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
