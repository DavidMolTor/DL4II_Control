/*
MIDI CONTROL 2023

Enums.cs

- Description: Enumeration class
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 1.7
*/

namespace MidiControl
{
    /*
    Command type enumeration
    */
    public enum SettingsCC
    {
        DelaySelected   = 1,
        ReverbSelected  = 2,
        ExpressionPedal = 3,
        PresetBypass    = 4,
        LooperMode      = 9,
        DelayTime       = 11,
        DelayNotes      = 12,
        DelayRepeats    = 13,
        DelayTweak      = 14,
        DelayTweez      = 15,
        DelayMix        = 16,
        ReverbDecay     = 17,
        ReverbTweak     = 18,
        ReverbRouting   = 19,
        ReverbMix       = 20,
        TapTempo        = 64
    }

    /*
    Delay models enumeration
    */
    public enum DelayModels
    {
        VintageDigital  = 0,
        Crisscross      = 1,
        Euclidean       = 2,
        DualDelay       = 3,
        PitchEcho       = 4,
        ADT             = 5,
        Ducked          = 6,
        Harmony         = 7,
        Heliosphere     = 8,
        Transistor      = 9,
        Cosmos          = 10,
        MultiPass       = 11,
        Adriatic        = 12,
        ElephantMan     = 13,
        Glitch          = 14,
        Looper          = 30
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
        TubeEcho    = 23,
        TapeEcho    = 24,
        MultiHead   = 25,
        Sweep       = 26,
        Analog      = 27,
        AnalogMod   = 28,
        LoResDelay  = 29,
        Looper      = 30
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
        Cave            = 8,
        Plate           = 9,
        Ganymede        = 10,
        Chamber         = 11,
        HotSprings      = 12,
        Hall            = 13,
        Glitz           = 14,
        ReverbOff       = 15
    };

    /*
    Reverb routing enumeration
    */
    public enum ReverRouting
    {
        ReverbDelay = 0,
        Parallel    = 1,
        DelayReverb = 2
    };

    /*
    Footswitch status enumeration
    */
    public enum FootswitchStatus
    {
        Off     = 0,
        Green   = 1,
        Dim     = 2,
        Red     = 3
    };

    /*
    Alternative button status enumeration
    */
    public enum AltButtonStatus
    {
        White = 0,
        Green = 1
    };

    /*
    Time subdivisions enumeration
    */
    public enum TimeSubdivisions
    {
        EighthTriplet   = 0,
        EighthFull      = 1,
        EighthDotted    = 2,
        QuarterTriplet  = 3,
        QuarterFull     = 4,
        QuarterDotted   = 5,
        HalfTripplet    = 6,
        HalfFull        = 7,
        HalfDotted      = 8
    };
}
