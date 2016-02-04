using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	internal class Parser
	{
		public static List<Group> Load(string wad, string wix)
		{
			var groups = new List<Group>();

			Group group = null;

			using (FileStream wadReader = File.Open(wad, FileMode.Open))
			using (StreamReader wixStream = new StreamReader(File.Open(wix, FileMode.Open)))
			{
				while (!wixStream.EndOfStream)
				{
					var line = wixStream.ReadLine();
					var parts = line.Split(' ');
					var identifier = parts[0];

					switch (identifier)
					{
						case "!group":
						{
							if (group != null)
							{
								groups.Add(group);
							}

							group = new Group(parts[1]);

							break;
						}
						default:
						{
							var name = parts[0];
							var offset = int.Parse(parts[1]);
							var length = int.Parse(parts[2]);

							var data = new byte[length];

							wadReader.Seek(offset, SeekOrigin.Begin);
							wadReader.Read(data, 0, length);

							group.Entries.Add(new Entry(name, data));

							break;
						}
					}
				}

				if (group != null)
				{
					groups.Add(group);
				}
			}

			return groups;
		}

		public static void Save(string wad, string wix, List<Group> groups)
		{
			var offset = 0;

			using (FileStream wadStream = File.Open(wad, FileMode.Create))
			using (StreamWriter wixStream = new StreamWriter(File.Open(wix, FileMode.Create)))
			{
				foreach (var group in groups)
				{
					wixStream.WriteLine($"!group {group.Name}");

					foreach (var entry in group.Entries)
					{
						wadStream.Write(entry.Data, 0, entry.Data.Length);
						wixStream.WriteLine($"{entry.Name} {offset} {entry.Data.Length}");

						offset += entry.Data.Length;
					}
				}
			}
		}
	}
}
