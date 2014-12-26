# SpelunkyWad

A .NET library for manipulating Spelunky WAD (and to a lesser extent, WIX) files.

## File structure

A WAD file in Spelunky is an archive of data. It is accompanied by a WIX file which describes the various groups and entries within the WAD file itself.

An archive can have multiple groups, and these groups can contain multiple files.

## Support

Currently, the library can read and extract entries from an archive. I plan to add saving support sometime in the future.

## Example

A basic example to load a WAD and WIX file would be as follows.

```csharp
var archive = new Archive("MyArchive.wad", "MyArchive.wix");
archive.Load();

foreach (var group in archive.Groups)
{
	//do something with the group

	foreach (var entry in group.Entries)
	{
		//do something with the entry
	}
}
```