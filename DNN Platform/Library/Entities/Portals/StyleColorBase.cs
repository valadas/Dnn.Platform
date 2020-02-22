// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Text.RegularExpressions;


namespace DotNetNuke.Entities.Portals
{
    /// <summary>
    /// Represetns a CSS color and it's components
    /// </summary>
    public class StyleColorBase
    {
        private enum Component
        {
            red,
            green,
            blue
        }

        private string _hex;

        /// <summary>
        /// Instantiates a new StyleColor with the default white color.
        /// </summary>
        public StyleColorBase()
        {
            _hex = "FFFFFF";
        }

        /// <summary>
        /// Instantiates a new StyleColor with the provided color but falls back to white if not provided or invalid.
        /// </summary>
        /// <param name="hexValue"></param>
        public StyleColorBase(string hexValue)
        {
            if (IsValidCssColor(hexValue))
            {
                this.HexValue = ExpandColor(hexValue);
            }
            else
            {
                this.HexValue = ExpandColor("FFFFFF");
            }
        }

        /// <summary>
        /// Gets or sets the hex color value using a 3 or 6 character long hexadecimal string.
        /// </summary>
        /// <remarks>Do not include the # sing.</remarks>
        /// <example>0F3, 000, FFF, 0012AB, 000000, FFFFFF</example>
        public string HexValue
        {
            get { return _hex; }
            set
            { 
                if (IsValidCssColor(value))
                {
                    this._hex = ExpandColor(value);
                } 
            }
        }

        /// <summary>
        /// Gets or sets the red value of the color as a byte (0 - 255).
        /// </summary>
        public byte Red
        {
            get { return this.GetComponent(Component.red); }
            set { this.SetComponent(Component.red, value); }
        }

        /// <summary>
        /// Gets or sets the green value of the color as a byte (0 - 255).
        /// </summary>
        public byte Green
        {
            get { return this.GetComponent(Component.green); }
            set { this.SetComponent(Component.green, value); }
        }

        /// <summary>
        /// Gets or sets the blue value of the color as a byte (0 - 255).
        /// </summary>
        public byte Blue
        {
            get { return this.GetComponent(Component.blue); }
            set { this.SetComponent(Component.blue, value); }
        }

        public string MinifiedHex
        {
            get
            {
                if (this._hex[0] == this._hex[1] &&
                    this._hex[2] == this._hex[3] &&
                    this._hex[4] == this._hex[5])
                {
                    return this._hex[0].ToString() + this._hex[2].ToString() + this._hex[4].ToString();
                }

                return this._hex;
            }
        }


        /// <summary>
        /// Gets the red, green or blue component of the current color.
        /// </summary>
        /// <param name="comp"></param>
        /// <returns></returns>
        private byte GetComponent(Component comp)
        {
            switch (comp)
            {
                case Component.red:
                    return byte.Parse(_hex.Substring(0, 2), NumberStyles.HexNumber);
                case Component.green:
                    return byte.Parse(_hex.Substring(2, 2), NumberStyles.HexNumber);
                case Component.blue:
                    return byte.Parse(_hex.Substring(4, 2), NumberStyles.HexNumber);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Sets the color hex based on a color component and value.
        /// </summary>
        /// <param name="comp">The component <see cref="Component"/> for which the value is for.</param>
        /// <param name="value">The byte value for the component (0-255).</param>
        private void SetComponent(Component comp, byte value)
        {
            switch (comp)
            {
                case Component.red:
                    this._hex = this._hex.Remove(0, 2).Insert(0, $"{value:x2}");
                    break;
                case Component.green:
                    this._hex = this._hex.Remove(2, 2).Insert(0, $"{value:x2}");
                    break;
                case Component.blue:
                    this._hex = this._hex.Remove(4, 2).Insert(0, $"{value:x2}");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Expands a 3 character hex value into it's 6 character version.
        /// </summary>
        /// <param name="hexValue"></param>
        /// <returns>a 6 character string uppercased.</returns>
        private static string ExpandColor(string hexValue)
        {
            if (hexValue.Length == 6)
            {
                return hexValue.ToUpperInvariant();
            }

            string value;
            char r = hexValue[0];
            char g = hexValue[1];
            char b = hexValue[2];
            value = string.Concat(r, r, g, g, b, b);
            return value.ToUpperInvariant();
        }

        /// <summary>
        /// Checks if a provided hex string is a valid 3 or 6 character CSS color hex value.
        /// </summary>
        /// <param name="hexValue">The string to test for validity.</param>
        /// <returns>True if valid, false if not.</returns>
        private static bool IsValidCssColor(string hexValue)
        {
            if (string.IsNullOrWhiteSpace(hexValue))
            {
                throw new ArgumentNullException("You need to provide a CSS color value in the constructor.");
            }

            Regex regex = new Regex(@"([\da-f]{3}){1,2}", RegexOptions.IgnoreCase);
            if (!regex.IsMatch(hexValue))
            {
                throw new ArgumentOutOfRangeException($"The value {hexValue} that was provided is not valid, it needs to be 3 or 6 characters long hexadecimal string without the # sing.");
            }

            return true;
        }
    }
}
