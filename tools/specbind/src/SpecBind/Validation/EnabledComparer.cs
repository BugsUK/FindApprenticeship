﻿// <copyright file="EnabledComparer.cs">
//    Copyright © 2013 Dan Piessens  All rights reserved.
// </copyright>

namespace SpecBind.Validation
{
    using SpecBind.Pages;

    /// <summary>
    /// A validation comparer to see if something is enabled.
    /// </summary>
    public class EnabledComparer : ValidationComparerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnabledComparer"/> class.
        /// </summary>
        public EnabledComparer()
            : base("enabled", "isenabled")
        {
        }

        /// <summary>
        /// Compares the values using the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="actualValue">The actual value.</param>
        /// <returns><c>true</c> if the comparison passes, <c>false</c> otherwise.</returns>
        public override bool Compare(IPropertyData property, string expectedValue, string actualValue)
        {
            bool parsedValue;
            return (expectedValue != null && bool.TryParse(expectedValue, out parsedValue) && !parsedValue)
                       ? !property.CheckElementEnabled()
                       : property.CheckElementEnabled();
        }
    }
}