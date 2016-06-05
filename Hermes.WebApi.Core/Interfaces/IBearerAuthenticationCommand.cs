// ***********************************************************************
// Assembly         : Hermes.WebApi.Core
// Author           : avinash.dubey
// Created          : 01-19-2016
//
// Last Modified By : avinash.dubey
// Last Modified On : 01-21-2016
// ***********************************************************************
// <copyright file="IAuthenticationCommand.cs" company="">
//     Copyright ©  2015
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Hermes.WebApi.Core.Interfaces
{
    /// <summary>
    /// Interface IBearerAuthenticationCommand
    /// </summary>
    public interface IBearerAuthenticationCommand : IAuthenticationCommand
    {
        /// <summary>
        /// Gets or sets the authentication commands.
        /// </summary>
        /// <value>
        /// The authentication commands.
        /// </value>
        new HashSet<IBearerAuthentication> AuthenticationCommands { get; set; }

        /// <summary>
        /// Adds the new command.
        /// </summary>
        /// <param name="authentication">The authentication.</param>
        void AddNewCommand(IBearerAuthentication authentication);
    }
}