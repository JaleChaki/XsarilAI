using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;
using Discord.WebSocket;
using Newtonsoft.Json;
using XLogger;
using XLogger.Configuration;
using XLogger.Formatters;
using XsarilAI.Configuration.Reader;
using XsarilAI.Guilds;
using XsarilAI.Handlers;
using XsarilAI.Services;

namespace XsarilAI {
	class Program {

		private class LogMessageFormatter : ObjectFormatter<Discord.LogMessage> {

			public override string Format(LogMessage log) {
				return log.Object.ToString();
			}
		}

		private class SocketMessageFormatter : ObjectFormatter<SocketMessage> {
			public override string Format(LogMessage log) {
				SocketMessage message = log.Object as SocketMessage;
				return "[" + message.Author.Username + "] " + message.Content;
			}
		}

		static void Main(string[] args) {
			
			const string configPath = "AppConfig.json";
			IConfigurationReaderProvider readerProvider = new DefaultConfigurationReaderProvider();
			XsarilAI.Configuration.IConfiguration config = readerProvider.GetReader(configPath).Read(configPath);



			LoggerConfiguration.ConfigureLoggerConfiguration(builder =>
				builder.UseConsoleLogging()
					.AddFormatter(new SocketMessageFormatter())
					.AddFormatter(new LogMessageFormatter())
					.UseLogLevel(Enum.Parse<LogLevel>(config["log.level"]))
			);

			IGuildSettingsReader reader = new JsonGuildSettingsReader(new DefaultHandlerFactory());
			ICollection<GuildSettings> guilds = new List<GuildSettings>();
			string[] settingsFiles = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(config["guilds.settings"]));
			foreach (string file in settingsFiles) {
				guilds.Add(reader.Read(file));
			}


			DiscordBot bot = new DiscordBot(config, guilds, new object[] { new DefaultMusicPlayer(config), new DefaultMusicSearchService(config) });

			bot.Run().RunSync();
		}
	}
}
