using System;

namespace Hermes.WebApi.Core.Exceptions
{
	[Serializable]
	public class HermesException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HermesException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception.</param>
		public HermesException(string message, Exception exception)
			: base(message, exception)
		{
		}

		public HermesException(string message)
			: base(message)
		{
		}
	}
}