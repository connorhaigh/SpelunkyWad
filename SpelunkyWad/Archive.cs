using System;
using System.Collections.Generic;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents a collective WAD file and WIX file.
	/// </summary>
	public sealed class Archive
	{
		/// <summary>
		/// Creates a new archive instance with the specified groups.
		/// </summary>
		/// <param name="groups">The groups.</param>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		public Archive(IEnumerable<Group> groups)
		{
			this.Groups = new List<Group>(groups ?? throw new ArgumentNullException(nameof(groups)));
		}

		/// <summary>
		/// Creates a new archive instance.
		/// </summary>
		public Archive()
		{
			this.Groups = new List<Group>();
		}

		/// <summary>
		/// Gets the groups.
		/// </summary>
		public IList<Group> Groups { get; } = null;
	}
}
