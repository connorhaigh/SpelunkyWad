# SpelunkyWad

A .NET library for manipulating Spelunky WAD (and to a lesser extent, WIX) files.

## File structure

A WAD file in Spelunky is an archive of data. It is accompanied by a WIX file which describes the various groups and entries within the WAD file itself.

An archive can have multiple groups, and these groups can contain multiple files.

## Example

A basic example to load a WAD and WIX file would be as follows.

```csharp
var archive = new Archive("MyArchive.wad", "MyArchive.wix");
archive.Load();
```

```csharp
foreach (var group in archive.Groups)
{
	//do something with the group

	foreach (var entry in group.Entries)
	{
		//do something with tne entry
	}
}
```