/*
MIDI CONTROL 2023

RotaryKnob.cs

- Description: Rotary knob element class
- Author: David Molina Toro
- Date: 03 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace MidiControl
{
    public partial class RotaryKnob : UserControl
    {
        /*
        Public constructor
        */
        public RotaryKnob()
        {
            InitializeComponent();
        }

        //Knob status variable
        public int Status = 0;

        //Knob step enumeration
        List<int> Steps = new List<int>();

        //Rotation limit variable
        public bool Limited = true;

        /*
        Sets the knob status
        */
        public void SetKnob(int iStatus, List<int> listSteps, bool bLimited = true)
        {
            //Set the knob status
            Status = iStatus;

            //Set the knob step enumeration
            Steps = listSteps;

            //Set if the knob is limited
            Limited = bLimited;

            //Check if the knob is limited
            if (Limited)
            {
                transformKnob.Angle = Constants.KNOB_ROTATION_MIN + (Constants.KNOB_ROTATION_MAX - Constants.KNOB_ROTATION_MIN) * (double)Steps.IndexOf(Status) / (Steps.Count - 1);
            }
            else
            {
                transformKnob.Angle = 360 * (double)Steps.IndexOf(Status) / Steps.Count; 
            }
        }

        //Variables for rotary knob actions
        private Point pointMouse    = new Point();
        private double dAngleActual = 0;

        /*
        Mouse press function for knob element
        */
        private void Knob_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Reset the previous mouse position
            pointMouse = e.GetPosition(this);

            //Set the initial angle
            dAngleActual = transformKnob.Angle;

            //Make the knob capture the mouse
            gridKnob.CaptureMouse();
        }

        /*
        Mouse move function for knob element
        */
        private void Knob_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (gridKnob.IsMouseCaptureWithin)
            {
                //Calculate the amount of degrees to rotate
                dAngleActual += (e.GetPosition(this).Y - pointMouse.Y);

                //Reset the previous mouse position
                pointMouse = e.GetPosition(this);

                //Set the current knob rotation
                SetRotation(ref dAngleActual);
            }
        }

        /*
        Mouse release function for knob element
        */
        private void Knob_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            gridKnob.ReleaseMouseCapture();
        }

        /*
        Mouse wheel function for knob element
        */
        private void Knob_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            //Add gap to the currenat angle
            dAngleActual = transformKnob.Angle + Math.Sign(e.Delta) * Constants.KNOB_ROTATION_RATE;

            //Set the current knob rotation
            SetRotation(ref dAngleActual);
        }

        /*
        Sets the selected knob rotation
        */
        private void SetRotation(ref double dAngle)
        {
            //Check if the knob is limited
            if (Limited)
            {
                //Check if the knob has reached any limit
                dAngle = dAngle > Constants.KNOB_ROTATION_MAX ? Constants.KNOB_ROTATION_MAX : dAngle;
                dAngle = dAngle < Constants.KNOB_ROTATION_MIN ? Constants.KNOB_ROTATION_MIN : dAngle;

                //Calculate the knob status and set its angle
                Status = Steps[(int)Math.Floor((dAngle - Constants.KNOB_ROTATION_MIN) / (Constants.KNOB_ROTATION_MAX - Constants.KNOB_ROTATION_MIN) * (Steps.Count - 1))];
                transformKnob.Angle = Constants.KNOB_ROTATION_MIN + (Constants.KNOB_ROTATION_MAX - Constants.KNOB_ROTATION_MIN) * (double)Steps.IndexOf(Status) / (Steps.Count - 1);
            }
            else
            {
                //Set the angle as absolute
                dAngle = dAngle >= 360 ? dAngle - 360 : dAngle;
                dAngle = dAngle < 0 ? dAngle + 360 : dAngle;

                //Calculate the knob status and set its angle
                Status = Steps[(int)Math.Floor(dAngle / 360 * Steps.Count)];
                transformKnob.Angle = 360 * (double)Steps.IndexOf(Status) / Steps.Count;
            }
        }
    }
}
