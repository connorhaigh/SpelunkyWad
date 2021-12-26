using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpelunkyWad.Tests
{
	[TestClass]
	public sealed class ArchiveWriterTests
	{
		[TestMethod]
		public void TestWrite()
		{
			var fooMemoryStream = new MemoryStream();
			var barMemoryStream = new MemoryStream();
			var bazMemoryStream = new MemoryStream();

			fooMemoryStream.Write(new byte[] { 27, 95, 17 });
			barMemoryStream.Write(new byte[] { 69, 10, 45, 91, 28, 3 });
			bazMemoryStream.Write(new byte[] { 97, 57, 81, 8, 12, 63, 91, 76, 29 });

			var archive = new Archive();
			archive.Groups.Add(new Group("Test"));
			archive.Groups[0].Entries.Add(new Entry("Foo", fooMemoryStream));
			archive.Groups[0].Entries.Add(new Entry("Bar", barMemoryStream));
			archive.Groups[0].Entries.Add(new Entry("Baz", bazMemoryStream));

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream())
			{
				ArchiveWriter.Write(archive, wadStream, wixStream);

				Assert.IsTrue(wadStream.ToArray().SequenceEqual(new byte[] { 27, 95, 17, 69, 10, 45, 91, 28, 3, 97, 57, 81, 8, 12, 63, 91, 76, 29 }));
				Assert.AreEqual(string.Join(Environment.NewLine, "!group Test", "Foo 0 3", "Bar 3 6", "Baz 9 9", string.Empty), Encoding.UTF8.GetString(wixStream.ToArray()));
			}
		}

		[TestMethod]
		public void TestWriteSingleGroupSingleEntry()
		{
			var archive = new Archive();
			archive.Groups.Add(new Group("Alpha"));
			archive.Groups[0].Entries.Add(new Entry("File", new MemoryStream(new byte[] { 1, 2, 3 })));

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream())
			{
				ArchiveWriter.Write(archive, wadStream, wixStream);

				Assert.AreEqual(string.Join(Environment.NewLine, "!group Alpha", "File 0 3", string.Empty), Encoding.UTF8.GetString(wixStream.ToArray()));
			}
		}

		[TestMethod]
		public void TestWriteMultipleGroupsSingleEntries()
		{
			var archive = new Archive();
			archive.Groups.Add(new Group("Alpha"));
			archive.Groups.Add(new Group("Beta"));
			archive.Groups.Add(new Group("Gamma"));
			archive.Groups[0].Entries.Add(new Entry("Foo", new MemoryStream(new byte[] { 1, 2, 3 })));
			archive.Groups[1].Entries.Add(new Entry("Bar", new MemoryStream(new byte[] { 4, 5, 6 })));
			archive.Groups[2].Entries.Add(new Entry("Baz", new MemoryStream(new byte[] { 7, 8, 9 })));

			using (var wadStream = new MemoryStream())
			using (var wixStream = new MemoryStream())
			{
				ArchiveWriter.Write(archive, wadStream, wixStream);

				wadStream.Seek(0L, SeekOrigin.Begin);
				wixStream.Seek(0L, SeekOrigin.Begin);

				Assert.AreEqual(string.Join(Environment.NewLine, "!group Alpha", "Foo 0 3", "!group Beta", "Bar 3 3", "!group Gamma", "Baz 6 3", string.Empty), Encoding.UTF8.GetString(wixStream.ToArray()));
			}
		}
	}
}
