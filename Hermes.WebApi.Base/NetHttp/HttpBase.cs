// ***********************************************************************
// Assembly         : Hermes.WebApi.Base
// Author           : avinash.dubey
// Created          : 01-22-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-22-2016
// ***********************************************************************
// <copyright file="HttpBase.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Net;

namespace Hermes.WebApi.Base.NetHttp
{
	/// <summary>
	/// Class HttpBase.
	/// </summary>
	public class HttpBase
	{
		/// <summary>
		/// The default cookie container
		/// </summary>
		protected static readonly CookieContainer DefaultCookieContainer = new CookieContainer();

		/// <summary>
		/// The _username
		/// </summary>
		protected static string _username = null;

		/// <summary>
		/// The _password
		/// </summary>
		protected static string _password = null;

		/// <summary>
		/// The _has sent credentials
		/// </summary>
		protected static bool _hasSentCredentials;

		/// <summary>
		/// The _processing record count
		/// </summary>
		protected static int _processingRecordCount;

		/// <summary>
		/// The _processing average time
		/// </summary>
		protected static double _processingAverageTime;

		/// <summary>
		/// The processing lock object
		/// </summary>
		protected static readonly object ProcessingLockObject = new object();

		/// <summary>
		/// The average client process time HTTP header
		/// </summary>
		public const string AvgClientProcessTimeHttpHeader = "X-Hermes-avgProcessTime";

		/// <summary>
		/// The timeout
		/// </summary>
		public static int Timeout = 10;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="HttpBase"/> is preauthenticate.
		/// </summary>
		/// <value><c>true</c> if preauthenticate; otherwise, <c>false</c>.</value>
		public static bool Preauthenticate { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [disable processing statisitcs].
		/// </summary>
		/// <value><c>true</c> if [disable processing statisitcs]; otherwise, <c>false</c>.</value>
		public static bool DisableProcessingStatisitcs { get; set; }

		/// <summary>
		/// Gets or sets the URI base.
		/// </summary>
		/// <value>The URI base.</value>
		public static string UriBase { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [disable HTTP keep alive].
		/// </summary>
		/// <value><c>true</c> if [disable HTTP keep alive]; otherwise, <c>false</c>.</value>
		public static bool DisableHttpKeepAlive { get; set; }
	}
}