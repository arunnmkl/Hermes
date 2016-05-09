using AmadeusConsulting.Simplex.Base.Serialization;
using Atlas.Core.Component;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.WebApi.Test.ConfigurationTest
{
	[TestClass]
	public class ComponentConfigurationTest
	{
		[TestMethod]
		public void ComponentConfigurationTest_Key_UserName()
		{
			var result = Context.ComponentSettings.UserName;

			Assert.AreEqual(result, "UserOne");
		}

		[TestMethod]
		public void ComponentConfigurationTest_Key_Password()
		{
			var result = Context.ComponentSettings.Password;

			Assert.AreEqual(result, "passwordTestOne");
		}

		[TestMethod]
		public void ComponentConfigurationTest_GtCommon_SqlSerializer_Success()
		{
			var result = Context.GtCommon;

			Assert.IsNotNull(result, "Context.GtCommon is null");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void ComponentConfigurationTest_GtCommon_SqlSerializer_Failed()
		{
			var result = SqlSerializer.ByName("authTset");

			Assert.IsNull(result, "Context.GtCommon is null");
		}

		[TestMethod]
		public void ComponentConfigurationTest_GtCommon_SqlSerializer_ComponentPaths()
		{
			var actualURL = "CommandCenter";

			var expectedURL = ComponentPaths.GetComponentPathByKey("CommandCenter");

			Assert.AreEqual(actualURL, expectedURL);
		}

		[TestMethod]
		public void ComponentConfigurationTest_GtCommon_SqlSerializer_ComponentPaths_CookieContainer()
		{
			var actualURL = "CommandCenter";

			var expectedURL = ComponentPaths.GetCookieContainerByKey("CommandCenter", "authCookie");

			Assert.AreEqual(actualURL, expectedURL);
		}
	}
}
