// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="ILog.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace Hermes.WebApi.Core.Interfaces
{
	/// <summary>
	/// Interface ILog
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Logs the specified ex.
		/// </summary>
		/// <param name="ex">The ex.</param>
		void Log(Exception ex);
	}
}