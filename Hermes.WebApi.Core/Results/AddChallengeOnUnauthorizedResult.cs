// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="AddChallengeOnUnauthorizedResult.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hermes.WebApi.Core.Results
{
	/// <summary>
	/// THis is used to add the challenges to the response as this is the part of the handshake
	/// </summary>
	/// <see cref="https://en.wikipedia.org/wiki/Challenge%E2%80%93response_authentication" />
	public class AddChallengeOnUnauthorizedResult : IHttpActionResult
	{
		/// <summary>
		/// Gets the challenge.
		/// </summary>
		/// <value>The challenge.</value>
		public AuthenticationHeaderValue Challenge { get; private set; }

		/// <summary>
		/// Gets the inner result.
		/// </summary>
		/// <value>The inner result.</value>
		public IHttpActionResult InnerResult { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AddChallengeOnUnauthorizedResult" /> class.
		/// </summary>
		/// <param name="challenge">The challenge.</param>
		/// <param name="innerResult">The inner result.</param>
		public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
		{
			Challenge = challenge;
			InnerResult = innerResult;
		}

		/// <summary>
		/// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage" />.</returns>
		public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				// Only add one challenge per authentication scheme.
				if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
				{
					response.Headers.WwwAuthenticate.Add(Challenge);
				}
			}

			return response;
		}
	}
}