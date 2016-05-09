using Hermes.WebApi.Core.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.WebApi.Test.ConfigurationTest
{
	public class FirstComponentConfiguration : ConfigurationSection
	{
		public FirstComponentConfiguration()
		{
		}

		/// <summary>
		/// Gets the Accounting Physical Path
		/// </summary>
		[ConfigurationProperty("UserName")]
		public string UserName
		{
			get { return (string)this["UserName"]; }
		}

		[ConfigurationProperty("Password")]
		public string Password 
		{
			get { return (string)this["Password"]; }
		}
	}
}
