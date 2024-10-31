/*
MIDI CONTROL 2023

Functions.cs

- Description: Functions class
- Author: David Molina Toro
- Date: 02 - 02 - 2023
- Version: 1.7
*/

using System.Windows;
using System.Windows.Media;

namespace MidiControl
{
    internal class Functions
    {
        /*
        Returns the children of the given object with the given name
        */
        public static T FindChild<T>(DependencyObject parent, string sChildName) where T : DependencyObject
        {
            //Create a new found child object
            T foundChild = default;

            //Look for the child inside the given object
            int iChildrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < iChildrenCount; i++)
            {
                //Get the current child
                var child = VisualTreeHelper.GetChild(parent, i);

                //Check for the child type
                if (!(child is T))
                {
                    //Repeat the process until a child is found
                    foundChild = FindChild<T>(child, sChildName);

                    //Break if the child is found
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(sChildName))
                {
                    //Search for the child
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == sChildName)
                    {
                        //The child has been found by name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    //The child has been found the first time
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
