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
using System.Windows.Input;
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

        //Steps list for the knob
        public List<int> listSteps = new List<int>();

        /*
        Sets the selected knob status
        */
        public void SetKnob(int iValue)
        {
            /*
            //Check the selected knob
            double dAngle = 0;
            switch (imageKnob.Name.Split('_').Last())
            {
                case "DelaySelect":
                    int iDelaySteps = iValue < Constants.ALTDELAY_INITIAL ? iValue : iValue - Constants.ALTDELAY_INITIAL;
                    iDelaySteps     = iDelaySteps < Constants.LOOPER_POSITION ? iDelaySteps : iDelaySteps + 1;

                    //Set the delay selector rotation
                    RotateTransform rotateTransform_DelaySelect = new RotateTransform(iDelaySteps * 360 / Constants.SELECT_KNOB_STEPS);
                    imageKnob.RenderTransform = rotateTransform_DelaySelect;
                    break;
                case "ReverbSelect":
                    int iReverbSteps    = iValue < Constants.LOOPER_POSITION ? iValue : iValue + 1;
                    iReverbSteps        = iValue != Constants.LOOPER_VALUE ? iReverbSteps : Constants.LOOPER_POSITION;

                    //Set the reverb selector rotation
                    RotateTransform rotateTransform_ReverbSelect = new RotateTransform(iReverbSteps * 360 / Constants.SELECT_KNOB_STEPS);
                    imageKnob.RenderTransform = rotateTransform_ReverbSelect;
                    break;
                case "ReverbRouting":
                    switch (iValue)
                    {
                        case 0:
                            dAngle = Constants.KNOB_MIN_ROTATION;
                            break;
                        case 1:
                            dAngle = 0;
                            break;
                        case 2:
                            dAngle = Constants.KNOB_MAX_ROTATION;
                            break;
                    }

                    //Set the reverb routing knob rotation
                    RotateTransform rotateTransform_ReverbRouting = new RotateTransform(dAngle);
                    imageKnob.RenderTransform = rotateTransform_ReverbRouting;
                    break;
                default:
                    dAngle = Constants.KNOB_MIN_ROTATION + (Constants.KNOB_MAX_ROTATION - Constants.KNOB_MIN_ROTATION) * (double)iValue / Constants.KNOB_MAX_VALUE;
                    RotateTransform rotateTransform = new RotateTransform(dAngle);
                    imageKnob.RenderTransform = rotateTransform;
                    break;
            }
            */
        }

        //Variables for rotary knob actions
        private Point pointMouse = new Point();

        /*
        Mouse press function for knob element
        */
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Reset the previous mouse position
            pointMouse = e.GetPosition(this);

            //Make the knob capture the mouse
            gridKnob.CaptureMouse();
        }

        /*
        Mouse move function for knob element
        */
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (gridKnob.IsMouseCaptureWithin)
            {
                //Calculate the amount of degrees to rotate
                double dAngle = transformKnob.Angle + (e.GetPosition(this).Y - pointMouse.Y);

                //Reset the previous mouse position
                pointMouse = e.GetPosition(this);

                //Set the knob rotation
                SetRotation(dAngle);
            }
        }

        /*
        Mouse release function for knob element
        */
        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            gridKnob.ReleaseMouseCapture();
        }

        /*
        Mouse wheel function for knob element
        */
        private void Knob_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //Calculate the amount of degrees to rotate
            double dAngle = transformKnob.Angle + Math.Sign(e.Delta) * Constants.KNOB_ROTATION_RATE;

            //Set the knob rotation
            SetRotation(dAngle);
        }

        /*
        Sets the selected knob rotation
        */
        private void SetRotation(double dAngle)
        {
            //Check the selected knob steps
            if (listSteps.Count == 0)
            {
                dAngle = dAngle > Constants.KNOB_ROTATION_MAX ? Constants.KNOB_ROTATION_MAX : dAngle;
                dAngle = dAngle < Constants.KNOB_ROTATION_MIN ? Constants.KNOB_ROTATION_MIN : dAngle;  
            }
            else
            {
                dAngle = Math.Floor(dAngle / (360 / listSteps.Count)) * 360 / listSteps.Count;
            }

            //Apply the rotation transform
            transformKnob.Angle = dAngle;
        }
    }
}
