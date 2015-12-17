# SpelunkyWad

A C# library for manipulating Spelunky WAD (and to a lesser extent, WIX) files.

## File structure

A WAD file in Spelunky is an archive of data. It is accompanied by a WIX file which describes the various groups and entries within the WAD file itself.

An archive can have multiple groups, and these groups can contain multiple files.

## Support

The library can freely load and save to and from WAD files, and will generate the appropriate WIX files when necessary.

## Example

A basic example to load a WAD and WIX file would be as follows.

```csharp

// Load an archive named 'MyArchive' into memory.
// You can manually specify a matching WIX file.

var archive = new Archive("MyArchive.wad", "MyArchive.wix");
archive.Load();

foreach (var group in archive.Groups)
{
	// Perform some processing on the groups.

	foreach (var entry in group.Entries)
	{
		// Perform some processing on the entries.
	}
}

// Save any changes back to the file.

archive.Save();
```