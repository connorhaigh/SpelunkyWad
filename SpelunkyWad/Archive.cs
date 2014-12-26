using System;
using System.Collections.Generic;
using System.IO;
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
		/// <param name="wadFile">the WAD file</param>
		/// <param name="wixFile">the WIX file</param>
		public Archive(string wadFile, string wixFile)
		{
			this.WadFile = wadFile;
			this.WixFile = wixFile;

			this.Groups = new List<Group>();
		}

		/// <summary>
		/// Returns a string representation of this archive.
		/// </summary>
		/// <returns>a string representation</returns>
		public override string ToString()
		{
			return string.Format("Archive (WAD File: {0}, WIX File: {1}, Groups: {2})", this.WadFile, this.WixFile, this.Groups);
		}

		/// <summary>
		/// Loads this WAD archive from file.
		/// </summary>
		public void Load()
		{
			//default
			var group = new Group("Default");

			using (StreamReader streamReader = new StreamReader(File.Open(this.WixFile, FileMode.Open)))
			{
				while (!streamReader.EndOfStream)
				{
					//read
					var line = streamReader.ReadLine();
					var parts = line.Split(' ');
					var identifier = parts[0];

					switch (identifier)
					{
						case "!group":
						{
							//new group
							group = new Group(parts[1]);
							this.Groups.Add(group);

							break;
						}
						default:
						{
							//entry
							var name = parts[0];
							var offset = int.Parse(parts[1]);
							var length = int.Parse(parts[2]);

							//add
							var entry = new Entry(name, offset, length);
							group.Entries.Add(entry);

							break;
						}
					}
				}
			}
		}

		/// <summary>
		/// Reads and returns the contents of the specified entry.
		/// </summary>
		/// <param name="entry">the entry</param>
		/// <returns>the contents</returns>
		public byte[] Read(Entry entry)
		{
			using (FileStream fileStream = File.Open(this.WadFile, FileMode.Open))
			{
				//result
				var result = new byte[entry.Length];

				//read
				fileStream.Seek(entry.Offset, SeekOrigin.Begin);
				fileStream.Read(result, 0, entry.Length);

				return result;
			}
		}

		/// <summary>
		/// Extracts the specified entry to the specified location.
		/// </summary>
		/// <param name="entry">the entry</param>
		/// <param name="file">the file</param>
		public void Extract(Entry entry, string file)
		{
			//read
			var data = this.Read(entry);

			using (FileStream fileStream = File.Open(file, FileMode.CreateNew))
			{
				//write
				fileStream.Write(data, 0, data.Length);
			}
		}

		/// <summary>
		/// The location of the WAD file for this archive.
		/// </summary>
		public string WadFile
		{
			get;
			set;
		}

		/// <summary>
		/// The location of the WIX file for this archive.
		/// </summary>
		public string WixFile
		{
			get;
			set;
		}

		/// <summary>
		/// The groups in this archive..
		/// </summary>
		public List<Group> Groups
		{
			get;
			set;
		}
	}
}
