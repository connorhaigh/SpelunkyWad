using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpelunkyWad.Tests
{
	[TestClass]
	public sealed class ArchiveReaderTests
	{
		[TestMethod]
		public void TestRead()
		{
			var wad = new byte[]
			{
				27, 95, 17,
				69, 10, 45, 91, 28, 3,
				97, 57, 81, 8, 12, 63, 91, 76, 29
			};

			var wix = string.Join(Environment.NewLine,
				"!group Test",
				"Foo 0 3",
				"Bar 3 6",
				"Baz 9 9");

			using (var wadStream = new MemoryStream(wad.ToArray()))
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var archive = ArchiveReader.Read(wadStream, wixStream);

				Assert.AreEqual(1, archive.Groups.Count);
				Assert.AreEqual(3, archive.Groups[0].Entries.Count);

				Assert.AreEqual("Test", archive.Groups[0].Name);
				Assert.AreEqual("Foo", archive.Groups[0].Entries[0].Name);
				Assert.AreEqual("Bar", archive.Groups[0].Entries[1].Name);
				Assert.AreEqual("Baz", archive.Groups[0].Entries[2].Name);

				Assert.AreEqual(3, archive.Groups[0].Entries[0].Stream.Length);
				Assert.AreEqual(3, archive.Groups[0].Entries[0].Stream.Length);
				Assert.AreEqual(3, archive.Groups[0].Entries[0].Stream.Length);

				var fooMemoryStream = new MemoryStream();
				var barMemoryStream = new MemoryStream();
				var bazMemoryStream = new MemoryStream();

				archive.Groups[0].Entries[0].Stream.CopyTo(fooMemoryStream);
				archive.Groups[0].Entries[1].Stream.CopyTo(barMemoryStream);
				archive.Groups[0].Entries[2].Stream.CopyTo(bazMemoryStream);

				Assert.IsTrue(fooMemoryStream.ToArray().SequenceEqual(new byte[] { 27, 95, 17 }));
				Assert.IsTrue(barMemoryStream.ToArray().SequenceEqual(new byte[] { 69, 10, 45, 91, 28, 3, }));
				Assert.IsTrue(bazMemoryStream.ToArray().SequenceEqual(new byte[] { 97, 57, 81, 8, 12, 63, 91, 76, 29 }));
			}
		}

		[TestMethod]
		public void TestReadSingleGroupSingleEntry()
		{
			var wix = string.Join(Environment.NewLine,
				"!group Alpha",
				"File 0 128");

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var archive = ArchiveReader.Read(wadStream, wixStream);

				Assert.AreEqual(1, archive.Groups.Count);
				Assert.AreEqual(1, archive.Groups[0].Entries.Count);

				Assert.AreEqual("Alpha", archive.Groups[0].Name);
				Assert.AreEqual("File", archive.Groups[0].Entries[0].Name);
			}
		}

		[TestMethod]
		public void TestReadMultipleGroupsSingleEntries()
		{
			var wix = string.Join(Environment.NewLine,
				"!group Alpha",
				"Foo 0 128",
				"!group Beta",
				"Bar 128 128",
				"!group Gamma",
				"Baz 256 128");

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var archive = ArchiveReader.Read(wadStream, wixStream);

				Assert.AreEqual(3, archive.Groups.Count);

				Assert.AreEqual(1, archive.Groups[0].Entries.Count);
				Assert.AreEqual(1, archive.Groups[1].Entries.Count);
				Assert.AreEqual(1, archive.Groups[2].Entries.Count);

				Assert.AreEqual("Alpha", archive.Groups[0].Name);
				Assert.AreEqual("Foo", archive.Groups[0].Entries[0].Name);

				Assert.AreEqual("Beta", archive.Groups[1].Name);
				Assert.AreEqual("Bar", archive.Groups[1].Entries[0].Name);

				Assert.AreEqual("Gamma", archive.Groups[2].Name);
				Assert.AreEqual("Baz", archive.Groups[2].Entries[0].Name);
			}
		}

		[TestMethod]
		public void TestReadMultipleGroupsMultipleEntries()
		{
			var wix = string.Join(Environment.NewLine,
				"!group Alpha",
				"Foo1 0 128",
				"Foo2 128 128",
				"Foo3 128 128",
				"!group Beta",
				"Bar1 0 128",
				"Bar2 128 128",
				"Bar3 256 128",
				"!group Gamma",
				"Baz1 0 128",
				"Baz2 128 128",
				"Baz3 256 128");

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var archive = ArchiveReader.Read(wadStream, wixStream);

				Assert.AreEqual(3, archive.Groups.Count);

				Assert.AreEqual(3, archive.Groups[0].Entries.Count);
				Assert.AreEqual(3, archive.Groups[1].Entries.Count);
				Assert.AreEqual(3, archive.Groups[2].Entries.Count);

				Assert.AreEqual("Alpha", archive.Groups[0].Name);
				Assert.AreEqual("Foo1", archive.Groups[0].Entries[0].Name);
				Assert.AreEqual("Foo2", archive.Groups[0].Entries[1].Name);
				Assert.AreEqual("Foo3", archive.Groups[0].Entries[2].Name);

				Assert.AreEqual("Beta", archive.Groups[1].Name);
				Assert.AreEqual("Bar1", archive.Groups[1].Entries[0].Name);
				Assert.AreEqual("Bar2", archive.Groups[1].Entries[1].Name);
				Assert.AreEqual("Bar3", archive.Groups[1].Entries[2].Name);

				Assert.AreEqual("Gamma", archive.Groups[2].Name);
				Assert.AreEqual("Baz1", archive.Groups[2].Entries[0].Name);
				Assert.AreEqual("Baz2", archive.Groups[2].Entries[1].Name);
				Assert.AreEqual("Baz3", archive.Groups[2].Entries[2].Name);
			}
		}

		[TestMethod]
		public void TestReadNoGroup()
		{
			var wix = string.Join(Environment.NewLine, "File 0 128");

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var exception = Assert.ThrowsException<ArchiveReaderException>(() => ArchiveReader.Read(wadStream, wixStream));

				Assert.AreEqual("Attempted to add an entry with no previously defined group.", exception.Message);
			}
		}

		[TestMethod]
		public void TestReadInvalidGroup()
		{
			var wix = string.Join(Environment.NewLine, "!group");

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var exception = Assert.ThrowsException<ArchiveReaderException>(() => ArchiveReader.Read(wadStream, wixStream));

				Assert.AreEqual("Failed to parse group line due to insufficient tokens.", exception.Message);
			}
		}

		[TestMethod]
		public void TestReadInvalidEntry()
		{
			var wix = string.Join(Environment.NewLine, "!group Test", "File");

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var exception = Assert.ThrowsException<ArchiveReaderException>(() => ArchiveReader.Read(wadStream, wixStream));

				Assert.AreEqual("Failed to parse entry line due to insufficient tokens.", exception.Message);
			}
		}

		[TestMethod]
		public void TestReadNonNumericEntryOffset()
		{
			var wix = string.Join(Environment.NewLine, "!group Test", "File A 128");

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var exception = Assert.ThrowsException<ArchiveReaderException>(() => ArchiveReader.Read(wadStream, wixStream));

				Assert.AreEqual("Failed to parse entry offset.", exception.Message);
			}
		}

		[TestMethod]
		public void TestReadNonNumericEntryLength()
		{
			var wix = string.Join(Environment.NewLine, "!group Test", "File 128 A");

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream(Encoding.UTF8.GetBytes(wix)))
			{
				var exception = Assert.ThrowsException<ArchiveReaderException>(() => ArchiveReader.Read(wadStream, wixStream));

				Assert.AreEqual("Failed to parse entry length.", exception.Message);
			}
		}
	}
}
