using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Abstractions_ASQL_03
{
    static class LabelChanger
    {
        /// <summary>
        /// Change the msg of a label
        /// </summary>
        /// <param name="msg">The new message</param>
        /// <param name="label">The label to change</param>
        /// <returns>Validation</returns>
        static public bool ChangeText(string msg, Label label)
        {
            bool validation = false;
            string oldText = label.Text.ToString();

            label.Text = msg;
            if (oldText != label.Text.ToString())
                validation = true;

            return validation;
        }

        /// <summary>
        /// Change the color of the labels foreground text
        /// </summary>
        /// <param name="color">The color to change into</param>
        /// <param name="label">The label to change</param>
        /// <returns>Currently always TRUE</returns>
        static public bool ChangeColor(string color, Label label)
        {
            bool validation = true;

            if (color == "Red" || color == "red")
                label.ForeColor = Color.Red;
            else if (color == "Blue" || color == "blue")
                label.ForeColor = Color.Blue;
            else if (color == "Green" || color == "green")
                label.ForeColor = Color.Green;
            else
                label.ForeColor = Color.Black;

            return validation;
        }
    }
}
