using Sharpknife.Core;
using Sharpknife.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpelunkyWad.Utility
{
	class Program
	{
		/// <summary>
		/// Main method.
		/// </summary>
		/// <param name="args">application arguments</param>
		static void Main(string[] args)
		{
			//command line
			var commandLine = new CommandLine(args);

			try
			{
				//arguments
				var input = commandLine.GetString("-input");
				var output = commandLine.GetString("-output");
				var verbose = commandLine.GetFlag("verbose");

				if (Directory.Exists(input))
				{
					//debug
					Debug.Information("Compressing {0}...", input);

					//archive
					var archiveName = (output + ".wad");
					var archive = new Archive(archiveName);
					var directories = Directory.GetDirectories(input);

					foreach (var directory in directories)
					{
						//directory name
						var directoryName = Path.GetFileName(directory);

						if (verbose)
						{
							Debug.Warning("Compressing directory {0}...", directoryName);
						}

						//group path
						var groupPath = Path.Combine(input, directoryName);

						//group
						var group = new Group(directoryName);
						var files = Directory.GetFiles(directory);

						foreach (var file in files)
						{
							//name
							var fileName = Path.GetFileName(file);

							if (verbose)
							{
								Debug.Information("Compressing entry {0}...", fileName);
							}

							//entry path
							var entryPath = Path.Combine(groupPath, fileName);

							//entry
							var data = File.ReadAllBytes(entryPath);
							var entry = new Entry(fileName, data);

							//add
							group.Entries.Add(entry);
						}

						//add
						archive.Groups.Add(group);
					}

					if (verbose)
					{
						Debug.Warning("Saving archive...");
					}

					//save
					archive.Save();
				}
				else
				{
					//debug
					Debug.Information("Extracting {0}...", input);

					//archive
					var archive = new Archive(input);
					archive.Load();

					foreach (var group in archive.Groups)
					{
						if (verbose)
						{
							Debug.Warning("Extracting group {0}...", group.Name);
						}

						//group path
						var groupPath = Path.Combine(output, group.Name);

						//directory
						Directory.CreateDirectory(groupPath);

						foreach (var entry in group.Entries)
						{
							if (verbose)
							{
								Debug.Information("Extracting entry {0}...", entry.Name);
							}

							//entry path
							var entryPath = Path.Combine(groupPath, entry.Name);

							//extract
							File.WriteAllBytes(entryPath, entry.Data);
						}
					}
				}


				//debug
				Debug.Information("Completed successfully.");
			}
			catch (Exception exception)
			{
				//error
				Debug.Error(exception.Message);
			}

			//wait
			Debug.AnyKeyPrompt();
		}
	}
}
