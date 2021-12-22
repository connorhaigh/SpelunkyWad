using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SubstreamSharp;

namespace SpelunkyWad
{
	/// <summary>
	/// Provides the ability to load <see cref="Archive" /> instances in the Spelunky format.
	/// </summary>
	public static class ArchiveReader
	{
		/// <summary>
		/// Reads the <see cref="Archive" /> instance from the specified WAD stream alongside the specified WIX stream.
		/// </summary>
		/// <param name="wadStream">The WAD stream.</param>
		/// <param name="wixStream">The WIX stream.</param>
		/// <returns>The archive.</returns>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		/// <exception cref="ArchiveReaderException">If the archive cannot be read.</exception>
		public static Archive Read(Stream wadStream, Stream wixStream)
		{
			if (wadStream == null)
			{
				throw new ArgumentNullException(nameof(wadStream));
			}

			if (wixStream == null)
			{
				throw new ArgumentNullException(nameof(wixStream));
			}

			var wixStreamReader = new StreamReader(wixStream);

			var archive = new Archive();

			Group group = null;
			Entry entry = null;

			while (!wixStreamReader.EndOfStream)
			{
				var line = wixStreamReader
					.ReadLine()
					.Trim();

				if (string.IsNullOrWhiteSpace(line))
				{
					continue;
				}

				var tokens = line
					.Split(new char[] { ' ' })
					.Where(token => !string.IsNullOrWhiteSpace(token))
					.Select(token => token.Trim())
					.ToArray();

				if (tokens[0] == "!group")
				{
					if (tokens.Length < 2)
					{
						throw new ArchiveReaderException("Failed to parse group line due to insufficient tokens.");
					}

					archive.Groups.Add(group = new Group(tokens[1]));
				}
				else
				{
					if (tokens.Length < 3)
					{
						throw new ArchiveReaderException("Failed to parse entry line due to insufficient tokens.");
					}

					if (group == null)
					{
						throw new ArchiveReaderException("Attempted to add an entry with no previously defined group.");
					}

					if (!long.TryParse(tokens[1], out var offset))
					{
						throw new ArchiveReaderException("Failed to parse entry offset.");
					}

					if (!long.TryParse(tokens[2], out var length))
					{
						throw new ArchiveReaderException("Failed to parse entry length.");
					}

					// We use a substream here as we only want to provide access to the single entry in the WAD file.
					// We could have used a memory stream here, but that would require having to load the entire contents into memory.
					// This is wrapped in a simple read-only stream to disallow any modification directly to the underlying stream.

					group.Entries.Add(entry = new Entry(tokens[0], new EntryStream(new Substream(wadStream, offset, length))));
				}
			}

			return archive;
		}

		/// <summary>
		/// Asynchronously reads the <see cref="Archive" /> instance from the specified WAD stream alongside the specified WIX stream.
		/// </summary>
		/// <param name="wadStream">The WAD stream.</param>
		/// <param name="wixStream">The WIX stream.</param>
		/// <returns>A task representing the operation.</returns>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		/// <exception cref="ArchiveReaderException">If the archive cannot be read.</exception>
		public static Task<Archive> ReadAsync(Stream wadStream, Stream wixStream) => Task.Run(() => ArchiveReader.Read(wadStream, wixStream));
	}
}
