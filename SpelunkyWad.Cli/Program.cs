using System;
using System.IO;
using System.Linq;
using CommandLine;

namespace SpelunkyWad.Cli
{
	public static class Program
	{
		private abstract class Options
		{
			[Option('w', "wad", Required = true, HelpText = "Specifies the WAD file.")]
			public string Wad { get; set; } = null;

			[Option('x', "wix", Required = true, HelpText = "Specifies the WIX file.")]
			public string Wix { get; set; } = null;
		}

		[Verb("create", HelpText = "Create an archive from files.")]
		private sealed class CreateOptions : Options
		{
			[Option('i', "input", Required = true, HelpText = "Specifies the input directory.")]
			public string Input { get; set; } = null;
		}

		[Verb("extract", HelpText = "Extract files from an archive.")]
		private sealed class ExtractOptions : Options
		{
			[Option('o', "output", Required = true, HelpText = "Specifies the output directory.")]
			public string Output { get; set; } = null;
		}

		[Verb("patch", HelpText = "Patch an archive in-place with another archive.")]
		private sealed class PatchOptions : Options
		{
			[Option("patch-wad", Required = true, HelpText = "Specifies the patching WAD file.")]
			public string PatchWad { get; set; } = null;

			[Option("patch-wix", Required = true, HelpText = "Specifies the patching WIX file.")]
			public string PatchWix { get; set; } = null;
		}

		public static void Main(string[] args)
		{
			Parser.Default
				.ParseArguments<CreateOptions, ExtractOptions, PatchOptions>(args)
				.WithParsed<CreateOptions>(options =>
				{
					using (var wadStream = new FileStream(options.Wad, FileMode.Create))
					using (var wixStream = new FileStream(options.Wix, FileMode.Create))
					{
						Console.Out.WriteLine("Creating archive...");

						var archive = new Archive();

						foreach (var directory in Directory.GetDirectories(options.Input))
						{
							var group = new Group(Path.GetFileName(directory));

							foreach (var file in Directory.GetFiles(directory))
							{
								Console.Out.WriteLine($"Creating entry '{Path.GetFileName(directory)}/{Path.GetFileName(file)}'...");

								var entry = new Entry(Path.GetFileName(file), new FileStream(file, FileMode.Open));

								group.Entries.Add(entry);
							}

							archive.Groups.Add(group);
						}

						Console.Out.WriteLine("Writing archive...");

						ArchiveWriter.Write(archive, wadStream, wixStream);
					}
				})
				.WithParsed<ExtractOptions>(options =>
				{
					using (var wadStream = new FileStream(options.Wad, FileMode.Open))
					using (var wixStream = new FileStream(options.Wix, FileMode.Open))
					{
						Console.Out.WriteLine("Reading archive...");

						var archive = ArchiveReader.Read(wadStream, wixStream);

						Console.Out.WriteLine("Extracting archive...");

						foreach (var group in archive.Groups)
						{
							Directory.CreateDirectory(Path.Combine(options.Output, group.Name));

							foreach (var entry in group.Entries)
							{
								Console.Out.WriteLine($"Extracting entry '{group.Name}/{entry.Name}'...");

								using (var fileStream = new FileStream(Path.Combine(options.Output, group.Name, entry.Name), FileMode.Create))
								{
									entry.Stream.CopyTo(fileStream);
								}
							}
						}
					}
				})
				.WithParsed<PatchOptions>(options =>
				{
					string temporaryWad = null;
					string temporaryWix = null;

					using (var wadStream = new FileStream(options.Wad, FileMode.Open))
					using (var wixStream = new FileStream(options.Wix, FileMode.Open))
					using (var patchWadSream = new FileStream(options.PatchWad, FileMode.Open))
					using (var patchWixStream = new FileStream(options.PatchWix, FileMode.Open))
					{
						Console.Out.WriteLine("Reading archive...");

						var archive = ArchiveReader.Read(wadStream, wixStream);

						Console.Out.WriteLine("Reading patch archive...");

						var patchArchive = ArchiveReader.Read(patchWadSream, patchWixStream);

						foreach (var group in archive.Groups)
						{
							var patchGroup = patchArchive.Groups.FirstOrDefault(patchGroup => patchGroup.Name == group.Name);

							if (patchGroup == null)
							{
								continue;
							}

							foreach (var entry in group.Entries)
							{
								var patchEntry = patchGroup.Entries.FirstOrDefault(patchEntry => patchEntry.Name == entry.Name);

								if (patchEntry == null)
								{
									continue;
								}

								Console.Out.WriteLine($"Patching entry '{group.Name}/{entry.Name}'...");

								entry.Stream = patchEntry.Stream;
							}
						}

						Console.Out.WriteLine("Creating temporary files...");

						temporaryWad = Path.GetTempFileName();
						temporaryWix = Path.GetTempFileName();

						using (var temporaryWadStream = new FileStream(temporaryWad, FileMode.Create))
						using (var temporaryWixStream = new FileStream(temporaryWix, FileMode.Create))
						{
							Console.Out.WriteLine("Writing archive...");

							ArchiveWriter.Write(archive, temporaryWadStream, temporaryWixStream);
						}
					}

					Console.Out.WriteLine("Moving temporary files...");

					File.Move(temporaryWad, options.Wad, true);
					File.Move(temporaryWix, options.Wix, true);
				});
		}
	}
}
