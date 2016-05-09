using System.Collections.Generic;

namespace Hermes.WebApi.Base.SqlSerializer
{
	/// <summary>
	///
	/// </summary>
	public abstract class MultiColumnPropertyMappingAttribute : MappingAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MultiColumnPropertyMappingAttribute"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		protected MultiColumnPropertyMappingAttribute(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Gets the column names.
		/// </summary>
		/// <returns></returns>
		public abstract List<string> GetColumnNames();

		/// <summary>
		/// Constructs the value.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <param name="propertyBag">The property bag.</param>
		/// <returns></returns>
		public abstract object ConstructValue(object obj, Dictionary<string, object> propertyBag);
	}
}