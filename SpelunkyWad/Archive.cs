using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	/// <summary>
	/// Represents a WAD archive.
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
		/// <returns>a string representation</returns>
		public override string ToString()
		{
			return $"Archive (WAD: {this.Wad}, WIX: {this.Wix})";
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
