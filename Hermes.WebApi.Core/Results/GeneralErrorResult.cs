// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="GeneralErrorResult.cs" company="">
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
	/// Class GeneralErrorResult.
	/// </summary>
	public class GeneralErrorResult : IHttpActionResult
	{
		/// <summary>
		/// Gets or sets the request.
		/// </summary>
		/// <value>The request.</value>
		public HttpRequestMessage Request { get; set; }
		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>The content.</value>
		public string Content { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GeneralErrorResult"/> class.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="content">The content.</param>
		public GeneralErrorResult(HttpRequestMessage request, string content)
		{
			Request = request;
			content = Content;
		}

		/// <summary>
		/// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
		/// </summary>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		/// <returns>A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage" />.</returns>
		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, Content);
			response.RequestMessage = Request;
			return Task.FromResult(response);
		}
	}
}