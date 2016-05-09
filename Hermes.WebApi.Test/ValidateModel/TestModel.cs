using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.WebApi.Test.ValidateModelTest
{
	public class TestModel
	{
		[Required]
		public string Name { get; set; }
	}
}
