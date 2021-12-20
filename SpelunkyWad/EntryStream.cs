using System;
using System.IO;

namespace SpelunkyWad
{
	internal sealed class EntryStream : Stream
	{
		public EntryStream(Stream stream)
		{
			this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
		}

		public override bool CanWrite => false;

		public override bool CanRead => this.stream.CanRead;
		public override bool CanSeek => this.stream.CanSeek;

		public override long Length => this.stream.Length;

		public override long Position
		{
			get => this.stream.Position;
			set => this.stream.Position = value;
		}

		public override long Seek(long offset, SeekOrigin origin) => this.stream.Seek(offset, origin);
		public override int Read(byte[] buffer, int offset, int count) => this.stream.Read(buffer, offset, count);
		public override void Flush() => this.stream.Flush();

		public override void SetLength(long value) => throw new NotSupportedException("Attempted to set the length of a read-only entry stream.");
		public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException("Attempted to write to a read-only entry stream.");

		private readonly Stream stream = null;
	}
}
