/*
MIDI CONTROL 2023

Constants.cs

- Description: Constants declaration script
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 0.1
*/

namespace MidiControl
{
    internal class Constants
    {
        //Connection checking period in milliseconds
        public const int CONNECTION_PERIOD = 2000;

        //No device found connected value
        public const int NO_DEVICE = -1;

        //DL4 MIDI device name
        public const string DL4_PRODUCT_NAME = "DL4 MkII MIDI";

        //DL4 knob rotation steps and limits
        public const int SELECT_KNOB_STEPS = 16;
        public const int KNOB_MIN_ROTATION = -150;
        public const int KNOB_MAX_ROTATION = 150;
        public const int PIXELS_PER_DEGREE = 2;
    }
}
