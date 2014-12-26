using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents an entry in a WAD archive.
	/// </summary>
	public class Entry
	{
		/// <summary>
		/// Creates a new entry.
		/// </summary>
		/// <param name="name">the name</param>
		/// <param name="offset">the offset</param>
		/// <param name="length">the length</param>
		public Entry(string name, int offset, int length)
		{
			this.Name = name;

			this.Offset = offset;
			this.Length = length;
		}

		/// <summary>
		/// Returns a string representation of this entry.
		/// </summary>
		/// <returns>a string representation</returns>
		public override string ToString()
		{
			return string.Format("Entry (Name: {0}, Offset: {1}, Length: {2})", this.Name, this.Offset, this.Length);
		}

		/// <summary>
		/// The name of this entry.
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// The offset of this entry.
		/// </summary>
		public int Offset
		{
			get;
			private set;
		}

		/// <summary>
		/// The length of this entry.
		/// </summary>
		public int Length
		{
			get;
			private set;
		}
	}
}
