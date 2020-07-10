using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace XsarilAI.Configuration.Reader {
	public class JsonConfigurationReader : IConfigurationReader {
		public IConfiguration Read(string filename) {
			return new DictionaryConfiguration(JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filename)));
		}
	}
}
