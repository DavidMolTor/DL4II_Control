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
using System.Windows.Input;
using System.Windows.Controls;

//MIDI library
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Multimedia;

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
        }

        /*
        Sends the given command
        */
        public void SendCommand(CommandType iType, int iCommand, int iValue)
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
                                Channel         = (FourBitNumber)(int.Parse(textboxChannel.Text) - 1),
                                ProgramNumber   = (SevenBitNumber)iCommand
                            };

                            device.SendEvent(programChange);
                            break;
                        case CommandType.ControlChange:
                            ControlChangeEvent controlChange = new ControlChangeEvent()
                            {
                                Channel         = (FourBitNumber)(int.Parse(textboxChannel.Text) - 1),
                                ControlNumber   = (SevenBitNumber)iCommand,
                                ControlValue    = (SevenBitNumber)iValue
                            };

                            device.SendEvent(controlChange);
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Error: Device not connected");
            }
        }

        /*
        Check if the input character is a number
        */
        private void TextBoxNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text.ToCharArray()[0]);
        }
    }
}
