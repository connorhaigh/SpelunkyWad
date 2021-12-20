using System;
using System.IO;
using System.Threading.Tasks;

namespace SpelunkyWad
{
	/// <summary>
	/// Provides the ability to save <see cref="Archive" /> instances in the Spelunky format.
	/// </summary>
	public sealed class ArchiveWriter
	{
		/// <summary>
		/// Writes the specified <see cref="Archive" /> instance to the specified WAD stream alongside the specified WIX stream.
		/// </summary>
		/// <param name="archive">The archive.</param>
		/// <param name="wadStream">The WAD stream.</param>
		/// <param name="wixStream">The WIX stream.</param>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		/// <exception cref="ArchiveReaderException">If the archive cannot be written.</exception>
		public static void Write(Archive archive, Stream wadStream, Stream wixStream)
		{
			if (archive == null)
			{
				throw new ArgumentNullException(nameof(archive));
			}

			if (wadStream == null)
			{
				throw new ArgumentNullException(nameof(wadStream));
			}

			if (wixStream == null)
			{
				throw new ArgumentNullException(nameof(wixStream));
			}

			var wixStreamWriter = new StreamWriter(wixStream);

			foreach (var group in archive.Groups)
			{
				wixStreamWriter.WriteLine($"!group {group.Name.Trim()}");

				foreach (var entry in group.Entries)
				{
					if (!entry.Stream.CanSeek)
					{
						throw new ArchiveWriterException("Failed to write entry due to the stream being unseekable.");
					}

					if (!entry.Stream.CanRead)
					{
						throw new ArchiveWriterException("Failed to write entry due to the stream being unreadable.");
					}

					wixStreamWriter.WriteLine($"{entry.Name.Trim()} {wadStream.Position} {entry.Stream.Length}");

					entry.Stream.Seek(0L, SeekOrigin.Begin);
					entry.Stream.CopyTo(wadStream);
				}
			}

			wixStreamWriter.Flush();
		}

		/// <summary>
		/// Asynchronously writes the specified <see cref="Archive" /> instance to the specified WAD stream alongside the specified WIX stream.
		/// </summary>
		/// <param name="archive">The archive.</param>
		/// <param name="wadStream">The WAD stream.</param>
		/// <param name="wixStream">The WIX stream.</param>
		/// <returns>A task representing the operation.</returns>
		/// <exception cref="ArgumentNullException">If any arguments are null.</exception>
		/// <exception cref="ArchiveReaderException">If the archive cannot be written.</exception>
		public static Task WriteAsync(Archive archive, Stream wadStream, Stream wixStream) => Task.Run(() => ArchiveWriter.Write(archive, wadStream, wixStream));
	}
}
