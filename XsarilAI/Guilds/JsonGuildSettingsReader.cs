using Newtonsoft.Json;
using System.IO;
using System.Linq;
using XsarilAI.Handlers;

namespace XsarilAI.Guilds {
	public class JsonGuildSettingsReader : IGuildSettingsReader {

		protected class JsonGuildSettings {

			public ulong GuildId { get; set; }

			public char CommandPrefix { get; set; }

			public ulong WelcomeChannelId { get; set; }

			public ulong RoleGainChannelId { get; set; }

			public string[] Services { get; set; }

			public GuildSettings Transform(IHandlerFactory handlerFactory) {
				return new GuildSettings() {
					GuildId = this.GuildId,
					CommandPrefix = this.CommandPrefix,
					WelcomeChannelId = this.WelcomeChannelId,
					RoleGainChannelId = this.RoleGainChannelId,
					Handlers = Services.SelectMany(x => handlerFactory.GetHandlerCollection(x)).ToList()
				};
			}

		}

		protected readonly IHandlerFactory HandlerFactory;

		public JsonGuildSettingsReader(IHandlerFactory handlerFactory) {
			this.HandlerFactory = handlerFactory;
		}

		public GuildSettings Read(string filename) {
			return JsonConvert.DeserializeObject<JsonGuildSettings>(File.ReadAllText(filename)).Transform(HandlerFactory);
		}
	}
}
