# SpelunkyWad

A C# library for manipulating Spelunky WAD (and to a lesser extent, WIX) files

## File structure

A WAD file in Spelunky is an archive of data. It is accompanied by a WIX file which describes the various groups and entries within the WAD file itself.

An archive can have multiple groups, and these groups can contain multiple files.

## Support

The library can freely load and save to and from WAD files, and will generate the appropriate WIX files when necessary.

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