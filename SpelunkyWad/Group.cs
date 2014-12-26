using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents a group of entries.
	/// </summary>
	public class Group
	{
		/// <summary>
		/// Creates a new group.
		/// </summary>
		/// <param name="name">the name</param>
		/// <param name="entries">the entries</param>
		public Group(string name, List<Entry> entries)
		{
			this.Name = name;
			this.Entries = entries;
		}

		/// <summary>
		/// Creates a new group.
		/// </summary>
		/// <param name="name">the entries</param>
		public Group(string name) : this(name, new List<Entry>())
		{

		}

		/// <summary>
		/// Returns a string representation of this group.
		/// </summary>
		/// <returns>a string representation</returns>
		public override string ToString()
		{
			return string.Format("Group (Name: {0}, Entries: {1})", this.Name, this.Entries);
		}

		/// <summary>
		/// The name of this group.
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// The entries in this group.
		/// </summary>
		public List<Entry> Entries
		{
			get;
			private set;
		}
	}
}
