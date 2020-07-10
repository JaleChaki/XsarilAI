using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;
using Discord.WebSocket;
using XLogger;
using XLogger.Configuration;
using XLogger.Formatters;
using XsarilAI.Configuration.Reader;

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
			LoggerConfiguration.ConfigureLoggerConfiguration(builder =>
				builder.UseConsoleLogging()
					.AddFormatter(new SocketMessageFormatter())
					.AddFormatter(new LogMessageFormatter())
					.UseLogLevel(LogLevel.Debug)
			);
			IConfigurationReaderProvider readerProvider = new DefaultConfigurationReaderProvider();
			DiscordBot bot = new DiscordBot(readerProvider.GetReader("AppConfig.json").Read("AppConfig.json"));
			/*bot.Handlers.Add(new LoggerHandler());
			bot.Handlers.Add(new MessageReceiveHandler(bot));
			bot.Handlers.Add(new EmojiAddHandler(bot, "welcome", "l_hand", "CS:GOсть"));
			bot.Handlers.Add(new SubscribeCommandHandler(bot, ".sub мемы", "voice-flood", "memaseks"));
			bot.Handlers.Add(new UnsubscribeCommandHandler(bot, ".unsub мемы", "voice-flood", "memaseks"));
			bot.Handlers.Add(new PlayMusicHandler(bot, ".play"));*/
			bot.Run().GetAwaiter().GetResult();
		}
	}
}
