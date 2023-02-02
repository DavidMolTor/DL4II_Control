/*
MIDI CONTROL 2023

DeviceControl.cs

- Description: Device control panel for DL4
- Author: David Molina Toro
- Date: 01 - 02 - 2023
- Version: 0.1
*/

using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MidiControl
{
    public partial class DeviceControl : UserControl
    {
        /*
        Public constructor
        */
        public DeviceControl()
        {
            InitializeComponent();

            //Set the configurable elements
            textboxChannel.Text = IControlConfig.Instance.GetChannelMIDI().ToString();
            textboxPreset.Text  = IControlConfig.Instance.GetSelectedPreset().ToString();

            //Set the current preset configuration
            currentConfig = IControlConfig.Instance.GetPreset(IControlConfig.Instance.GetSelectedPreset());

            //Update the device controls
            UpdateDevice();
        }

        //Current configuration structure
        DeviceConfig currentConfig = new DeviceConfig();

        /*
        Public constructor
        */
        private void UpdateDevice()
        {
            //Set all the reverb knobs status
            SetKnob(imageKnob_DelaySelect,  currentConfig.iDelaySelected);
            SetKnob(imageKnob_DelayTime,    currentConfig.iDelayTime);
            SetKnob(imageKnob_DelayRepeats, currentConfig.iDelayRepeats);
            SetKnob(imageKnob_DelayTweak,   currentConfig.iDelayTweak);
            SetKnob(imageKnob_DelayTweez,   currentConfig.iDelayTweez);
            SetKnob(imageKnob_DelayMix,     currentConfig.iDelayMix);

            //Set all the reverb knobs status
            SetKnob(imageKnob_ReverbSelect,     currentConfig.iReverbSelected);
            SetKnob(imageKnob_ReverbDecay,      currentConfig.iReverbDecay);
            SetKnob(imageKnob_ReverbTweak,      currentConfig.iReverbTweak);
            SetKnob(imageKnob_ReverbRouting,    currentConfig.iReverbRouting);
            SetKnob(imageKnob_ReverbMix,        currentConfig.iReverbMix);

            //Reset all footswitches
            imageFoot_A.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot None.png"));
            imageFoot_B.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot None.png"));
            imageFoot_C.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot None.png"));

            //Set the active footswitch
            switch (IControlConfig.Instance.GetSelectedPreset() % 3)
            {
                case 0:
                    imageFoot_C.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Green.png"));
                    break;
                case 1:
                    imageFoot_A.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Green.png"));
                    break;
                case 2:
                    imageFoot_B.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Green.png"));
                    break;
            }

            //Set the alternative button status
            if (currentConfig.iDelaySelected < Constants.ALTDELAY_INITIAL)
            {
                imageAlternative.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Alt White.png"));
            }
            else
            {
                imageAlternative.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Alt Green.png"));
            }
        }

        /*
        Sets the selected knob status
        */
        private void SetKnob(Image imageKnob, int iValue)
        {
            //Check the selected knob
            double dAngle = 0;
            switch (imageKnob.Name.Split('_').Last())
            {
                case "DelaySelect":
                    int iDelaySteps = iValue < Constants.ALTDELAY_INITIAL ? iValue : iValue - Constants.ALTDELAY_INITIAL;
                    iDelaySteps     = iDelaySteps < Constants.LOOPER_POSITION ? iDelaySteps : iDelaySteps + 1;

                    //Set the delay selector rotation
                    RotateTransform rotateTransform_DelaySelect = new RotateTransform(iDelaySteps * 360 / Constants.SELECT_KNOB_STEPS);
                    imageKnob_DelaySelect.RenderTransform = rotateTransform_DelaySelect;
                    break;
                case "ReverbSelect":
                    int iReverbSteps    = iValue < Constants.LOOPER_POSITION ? iValue : iValue + 1;
                    iReverbSteps        = iValue != Constants.LOOPER_VALUE ? iReverbSteps : Constants.LOOPER_POSITION;

                    //Set the reverb selector rotation
                    RotateTransform rotateTransform_ReverbSelect = new RotateTransform(iReverbSteps * 360 / Constants.SELECT_KNOB_STEPS);
                    imageKnob_ReverbSelect.RenderTransform = rotateTransform_ReverbSelect;
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
                    imageKnob_ReverbRouting.RenderTransform = rotateTransform_ReverbRouting;
                    break;
                default:
                    dAngle = Constants.KNOB_MIN_ROTATION + (Constants.KNOB_MAX_ROTATION - Constants.KNOB_MIN_ROTATION) * (double)iValue / Constants.KNOB_MAX_VALUE;
                    RotateTransform rotateTransform = new RotateTransform(dAngle);
                    imageKnob.RenderTransform = rotateTransform;
                    break;
            }
        }

        //Variables for rotary knob actions
        private double dInitialY    = 0;
        private double dInitialRot  = 0;
        private bool bKnobPressed   = false;

        /*
        Mouse press function for knob element
        */
        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //Make the knob capture the mouse
            ((Image)sender).CaptureMouse();

            //Reset the previous mouse position
            dInitialY = e.GetPosition(this).Y;

            //Get the current knob rotation
            RotateTransform rotateTransform = ((Image)sender).RenderTransform as RotateTransform;
            if (rotateTransform != null)
            {
                dInitialRot = rotateTransform.Angle;
            }
            else
            {
                dInitialRot = 0;
            }

            //Set the knob as pressed
            bKnobPressed = true;
        }

        /*
        Mouse move function for knob element
        */
        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            if (bKnobPressed)
            {
                //Calculate the amount of degrees to rotate
                double dAngle = dInitialRot + (e.GetPosition(this).Y - dInitialY) / Constants.PIXELS_PER_DEGREE;

                //Check if the current knob is a select one
                if (((Image)sender).Name.Contains("Select"))
                {
                    //Transform the angle into steps
                    dAngle = Math.Floor(dAngle / (360 / Constants.SELECT_KNOB_STEPS)) * 360 / Constants.SELECT_KNOB_STEPS;
                }
                else
                {
                    //Check the rotation conditions for the knob
                    dAngle = dAngle > Constants.KNOB_MAX_ROTATION ? Constants.KNOB_MAX_ROTATION : dAngle;
                    dAngle = dAngle < Constants.KNOB_MIN_ROTATION ? Constants.KNOB_MIN_ROTATION : dAngle;
                }

                //Apply the rotation transform
                RotateTransform rotateTransform = new RotateTransform(dAngle);
                ((Image)sender).RenderTransform = rotateTransform;
            }
        }

        /*
        Mouse release function for knob element
        */
        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            bKnobPressed = false;
            ((Image)sender).ReleaseMouseCapture();
        }

        /*
        Alternative button click function
        */
        private void ButtonAlt_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Get the container as an image
            Image image = ((Button)sender).Content as Image;

            //Alternate the switch status
            if (image.Source.ToString().Split('/').Last() == "DL4 MkII Alt White.png")
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Alt Green.png"));
            }
            else if (image.Source.ToString().Split('/').Last() == "DL4 MkII Alt Green.png")
            {
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Alt White.png"));
            }
        }

        /*
        Foot button click function
        */
        private void ButtonFoot_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Get the container as an image
            Image image = ((Button)sender).Content as Image;

            //Check the pressed button
            if (image.Source.ToString().Split('/').Last() != "DL4 MkII Foot Green.png")
            {
                //Reset all footswitches
                imageFoot_A.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot None.png"));
                imageFoot_B.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot None.png"));
                imageFoot_C.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot None.png"));

                //Set the selected one
                image.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/DL4 MkII Foot Green.png"));
            }
        }

        /*
        Check if the input character is a number
        */
        private void TextBoxNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text.ToCharArray()[0]);
        }

        /*
        Saves the current device configuration
        */
        private void ButtonSavePreset_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int iPreset = int.Parse(textboxPreset.Text);
            if (iPreset >= Constants.PRESET_COUNT_MIN && iPreset <= Constants.PRESET_COUNT_MAX)
            {
                IControlConfig.Instance.SavePreset(iPreset, currentConfig);
            }
            else
            {
                Console.WriteLine("Error: Preset number outside bounds");
            }
        }
    }
}
