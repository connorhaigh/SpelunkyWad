using System;
using System.IO;
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

				wadStream.Seek(0L, SeekOrigin.Begin);
				wixStream.Seek(0L, SeekOrigin.Begin);

				using (var wixStreamReader = new StreamReader(wixStream))
				{
					var wix = wixStreamReader.ReadToEnd();

					Assert.AreEqual(
						string.Join(Environment.NewLine,
							"!group Test",
							"Foo 0 3",
							"Bar 3 6",
							"Baz 9 9",
							string.Empty),
						wix);
				}
			}
		}
	}
}
