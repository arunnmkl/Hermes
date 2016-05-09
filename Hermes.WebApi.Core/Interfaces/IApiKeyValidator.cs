// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="IApiKeyValidator.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.WebApi.Core.Interfaces
{
	/// <summary>
	/// Interface IApiKeyValidator
	/// </summary>
	public interface IApiKeyValidator
	{
		/// <summary>
		/// Validates the key.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool ValidateKey(HttpRequestMessage message);

		/// <summary>
		/// Validates the key.
		/// </summary>
		/// <param name="apiKey">The API key.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		bool ValidateKey(string apiKey);
	}
}
