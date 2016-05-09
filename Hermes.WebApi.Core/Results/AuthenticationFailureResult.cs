// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="AuthenticationFailureResult.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hermes.WebApi.Core.Results
{
	/// <summary>
	/// This is an unauthorized response type
	/// </summary>
	public class AuthenticationFailureResult : IHttpActionResult
	{
		#region Properties

		/// <summary>
		/// Gets the reason phrase.
		/// </summary>
		/// <value>The reason phrase.</value>
		public string ReasonPhrase { get; private set; }

		/// <summary>
		/// Gets the request.
		/// </summary>
		/// <value>The request.</value>
		public HttpRequestMessage Request { get; private set; }

		#endregion Properties

		#region AuthenticationFailureResult

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationFailureResult" /> class.
		/// </summary>
		/// <param name="reasonPhrase">The reason phrase.</param>
		/// <param name="request">The request.</param>
		public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
		{
			ReasonPhrase = reasonPhrase;
			Request = request;
		}

		#endregion AuthenticationFailureResult

		#region Public Methods

		/// <summary>
		/// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage" />.</returns>
		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(Execute());
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Executes this instance.
		/// </summary>
		/// <returns>This is the http response for the unauthorized request.</returns>
		private HttpResponseMessage Execute()
		{
			HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
			response.RequestMessage = Request;
			response.ReasonPhrase = ReasonPhrase;
			return response;
		}

		#endregion Private Methods
	}
}