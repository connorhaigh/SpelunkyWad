using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents a Spelunky WAD archive.
	/// </summary>
	public class Archive
	{
		/// <summary>
		/// Creates a new WAD archive.
		/// </summary>
		/// <param name="wad">the WAD file</param>
		/// <param name="wix">the WIX file</param>
		public Archive(string wad, string wix)
		{
			if (wad == null)
			{
				throw new ArgumentNullException(nameof(wad));
			}

			if (wix == null)
			{
				throw new ArgumentNullException(nameof(wix));
			}

			this.Wad = wad;
			this.Wix = wix;

			this.Groups = new List<Group>();
		}

		/// <summary>
		/// Creates a new WAD archive.
		/// </summary>
		/// <param name="wadFile">the WAD file</param>
		public Archive(string wadFile) : this(wadFile, wadFile + ".wix")
		{

		}

		/// <summary>
		/// Returns a string representation of the archive.
		/// </summary>
		/// <returns>the result</returns>
		public override string ToString()
		{
			return $"Archive (WAD: {this.Wad}, WIX: {this.Wix})";
		}

		/// <summary>
		/// Returns if the archive equals another object.
		/// </summary>
		/// <param name="instance">the instance</param>
		/// <returns>the result</returns>
		public override bool Equals(object instance)
		{
			var archive = instance as Archive;

			if (archive != null)
			{
				return this.Wad == archive.Wad && this.Wix == archive.Wix;
			}

			return base.Equals(instance);
		}

		/// <summary>
		/// Returns the hash code for the archive.
		/// </summary>
		/// <returns>the hash code</returns>
		public override int GetHashCode()
		{
			return this.Wad.GetHashCode() ^ this.Wix.GetHashCode();
		}

		/// <summary>
		/// Loads the WAD archive from file.
		/// </summary>
		public void Load()
		{
			this.Groups = Parser.Load(this.Wad, this.Wix);
		}

		/// <summary>
		/// Saves the WAD archive to file.
		/// </summary>
		public void Save()
		{
			Parser.Save(this.Wad, this.Wix, this.Groups);
		}

		/// <summary>
		/// Gets or sets the location of the WAD file.
		/// </summary>
		public string Wad
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the location of the WIX file.
		/// </summary>
		public string Wix
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the groups.
		/// </summary>
		public List<Group> Groups
		{
			get;
			set;
		}
	}
}
