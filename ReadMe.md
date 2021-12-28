# SpelunkyWad

SpelunkyWad is a C# library for reading from and writing to WAD and WIX files in the Spelunky format.

## Overview

Spelunky stores the majority of its game data within WAD and WIX files. WAD files are uncompressed binary files with each entry packed next to each other. WIX files are uncompressed text files with each entry listed with its name, offset, and length within the corresponding WAD file. WIX files can contain named groups and each entry must belong to a particular group.

The contents of WAD and WIX files can be freely created, updated and deleted. Groups can be added and removed, and entries can be added and removed as well. An individual entry provides its data through a stream property allowing for flexibility for manipulating said data.

Included is also a command-line application which can be used to perform a few basic operations on WAD and WIX files, namely the creation of custom archives and the extraction of existing archives.

## Example

Reading an archive from a WAD file and a WIX file.

```csharp
using SpelunkyWad;

using (var wadStream = new FileStream("archive.wad", FileMode.Open))
using (var wixStream = new FileStream("archive.wix", FileMode.Open))
{
	var archive = ArchiveReader.Read(wadStream, wixStream);

	foreach (var group in archive.Groups)
	{
		// Process the group...

		foreach (var entry in group.Entries)
		{
			// Process the entry...
		}
	}
}
```

Writing an archive to a WAD file and a WIX file.

```csharp
using SpelunkyWad;

var archive = new Archive();

// Add groups and entries...

using (var wadStream = new FileStream("archive.wad", FileMode.Create))
using (var wixStream = new FileStream("archive.wix", FileMode.Create))
{
	ArchiveWriter.Write(archive, wadStream, wixStream);
}
```

## Notes

Entries are not loaded into memory when an archive is read. Instead, their streams point towards the specific region in the WAD file which encapsulates their contents. Due to this, it is important to keep the underlying WAD and WIX streams open for the duration the archive is being used. It is also important to ensure that if the same streams are used for both a read operation and write operation, that both are are moved to the beginning via the `Seek` method before the write operation.

As a side effect, the initial stream for each entry will only support read operations and cannot be written to. This is to avoid inadvertently modifying the underlying streams outside the context of a write operation. In order to perform an in-place replacement of the contents of an entry, replace the `Stream` property entirely.

In some official Spelunky archives, entries are occasionally duplicated in different groups but with the same name as well as same offset and length. While the library does support reading duplicate entries, it will not write duplicate entries; each entry will be written separately regardless if the data is duplicated.
