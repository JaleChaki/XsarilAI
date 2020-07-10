using System;
using System.IO;

namespace XsarilAI.Configuration.Reader {
	public class DefaultConfigurationReaderProvider : IConfigurationReaderProvider {

		private static readonly IConfigurationReader Instance = new JsonConfigurationReader();

		public IConfigurationReader GetReader(string filename) {
			if (Path.GetExtension(filename).ToLower() != ".json") {
				throw new ArgumentException("unsupported filename format", nameof(filename));
			}
			return Instance;
		}

	}
}
