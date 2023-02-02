/*
MIDI CONTROL 2023

Constants.cs

- Description: Constants declaration script
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Collections.Generic;

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
        public const int SELECT_KNOB_STEPS  = 16;
        public const int KNOB_MIN_ROTATION  = -150;
        public const int KNOB_MAX_ROTATION  = 150;
        public const int PIXELS_PER_DEGREE  = 2;
        public const int KNOB_MAX_VALUE     = 127;

        //Number of presets available
        public const int PRESET_COUNT_MIN = 1;
        public const int PRESET_COUNT_MAX = 128;

        //Selection knob parameters
        public const int ALTDELAY_INITIAL   = 15;
        public const int LOOPER_POSITION    = 8;
        public const int LOOPER_VALUE       = 15;

        //Available delay dictionary
        public readonly Dictionary<string, Tuple<string, string>> dictDelays = new Dictionary<string, Tuple<string, string>>()
        {
            { "VINTAGE DELAY",  new Tuple<string, string>("BITS", "DEPTH") },
            { "CRISSCROSS",     new Tuple<string, string>("TIME B", " CROSS") },
            { "EUCLIDEAN",      new Tuple<string, string>("FILL", "ROTATE") },
            { "DUAL DELAY",     new Tuple<string, string>("TIME B", "FEED B") },
            { "PITCH ECHO",     new Tuple<string, string>("PITCH", "CENTS") },
            { "ADT",            new Tuple<string, string>("DRIVE", "DEPTH") },
            { "DUCKED",         new Tuple<string, string>("LIMIT", "AMOUNT") },
            { "HARMONY",        new Tuple<string, string>("KEY", "MODES") },
            { "LOOPER",         new Tuple<string, string>("ECHO", "VOLUME") },
            { "HELIOSPHERE",    new Tuple<string, string>("REVERB", "DEPTH") },
            { "TRANSISTOR",     new Tuple<string, string>("HEADROOM", "WOW") },
            { "COSMOS",         new Tuple<string, string>("HEADS", "FEEDBACK") },
            { "MULTI PASS",     new Tuple<string, string>("PATTERN", "MODE") },
            { "ADRIATIC",       new Tuple<string, string>("RATE", "DEPTH") },
            { "ELEPHANT MAN",   new Tuple<string, string>("DEPTH", "MODE") },
            { "GLITCH",         new Tuple<string, string>("PITCH", "DEPTH") }
        };

        //Available legacy delay dictionary
        public readonly Dictionary<string, Tuple<string, string>> dictLegacy = new Dictionary<string, Tuple<string, string>>()
        {
            { "DIGITAL",        new Tuple<string, string>("BASS", "TREBLE") },
            { "DIGITAL MOD",    new Tuple<string, string>("RATE", "DEPTH") },
            { "ECHO PLATTER",   new Tuple<string, string>("WOW", "DRIVE") },
            { "STEREO",         new Tuple<string, string>("TIME B", "REPEATS B") },
            { "PING PONG",      new Tuple<string, string>("OFFSET", "SPREAD") },
            { "REVERSE",        new Tuple<string, string>("RATE", "DEPTH") },
            { "DYNAMIC",        new Tuple<string, string>("LIMIT", "DUCKING") },
            { "AUTO-VOL",       new Tuple<string, string>("DEPTH", "SWELL") },
            { "LOOPER",         new Tuple<string, string>("ECHO", "VOLUME") },
            { "TUBE ECHO",      new Tuple<string, string>("WOW", "DRIVE") },
            { "TAPE ECHO",      new Tuple<string, string>("BASS", "TREBLE") },
            { "MULTI-HEAD",     new Tuple<string, string>("HEADS 1/2", "HEADS 3/4") },
            { "SWEEP",          new Tuple<string, string>("RATE", "DEPTH") },
            { "ANALOG",         new Tuple<string, string>("BASS", "TREBLE") },
            { "ANALOG MOD",     new Tuple<string, string>("RATE", "DEPTH") },
            { "LO RES DELAY",   new Tuple<string, string>("TONE", "RESOLUTION") }
        };

        //Available reverb dictionary
        public readonly Dictionary<string, string> dictReverbs = new Dictionary<string, string>()
        {
            { "ROOM",           "PREDELAY" },
            { "SEARCHLIGHTS",   "MOD MIX" },
            { "PARTICLE VERB",  "CONDITION" },
            { "DOUBLE TANK",    "MOD DEPTH" },
            { "OCTO",           "INTENSITY" },
            { "TILE",           "PREDELAY" },
            { "DUCKING",        "PREDELAY" },
            { "PLATEAUX",       "PITCH MODES" },
            { "REVERB OFF",     "N/A" },
            { "CAVE",           "PREDELAY" },
            { "PLATE",          "PREDELAY" },
            { "GANYMEDE",       "MOD DEPTH" },
            { "CHAMBER",        "PREDELAY" },
            { "HOT SPRINGS",    "SPRING COUNT" },
            { "HALL",           "PREDELAY" },
            { "GLITZ",          "MOD DEPTH" }
        };
    }
}
