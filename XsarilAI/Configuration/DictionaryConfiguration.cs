using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace XsarilAI.Configuration {
	public class DictionaryConfiguration : IConfiguration {
		
		[JsonIgnore]
		public string this[string property] => Properties.ContainsKey(property) ? Properties[property] : null;

#pragma warning disable IDE0044
		[JsonProperty]
		private Dictionary<string, string> Properties;
#pragma warning restore IDE0044

		public DictionaryConfiguration(IDictionary<string, string> properties) {
			Properties = properties.ToDictionary(x => x.Key, x => x.Value);
		}

	}
}
