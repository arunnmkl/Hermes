// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-19-2016
// ***********************************************************************
// <copyright file="RoutingConfig.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace Hermes.WebApi.Core.Enums
{
	/// <summary>
	/// This is an ENUM to introduce the configuration for the web route creation
	/// </summary>
	/// <see cref="Hermes.WebApi.Core.Config" />
	public enum RoutingConfig
	{
		/// <summary>
		/// THis is for the default route creation.
		/// </summary>
		Default,

		/// <summary>
		/// This is used to resolve the same ApiContoller with same but in different namespace.
		/// </summary>
		/// <example>V1.CustomerController and V2.CustomerController</example>
		Namespace
	}
}