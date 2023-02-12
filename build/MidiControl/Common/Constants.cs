﻿/*
MIDI CONTROL 2023

Constants.cs

- Description: Constants declaration script
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 0.1
*/

using System.Collections.Generic;

namespace MidiControl
{
    internal class Constants
    {
        //Device configuration parameters
        public const int DEVICE_CONNECTION_PERIOD   = 2000;
        public const int DEVICE_UPDATE_PERIOD       = 100;
        public const int DEVICE_ERROR_PERIOD        = 2000;
        public const int DEVICE_MIDI_PERIOD         = 100;

        //DL4 knob rotation steps and limits
        public const double KNOB_ROTATION_MIN       = -150;
        public const double KNOB_ROTATION_MAX       = 150;
        public const double KNOB_ROTATION_RATE      = 10;

        //Number of presets available
        public const int PRESET_COUNT_MIN           = 1;
        public const int PRESET_COUNT_MAX           = 128;

        //Selection knob parameters
        public const int ALTDELAY_INITIAL           = 15;
        public const int MAX_KNOB_VALUES            = 128;

        //Footswitch parameters
        public const int FOOTSWITCH_HOLD_TIME       = 2000;
        public const int FOOTSWITCH_BLINK_PERIOD    = 200;
        public const int FOOTSWITCH_BLINK_COUNT     = 4;

        /*
        Delay models dictionary
        */
        public static readonly Dictionary<DelayModels, string> DICT_DELAY = new Dictionary<DelayModels, string>()
        {
            { DelayModels.VintageDigital,   "RESOLUTION| MOD DEPTH" },
            { DelayModels.Crisscross,       "DELAY TIME B| CROSS AMOUNT" },
            { DelayModels.Euclidean,        "STEP FILL|ROTATE" },
            { DelayModels.DualDelay,        "RIGHT DELAY TIME| RIGHT FEEDBACK" },
            { DelayModels.PitchEcho,        "PITCH INTERVAL|PITCH CENTS" },
            { DelayModels.ADT,              "DISTORTION|MOD DEPTH" },
            { DelayModels.Ducked,           "THRESHOLD|DUCKING AMOUNT" },
            { DelayModels.Harmony,          "KEY|PITCH MODES" },
            { DelayModels.Looper,           "ECHO MOD|ECHO VOLUME" },
            { DelayModels.Heliosphere,      "REVERB MIX|MOD DEPTH" },
            { DelayModels.Transistor,       "HEADROOM| WOW & FLUTTER" },
            { DelayModels.Cosmos,           "HEADS SELECT| WOW & FLUTTER" },
            { DelayModels.MultiPass,        "TAP PATTERN|DELAY MODE" },
            { DelayModels.Adriatic,         "MOD RATE|MOD DEPTH" },
            { DelayModels.ElephantMan,      "MOD DEPTH|MODE" },
            { DelayModels.Glitch,           "PITCH|GLITCH AMOUNT" }
        };

        /*
        Legacy models dictionary
        */
        public static readonly Dictionary<LegacyModels, string> DICT_LEGACY = new Dictionary<LegacyModels, string>()
        {
            { LegacyModels.Digital,         "BASS|TREBLE" },
            { LegacyModels.DigitalMod,      "MOD RATE|MOD DEPTH" },
            { LegacyModels.EchoPlatter,     "WOW & FLUTTER|DRIVE" },
            { LegacyModels.Stereo,          "RIGHT DELAY TIME|RIGHT REPEATS" },
            { LegacyModels.PingPong,        "TIME OFFSET|STEREO SPREAD" },
            { LegacyModels.Reverse,         "MOD RATE|MOD DEPTH" },
            { LegacyModels.Dynamic,         "THRESHOLD|DUCKING" },
            { LegacyModels.AutoVol,         "MOD DEPTH|SWELL TIME" },
            { LegacyModels.Looper,          "ECHO MOD|ECHO VOLUME" },
            { LegacyModels.TubeEcho,        "WOW & FLUTTER|DRIVE" },
            { LegacyModels.TapeEcho,        "BASS|TREBLE" },
            { LegacyModels.MultiHead,       "HEADS 1/2|HEADS 3/4" },
            { LegacyModels.Sweep,           "SWEEP RATE|SWEEP DEPTH" },
            { LegacyModels.Analog,          "BASS|TREBLE" },
            { LegacyModels.AnalogMod,       "MOD RATE|MOD DEPTH" },
            { LegacyModels.LoResDelay,      "TONE|RESOLUTION" }
        };

        /*
        Reverb models dictionary
        */
        public static readonly Dictionary<ReverbModels, string> DICT_REVERB = new Dictionary<ReverbModels, string>()
        {
            { ReverbModels.Room,            "PREDELAY" },
            { ReverbModels.Searchlights,    "INTENSITY" },
            { ReverbModels.ParticleVerb,    "CONDITION" },
            { ReverbModels.DoubleTank,      "MOD DEPTH" },
            { ReverbModels.Octo,            "INTENSITY" },
            { ReverbModels.Tile,            "PREDELAY" },
            { ReverbModels.Ducking,         "PREDELAY" },
            { ReverbModels.Plateaux,        "PITCH MODES" },
            { ReverbModels.ReverbOff,       "NONE" },
            { ReverbModels.Cave,            "PREDELAY" },
            { ReverbModels.Plate,           "PREDELAY" },
            { ReverbModels.Ganymede,        "MOD DEPTH" },
            { ReverbModels.Chamber,         "PREDELAY" },
            { ReverbModels.HotSprings,      "SPRING COUNT" },
            { ReverbModels.Hall,            "PREDELAY" },
            { ReverbModels.Glitz,           "MOD DEPTH" }
        };

        /*
        Note subdivisions models dictionary
        */
        public static readonly Dictionary<TimeSubdivisions, int> DICT_SUBDIVISIONS = new Dictionary<TimeSubdivisions, int>()
        {
            { TimeSubdivisions.EighthTriplet,   425 },
            { TimeSubdivisions.EighthFull,      500 },
            { TimeSubdivisions.EighthDotted,    750 },
            { TimeSubdivisions.QuarterTriplet,  875 },
            { TimeSubdivisions.QuarterFull,     1000 },
            { TimeSubdivisions.QuarterDotted,   1500 },
            { TimeSubdivisions.HalfTripplet,    1750 },
            { TimeSubdivisions.HalfFull,        2000 },
            { TimeSubdivisions.HalfDotted,      3000 }
        };
    }
}
