﻿namespace SFA.WebProxy.Diagnostics
{
    using System;

    /// <summary>
	/// Exception thrown whenever certain <see cref="ValueCheck"/> assertions
	/// fail.
	/// </summary>
	public class AssertFailedException : Exception
    {
        /// <summary>
        /// Constructs a new <see cref="AssertFailedException"/> object.
        /// </summary>
        /// <param name="message">
        /// The message to assign to the <see cref="Exception.Message"/>
        /// property.
        /// </param>
        public AssertFailedException(string message)
            : base(message)
        { }

        /// <summary>
        /// Constructs a new <see cref="AssertFailedException"/> object.
        /// </summary>
        /// <param name="message">
        /// The message to assign to the <see cref="Exception.Message"/>
        /// property.
        /// </param>
        /// <param name="innerException">
        /// The exception that caused this exception to occur.
        /// </param>
        public AssertFailedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }

    /// <summary>
    /// Contains value-checking methods that provide simple,
    /// <see cref="System.Diagnostics.Assert">Assert</see>-like interfaces
    /// to various kinds of value checks.  These methods are not DEBUG-
    /// conditional; they will always be compiled in, so they are suitable
    /// for checking parameter values in library-level code.
    /// </summary>
    public class ValueCheck
    {
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the given value is
        /// <c>null</c>.
        /// </summary>
        /// <param name="argName">
        /// The value to pass as the argument name to
        /// <see cref="ArgumentNullException"/>.
        /// </param>
        /// <param name="val">The value to check for null</param>
        public static void AssertNotNullArg(string argName, object val)
        {
            if (val == null)
                throw new ArgumentNullException(argName);
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the given value
        /// is <c>null</c>.
        /// </summary>
        /// <param name="argName">
        /// The value to pass as the argument name to
        /// <see cref="ArgumentNullException"/>.
        /// </param>
        /// <param name="val">The value to check for <c>null</c>.</param>
        /// <param name="message">
        /// A message to include in the exception that is thrown.
        /// </param>
        public static void AssertNotNullArg(
            string argName, object val, string message)
        {
            if (val == null)
                throw new ArgumentNullException(argName, message);
        }

        /// <summary>
        /// Throws <see cref="AssertFailedException"/> if the given
        /// object is null.
        /// </summary>
        /// <param name="val">The value to check for <c>null</c>.</param>
        /// <param name="message">
        /// A message to include in the exception that is thrown.
        /// </param>
        public static void AssertNotNull(object val, string message)
        {
            if (val == null)
                throw new AssertFailedException(message);
        }

        /// <summary>
        /// Throws <see cref="AssertFailedException"/> if the given
        /// condition value is false.
        /// </summary>
        /// <param name="cond">The condition to check.</param>
        /// <param name="message">
        /// A message to include in the exception that is thrown.
        /// </param>
        public static void AssertTrue(bool cond, string message)
        {
            if (!cond)
                throw new AssertFailedException(message);
        }

        /// <summary>
        /// Throws <see cref="ArgumentException"/> if the given condition
        /// value is false.
        /// </summary>
        /// <param name="argName">
        /// The value to pass as the argument name to
        /// <see cref="ArgumentException"/>.
        /// </param>
        /// <param name="cond">The condition to check.</param>
        /// <param name="message">
        /// A message to include in the exception that is thrown.
        /// </param>
        public static void AssertTrueArg(
            string argName, bool cond, string message)
        {
            if (!cond)
                throw new ArgumentException(message, argName);
        }

        /// <summary>
        /// Throws <see cref="ArgumentException"/> if the given string value
        /// is null or empty.
        /// </summary>
        /// <param name="argName">
        /// The value to pass as the argument name to
        /// <see cref="ArgumentNullException"/>.
        /// </param>
        /// <param name="strValue">The string value to check.</param>
        public static void AssertNonEmptyStringArg(
            string argName, string strValue)
        {
            if (strValue == null || strValue == "")
            {
                throw new ArgumentNullException(
                    argName, NullOrZeroLengthMessage
                );
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the given array
        /// is null or empty.
        /// </summary>
        /// <param name="argName">
        /// The value to pass as the argument name to
        /// <see cref="ArgumentNullException"/>.
        /// </param>
        /// <param name="arr">The array to check.</param>
        public static void AssertNonEmptyArrayArg(string argName, object[] arr)
        {
            if (arr == null || arr.Length == 0)
            {
                throw new ArgumentNullException(
                    argName, NullOrZeroLengthMessage
                );
            }
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if the given
        /// value is not in the given range (inclusive).
        /// </summary>
        /// <param name="argName">
        /// The value to pass as the argument name to
        /// <see cref="ArgumentNullException"/>.
        /// </param>
        /// <param name="val">The value to check.</param>
        /// <param name="min">The minimum for the value.</param>
        /// <param name="max">The maximum for the value.</param>
        public static void AssertInRangeArg(
            string argName, double val, double min, double max)
        {
            if (val < min || val > max)
                throw new ArgumentOutOfRangeException(argName);
        }

        /// <summary>
        /// Retrieves a string containing "Cannot be null or zero-length.".
        /// </summary>
        public static string NullOrZeroLengthMessage
        {
            get { return "Cannot be null or zero-length."; }
        }
    }
}