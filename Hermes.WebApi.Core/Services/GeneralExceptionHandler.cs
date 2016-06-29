// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="GeneralExceptionHandler.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using Hermes.WebApi.Core.Results;
using Hermes.WebApi.Core.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;
using Hermes.WebApi.Core.Exceptions;
using System.Net;

namespace Hermes.WebApi.Core.Services
{
    /// <summary>
    /// A class which handles all the exception in the application.
    /// </summary>
    /// <remarks>This is being used for the exception handling in the entire API</remarks>
    public class GeneralExceptionHandler : ExceptionHandler
    {
        /// <summary>
        /// When overridden in a derived class, handles the exception synchronously.
        /// </summary>
        /// <param name="context">The exception handler context.</param>
        public override void Handle(ExceptionHandlerContext context)
        {
            string contentMessage = "Oops! Sorry! Something went wrong. Please try again after some time.";

            if (context.ExceptionContext.Exception is AccessException
                || context.ExceptionContext.Exception is AuthorizationException)
            {
                contentMessage = context.ExceptionContext.Exception.Message;
                context.Result = new GeneralErrorResult("Unauthorized due to ACL on resource", context.ExceptionContext.Request, contentMessage, HttpStatusCode.Forbidden);
                return;
            }

            if (Configuration.Current.IsHandleUnHandledException)
            {
                object errorMessage = new
                {
                    Message = contentMessage,
                    MessageDetail = context.ExceptionContext.Exception.Message ?? string.Concat("Exception Message: ", context.ExceptionContext.Exception.ToString())
                };
                context.Result = new GeneralErrorResult(context.ExceptionContext.Request, contentMessage, errorMessage);
                return;
            }
            else if (!Configuration.Current.ExceptionSuppressed)
            {
                contentMessage = string.Concat(contentMessage, "Exception Message: ", context.ExceptionContext.Exception.ToString());
            }

            context.Result = new GeneralErrorResult(context.ExceptionContext.Request, contentMessage);
        }

        /// <summary>
        /// When overridden in a derived class, handles the exception asynchronously.
        /// </summary>
        /// <param name="context">The exception handler context.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous exception handling operation.</returns>
        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            Handle(context);

            return Task.FromResult(0);
        }
    }
}