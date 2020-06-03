using Discord.WebSocket;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XLogger;
using XsarilAI.Behaviors;

namespace XsarilAI.Handlers {
	public class PlayMusicHandler : IHandler {

		private readonly IBehaviorContainer Cache;

		private readonly string CommandPrefix;

		public PlayMusicHandler(IBehaviorContainer cache, string commandPrefix) {
			this.Cache = cache;
			this.CommandPrefix = commandPrefix;
		}

		public void SubscribeActions(DiscordSocketClient discordClient) {
			discordClient.MessageReceived += MessageReceiveAsync;
		}

		private Task MessageReceiveAsync(SocketMessage message) {
			if (!message.Content.ToLower().StartsWith(CommandPrefix.ToLower()) || !(message.Channel is SocketGuildChannel) || message.Content.Length < 10) {
				return Task.CompletedTask;
			}
			List<string> commandContent = message.Content.ToLower().Substring(CommandPrefix.Length).Split(' ').ToList();
			List<string> files = Directory.GetFiles(@"Music/").ToList();
			files.Sort((a, b) => {
				int aGood = 0;
				int bGood = 0;
				foreach (string content in commandContent) {
					if (a.ToLower().Contains(content)) {
						++aGood;
					}
					if (b.ToLower().Contains(content)) {
						++bGood;
					}
				}
				return aGood.CompareTo(bGood);
			});

			SocketGuild guild = (message.Channel as SocketGuildChannel).Guild;

			var voiceChannel = (message.Author as SocketGuildUser).VoiceChannel;
			if (voiceChannel != null) {
				Cache.Add(new PlayMusicBehavior(voiceChannel, files.Last()));
			} else {
				Logger.Debug("attempt to join empty channel");
			}
			return Task.CompletedTask;
		}
	}
}
