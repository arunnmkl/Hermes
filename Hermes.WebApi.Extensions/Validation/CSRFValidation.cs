// ***********************************************************************
// Assembly         : Hermes.WebApi.Extensions
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="CSRFValidation.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using Hermes.WebApi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.WebApi.Extensions.Validation
{
	/// <summary>
	/// Class CSRFValidation.
	/// </summary>
	public class CSRFValidation : ICSRFValidation
	{
		/// <summary>
		/// Gets the CSRF value.
		/// </summary>
		/// <returns>System.String.</returns>
		public string GetCSRFValue()
		{
			return Math.Abs((new Random()).Next(Guid.NewGuid().GetHashCode(), int.MaxValue)).ToString();
		}

		/// <summary>
		/// Validates the specified actual.
		/// </summary>
		/// <param name="actual">The actual.</param>
		/// <param name="expected">The expected.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool Validate(string actual, string expected)
		{
			return !string.Equals(actual, expected);
		}
	}
}
