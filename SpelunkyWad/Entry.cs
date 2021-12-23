using System;
using System.IO;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents an individual entry.
	/// </summary>
	public sealed class Entry
	{
		/// <summary>
		/// Creates a new entry instance with the specified name sourcing from the specified stream.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="stream">The stream.</param>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		public Entry(string name, Stream stream)
		{
			this.name = name ?? throw new ArgumentNullException(nameof(name));
			this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
		}

		/// <summary>
		/// Creates a new entry instance with the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		public Entry(string name)
		{
			this.name = name ?? throw new ArgumentNullException(nameof(name));
			this.stream = new MemoryStream();
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get => this.name;
			set => this.name = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Gets or sets the stream.
		/// </summary>
		public Stream Stream
		{
			get => this.stream;
			set => this.stream = value ?? throw new ArgumentNullException(nameof(value));
		}

		private string name = null;
		private Stream stream = null;
	}
}
