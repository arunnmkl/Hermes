using Hermes.WebApi.Core.Enums;
using Hermes.WebApi.Core.Results;
// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-22-2016
// ***********************************************************************
// <copyright file="ResponseExtensions.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using Hermes.WebApi.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Hermes.WebApi.Core
{
    /// <summary>
    /// This class provides extension methods used by Http response messages.
    /// </summary>
    public static class ResponseExtensions
    {
        /// <summary>
        /// Adds the cookie value with given key name.
        /// </summary>
        /// <param name="response">The response message.</param>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <param name="value">The value to be added in the cookie.</param>
        public static void AddCookie(this HttpResponseMessage response, string cookieName, string value)
        {
            response.Headers.Add("Cookie", string.Format("{0}={1};", cookieName, value));
        }

        /// <summary>
        /// Returns an individual HTTP Header value
        /// </summary>
        /// <param name="response">The response message.</param>
        /// <param name="key">The key.</param>
        /// <returns>The header value of the given header key.</returns>
        public static string GetHeader(this HttpResponseMessage response, string key)
        {
            IEnumerable<string> keys = null;
            if (!response.Headers.TryGetValues(key, out keys))
                return null;

            return keys.First();
        }

        /// <summary>
        /// Retrieves an individual cookie from the cookies collection.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="cookieName">Name of the cookie.</param>
        /// <returns>The value of the cookie for given name.</returns>
        public static string GetCookie(this HttpResponseMessage response, string cookieName)
        {
            IEnumerable<string> cookies = new List<string>();
            string cookieValue = string.Empty, cookieConstant = string.Format("{0}=", cookieName);
            if (response.Headers.TryGetValues("Cookie", out cookies) || response.Headers.TryGetValues("Set-Cookie", out cookies))
            {
                cookieValue = cookies.FirstOrDefault(c => c.StartsWith(cookieConstant));
            }

            if (cookieValue != null)
                return cookieValue.Replace(cookieConstant, string.Empty).Split(';').FirstOrDefault();

            return null;
        }

        /// <summary>
        /// Sets the authentication.
        /// </summary>
        /// <param name="response">The response message.</param>
        /// <param name="value">The value of ticket/token or any other value which can be stored in the auth value.</param>
        /// <param name="persist">if set to <c>true</c> [persist the cookie for one day].</param>
        public static void SetAuthentication(this HttpResponseMessage response, string value, bool persist = false)
        {
            if (response == null)
            {
                return;
            }

            CookieHeaderValue cookie = new CookieHeaderValue(Configuration.Current.AuthCookieName, value);
            cookie.Expires = DateTimeOffset.Now.AddDays(1);
            cookie.HttpOnly = true;
            cookie.Path = "/";
            response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
        }
    }
}