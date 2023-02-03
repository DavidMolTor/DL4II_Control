/*
MIDI CONTROL 2023

Enums.cs

- Description: Enumeration class
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 0.1
*/

using System;

namespace MidiControl
{
    /*
    Command type enumeration
    */
    public enum CommandType
    {
        None            = 0,
        ProgramChange   = 1,
        ControlChange   = 2
    }

    /*
    Delay models enumeration
    */
    public enum DelayModels
    {
        VintageDelay    = 0,
        Crisscross      = 1,
        Euclidean       = 2,
        DualDelay       = 3,
        PitchEcho       = 4,
        ADT             = 5,
        Ducked          = 6,
        Harmony         = 7,
        Looper          = 30,
        Heliosphere     = 8,
        Transistor      = 9,
        Cosmos          = 10,
        MultiPass       = 11,
        Adriatic        = 12,
        ElephantMan     = 13,
        Glitch          = 14
    };

    /*
    Legacy models enumeration
    */
    public enum LegacyModels
    {
        Digital     = 15,
        DigitalMod  = 16,
        EchoPlatter = 17,
        Stereo      = 18,
        PingPong    = 19,
        Reverse     = 20,
        Dynamic     = 21,
        AutoVol     = 22,
        Looper      = 30,
        TubeEcho    = 23,
        TapeEcho    = 24,
        MultiHead   = 25,
        Sweep       = 26,
        Analog      = 27,
        AnalogMod   = 28,
        LoResDelay  = 29
    };

    /*
    Reverb models enumeration
    */
    public enum ReverbModels
    {
        Room            = 0,
        Searchlights    = 1,
        ParticleVerb    = 2,
        DoubleTank      = 3,
        Octo            = 4,
        Tile            = 5,
        Ducking         = 6,
        Plateaux        = 7,
        ReverbOff       = 15,
        Cave            = 8,
        Plate           = 9,
        Ganymede        = 10,
        Chamber         = 11,
        HotSprings      = 12,
        Hall            = 13,
        Glitz           = 14
    };

    /*
    Footswitch status enumeration
    */
    public enum FootswitchStatus
    {
        Off     = 0,
        Green   = 1,
        Red     = 2,
    };

    /*
    Alternative button status enumeration
    */
    public enum AltButtonStatus
    {
        White = 0,
        Green = 1
    };
}
