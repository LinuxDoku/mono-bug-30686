using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace monobug30686
{
	class MyFakeStream : FileStream 
	{
		public MyFakeStream (string path, FileMode mode) : base(path, mode) {}

		/// <summary>
		/// Simulate "CanSeek" is false, which is the case when you are retreiving data from web.
		/// </summary>
		public override bool CanSeek => false;
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			var stream = new MyFakeStream("mono-bug-30686.zip", FileMode.Open);

			var unzipped = new MemoryStream ();
			var writer = new StreamWriter (unzipped);

			using (var archive = new ZipArchive (stream, ZipArchiveMode.Read)) {
				var entry = archive.Entries.First (x => x.Name == "Test.txt");
				writer.Write (new StreamReader(entry.Open()).ReadToEnd());
				writer.Flush();
			}

			Console.WriteLine (Encoding.ASCII.GetString(unzipped.ToArray()));
			Console.ReadLine();
		}
	}
}
