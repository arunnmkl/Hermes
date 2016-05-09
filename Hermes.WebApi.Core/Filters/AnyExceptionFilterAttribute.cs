﻿// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="AnyExceptionFilterAttribute.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Hermes.WebApi.Core.Filters
{
	/// <summary>
	/// Class AnyExceptionFilterAttribute.
	/// </summary>
	public class AnyExceptionFilterAttribute : ExceptionFilterAttribute
	{
		/// <summary>
		/// Called when [exception].
		/// </summary>
		/// <param name="context">The context.</param>
		public override void OnException(HttpActionExecutedContext context)
		{
			if (context.Exception is NotImplementedException)
			{
				context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
			}
		}
	}
}