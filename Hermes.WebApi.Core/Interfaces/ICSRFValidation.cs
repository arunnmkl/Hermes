// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="ICSRFValidation.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.WebApi.Core.Interfaces
{
	/// <summary>
	/// Interface ICSRFValidation
	/// </summary>
	public interface ICSRFValidation
	{
		/// <summary>
		/// Gets the CSRF value.
		/// </summary>
		/// <returns>System.String.</returns>
		string GetCSRFValue();

		/// <summary>
		/// Validates the specified actual.
		/// </summary>
		/// <param name="actual">The actual.</param>
		/// <param name="expected">The expected.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool Validate(string actual, string expected);
	}
}
