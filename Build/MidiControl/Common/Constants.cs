/*
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
        //Connection checking period in milliseconds
        public const int CONNECTION_PERIOD = 2000;

        //No device found connected value
        public const int NO_DEVICE = -1;

        //DL4 MIDI device name
        public const string DL4_PRODUCT_NAME = "DL4 MkII MIDI";

        //DL4 knob rotation steps and limits
        public const double KNOB_ROTATION_MIN   = -150;
        public const double KNOB_ROTATION_MAX   = 150;
        public const double KNOB_ROTATION_RATE  = 10;

        //Number of presets available
        public const int PRESET_COUNT_MIN   = 1;
        public const int PRESET_COUNT_MAX   = 128;

        //Selection knob parameters
        public const int ALTDELAY_INITIAL   = 15;
        public const int MAX_KNOB_VALUES    = 128;

        /*
        Delay models list
        */
        public static readonly List<DelayModels> LIST_DELAY = new List<DelayModels>()
        {
            DelayModels.VintageDelay,
            DelayModels.Crisscross,
            DelayModels.Euclidean,
            DelayModels.DualDelay,
            DelayModels.PitchEcho,
            DelayModels.ADT,
            DelayModels.Ducked,
            DelayModels.Harmony,
            DelayModels.Looper,
            DelayModels.Heliosphere,
            DelayModels.Transistor,
            DelayModels.Cosmos,
            DelayModels.MultiPass,
            DelayModels.Adriatic,
            DelayModels.ElephantMan,
            DelayModels.Glitch
        };

        /*
        Legacy models list
        */
        public static readonly List<LegacyModels> LIST_LEGACY = new List<LegacyModels>()
        {
            LegacyModels.Digital,
            LegacyModels.DigitalMod,
            LegacyModels.EchoPlatter,
            LegacyModels.Stereo,
            LegacyModels.PingPong,
            LegacyModels.Reverse,
            LegacyModels.Dynamic,
            LegacyModels.AutoVol,
            LegacyModels.Looper,
            LegacyModels.TubeEcho,
            LegacyModels.TapeEcho,
            LegacyModels.MultiHead,
            LegacyModels.Sweep,
            LegacyModels.Analog,
            LegacyModels.AnalogMod,
            LegacyModels.LoResDelay
        };

        /*
        Reverb models list
        */
        public static readonly List<ReverbModels> LIST_REVERB = new List<ReverbModels>()
        {
            ReverbModels.Room,
            ReverbModels.Searchlights,
            ReverbModels.ParticleVerb,
            ReverbModels.DoubleTank,
            ReverbModels.Octo,
            ReverbModels.Tile,
            ReverbModels.Ducking,
            ReverbModels.Plateaux,
            ReverbModels.ReverbOff,
            ReverbModels.Cave,
            ReverbModels.Plate,
            ReverbModels.Ganymede,
            ReverbModels.Chamber,
            ReverbModels.HotSprings,
            ReverbModels.Hall,
            ReverbModels.Glitz
        };
    }
}
