using System;
using System.Collections.Generic;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents a group, with a collection of individual entries.
	/// </summary>
	public sealed class Group
	{
		/// <summary>
		/// Creates a new group instance with the specified name and specified entries.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="entries">The entries.</param>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		public Group(string name, IEnumerable<Entry> entries)
		{
			this.name = name ?? throw new ArgumentNullException(name);
			this.Entries = new List<Entry>(entries ?? throw new ArgumentNullException(nameof(entries)));
		}

		/// <summary>
		/// Creates a new group instance with the specified name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		public Group(string name)
		{
			this.name = name ?? throw new ArgumentNullException(nameof(name));
			this.Entries = new List<Entry>();
		}

		/// <summary>
		/// Gets the name.
		/// </summary>
		public string Name
		{
			get => this.name;
			set => this.name = value ?? throw new ArgumentNullException(nameof(value));
		}

		/// <summary>
		/// Gets the entries.
		/// </summary>
		public IList<Entry> Entries { get; } = null;

		private string name = null;
	}
}
