// ***********************************************************************
// Assembly         : Hermes.WebApi.Extensions
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="ApiKeyValidator.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using Hermes.WebApi.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.WebApi.Extensions.Validation
{
	/// <summary>
	/// Class ApiKeyValidator.
	/// </summary>
	public class ApiKeyValidator : IApiKeyValidator
	{
		/// <summary>
		/// Gets the key.
		/// </summary>
		/// <value>The key.</value>
		public string Key { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ApiKeyValidator"/> class.
		/// </summary>
		/// <param name="key">The key.</param>
		public ApiKeyValidator(string key)
		{
			this.Key = key;
		}

		/// <summary>
		/// Validates the key.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		public bool ValidateKey(HttpRequestMessage message)
		{
			var query = message.RequestUri.ParseQueryString();
			string key = query["key"];
			return (key == Key);
		}

		/// <summary>
		/// Validates the key.
		/// </summary>
		/// <param name="apiKey">The API key.</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		/// <exception cref="System.NotImplementedException"></exception>
		public bool ValidateKey(string apiKey)
		{
			throw new NotImplementedException();
		}

	}
}
