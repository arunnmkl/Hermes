using System;

namespace Hermes.WebApi.Base.SqlSerializer
{
	[Flags]
	public enum ParameterFlags
	{
		Default,
		IdFieldsOnly,
		ExcludeIdentityFields
	}
}