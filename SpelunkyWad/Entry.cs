using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents an individual entry.
	/// </summary>
	public class Entry
	{
		/// <summary>
		/// Creates a new entry.
		/// </summary>
		/// <param name="name">the name</param>
		/// <param name="data">the data</param>
		public Entry(string name, byte[] data)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			if (data == null)
			{
				throw new ArgumentNullException(nameof(data));
			}

			this.Name = name;
			this.Data = data;
		}

		/// <summary>
		/// Returns a string representation of the entry.
		/// </summary>
		/// <returns>the result</returns>
		public override string ToString()
		{
			return $"Entry (Name: {this.Name})";
		}

		/// <summary>
		/// Returns if the entry equals another object.
		/// </summary>
		/// <param name="instance">the instance</param>
		/// <returns>the result</returns>
		public override bool Equals(object instance)
		{
			var entry = instance as Entry;

			if (entry != null)
			{
				return this.Name == entry.Name && this.Data == entry.Data;
			}

			return base.Equals(instance);
		}

		/// <summary>
		/// Returns the hash code for the entry.
		/// </summary>
		/// <returns>the hash code</returns>
		public override int GetHashCode()
		{
			return this.Name.GetHashCode() ^ this.Data.GetHashCode();
		}

		/// <summary>
		/// Gets the name of the entry.
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the data for the entry.
		/// </summary>
		public byte[] Data
		{
			get;
			private set;
		}
	}
}
