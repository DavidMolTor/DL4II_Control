/*
MIDI CONTROL 2023

Structs.cs

- Description: Structures class
- Author: David Molina Toro
- Date: 02 - 02 - 2023
- Version: 1.7
*/

using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace MidiControl
{
    /*
    Device configuration structure
    */
    [Serializable]
    public struct DeviceConfig
    {
        //Attributes
        public int iDelaySelected;
        public int iDelayTime;
        public int iDelayNotes;
        public int iDelayRepeats;
        public int iDelayTweak;
        public int iDelayTweez;
        public int iDelayMix;
        public int iReverbSelected;
        public int iReverbDecay;
        public int iReverbTweak;
        public int iReverbRouting;
        public int iReverbMix;

        /*
        Public constructor
        */
        public DeviceConfig(DeviceConfig config)
        {
            iDelaySelected  = config.iDelaySelected;
            iDelayTime      = config.iDelayTime;
            iDelayNotes     = config.iDelayNotes;
            iDelayRepeats   = config.iDelayRepeats;
            iDelayTweak     = config.iDelayTweak;
            iDelayTweez     = config.iDelayTweez;
            iDelayMix       = config.iDelayMix;
            iReverbSelected = config.iReverbSelected;
            iReverbDecay    = config.iReverbDecay;
            iReverbTweak    = config.iReverbTweak;
            iReverbRouting  = config.iReverbRouting;
            iReverbMix      = config.iReverbMix;
        }

        //Structure serialization declarations
        public DeviceConfig(string sData) : this() => this = Serializer.GetStruct<DeviceConfig>(sData);
        public string GetBytes() => Serializer.GetBytes(this);
    };

    /*
    Structure serializer class
    */
    public class Serializer
    {
        /*
        Returns the given string as an object
        */
        public static T GetStruct<T>(string sData) where T : new()
        {
            //Turn the string into a byte array
            byte[] data = Enumerable.Range(0, sData.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(sData.Substring(x, 2), 16)).ToArray();

            //Create a new object and get the size
            T retVal    = new T();
            int iSize   = Marshal.SizeOf(retVal);

            //Allocate the pointer and copy the data
            IntPtr pointer = Marshal.AllocHGlobal(iSize);
            Marshal.Copy(data, 0, pointer, iSize);

            //Cast the pointer into the object
            retVal = (T)Marshal.PtrToStructure(pointer, retVal.GetType());
            Marshal.FreeHGlobal(pointer);

            return retVal;
        }

        /*
        Returns the given object as an string
        */
        public static string GetBytes<T>(T structData) where T : new()
        {
            //Get the size and create a byte array
            int iSize   = Marshal.SizeOf(structData);
            byte[] data = new byte[iSize];

            //Allocate the pointer
            IntPtr pointer = Marshal.AllocHGlobal(iSize);
            Marshal.StructureToPtr(structData, pointer, true);

            //Copy the data to the pointer
            Marshal.Copy(pointer, data, 0, iSize);
            Marshal.FreeHGlobal(pointer);

            return BitConverter.ToString(data).Replace("-", "");
        }
    }
}