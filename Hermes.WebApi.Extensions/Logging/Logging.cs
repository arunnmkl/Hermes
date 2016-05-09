// ***********************************************************************
// Assembly         : Hermes.WebApi.Extensions
// Author           : avinash.dubey
// Created          : 01-25-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-25-2016
// ***********************************************************************
// <copyright file="Logging.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using Hermes.WebApi.Core.Interfaces;
using Hermes.WebApi.Extensions.TraceSource;
using System;

namespace Hermes.WebApi.Extensions.Logging
{
	/// <summary>
	/// This class is used for logging
	/// </summary>
	public class Logging : ILog
	{
		/// <summary>
		/// Logs the specified exception.
		/// </summary>
		/// <param name="ex">The exception.</param>
		public void Log(Exception ex)
		{
			try
			{
				HermesTraceSource.Error(ex.Message, ex, new string[] { "Error" });
			}
			catch (Exception)
			{
			}

			//string userName = null;

			//if (Thread.CurrentPrincipal != null)
			//{
			//	userName = Thread.CurrentPrincipal.Identity.Name;
			//}

			//try
			//{
			//	using (SqlTraceListener traceListener = new SqlTraceListener("GTLog"))
			//	{
			//		traceListener.Write(new LogEntry
			//		{
			//			Exception = ex,
			//			Message = ex.Message,
			//			Severity = System.Diagnostics.TraceEventType.Error,
			//			UserName = userName,
			//			Categories = new string[] { "Error" },
			//			AssemblyFullName = Assembly.GetCallingAssembly().FullName
			//		});
			//	}
			//}
			//catch (Exception)
			//{
			//}
		}
	}
}