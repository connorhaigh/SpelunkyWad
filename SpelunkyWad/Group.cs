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
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			if (entries == null)
			{
				throw new ArgumentNullException(nameof(entries));
			}

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
		/// Returns a string representation of the group.
		/// </summary>
		/// <returns>the result</returns>
		public override string ToString()
		{
			return $"Group (Name: {this.Name}, Entries: {this.Entries})";
		}

		/// <summary>
		/// Returns if the group equals another object.
		/// </summary>
		/// <param name="instance">the instance</param>
		/// <returns>the result</returns>
		public override bool Equals(object instance)
		{
			var group = instance as Group;

			if (group != null)
			{
				return this.Name == group.Name && this.Entries == group.Entries;
			}

			return base.Equals(instance);
		}

		/// <summary>
		/// Returns the hash code for the group.
		/// </summary>
		/// <returns>the hash code</returns>
		public override int GetHashCode()
		{
			return this.Name.GetHashCode() ^ this.Entries.GetHashCode();
		}

		/// <summary>
		/// Gets the name of the group.
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the entries in the group.
		/// </summary>
		public List<Entry> Entries
		{
			get;
			private set;
		}
	}
}
