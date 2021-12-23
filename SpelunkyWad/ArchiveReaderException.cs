using System;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents an <see cref="ArchiveReader" /> related exception.
	/// </summary>
	public sealed class ArchiveReaderException : Exception
	{
		/// <summary>
		/// Creates a new archive reader exception instance with the specified message caused by the specified exception.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="exception">The exception</param>
		public ArchiveReaderException(string message, Exception exception) : base(message, exception)
		{

		}

		/// <summary>
		/// Creates a new archive reader exception instance with the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		public ArchiveReaderException(string message) : base(message)
		{

		}
	}
}
