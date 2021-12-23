using System;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents an <see cref="ArchiveWriter" /> related exception.
	/// </summary>
	public sealed class ArchiveWriterException : Exception
	{
		/// <summary>
		/// Creates a new archive writer exception instance with the specified message caused by the specified exception.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception</param>
		public ArchiveWriterException(string message, Exception exception) : base(message, exception)
		{

		}

		/// <summary>
		/// Creates a new archive writer exception instance with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public ArchiveWriterException(string message) : base(message)
		{

		}
	}
}
