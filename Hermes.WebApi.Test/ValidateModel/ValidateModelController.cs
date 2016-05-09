using Hermes.WebApi.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Hermes.WebApi.Test.ValidateModelTest
{
	/// <summary>
	/// Controller
	/// </summary>
	public class ValidateModelController : ApiControllerBase
	{
		public IHttpActionResult Get()
		{
			return Ok();
		}

		//[ValidateModel]
		public IHttpActionResult Post(TestModel testModel)
		{
			return Ok();
		}
	}
}
