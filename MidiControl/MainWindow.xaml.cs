/*
MIDI CONTROL 2023

MainWindow.cs

- Description: Main window for DL4 control
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Collections.Generic;

//MIDI library
using NAudio.Midi;

namespace MidiControl
{
    public partial class MainWindow : Window
    {
        /*
        Public constructor
        */
        public MainWindow()
        {
            InitializeComponent();

            //Initialize the connection timer
            timerConnect = new Timer()
            {
                Enabled     = true,
                Interval    = Constants.CONNECTION_PERIOD
            };
            timerConnect.Elapsed += TimerConnect_Elapsed;
        }

        //MIDI connection timer
        Timer timerConnect;
        bool bConnected = false;

        /*
        Connection timer elpased function
        */
        private void TimerConnect_Elapsed(object? source, ElapsedEventArgs e)
        {
            //Get all connected MIDI devices
            Dictionary<string, int> dictDevices = new Dictionary<string, int>();
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
            {
                dictDevices.Add(MidiIn.DeviceInfo(i).ProductName, i);
            }

            //Set the connection status
            if (!bConnected && dictDevices.Keys.Contains(Constants.DL4_PRODUCT_NAME))
            {
                bConnected = true;
            }
            else if (bConnected && !dictDevices.Keys.Contains(Constants.DL4_PRODUCT_NAME))
            {
                bConnected = false;
            }

        }
    }
}
