using Discord;
using System;
using System.IO;
using System.Linq;
using XsarilAI.Events;
using XsarilAI.Services;

namespace XsarilAI.Handlers {
	public class PlayRandomMusicCommandHandler : CommandMessageHandler {

		private static readonly Random Random = new Random();

		public PlayRandomMusicCommandHandler() : base("playrandom") {

		}

		public override void Execute(IExecutionEnvironment environment, MessageReceivedEvent e, string commandArgs) {
			if (e.Instigator.VoiceChannel is null) {
				e.Channel.SendMessageAsync("Вам требуется зайти в какой-либо голосовой канал").RunSync();
				return;
			}
			IMusicSearchService musicSearchService = environment.GetService<IMusicSearchService>();
			IMusicPlayer musicPlayer = environment.GetService<IMusicPlayer>();
			string[] results = musicSearchService.GetOrCreateMusicSources(commandArgs).ToArray();
			if (results.Length == 0) {
				e.Channel.SendMessageAsync("Не найдено ни одного результата по данному запросу").RunSync();
				return;
			}
			string source = results[Random.Next(results.Length)];
			musicPlayer.Play(e.Instigator.VoiceChannel, e.Channel, e.GuildId, source);
			EmbedBuilder builder = new EmbedBuilder();
			builder.AddField(Path.GetFileNameWithoutExtension(source), $"Now playing (selected from {results.Length} sources)");
			builder.WithColor(Color.Green);
			e.Channel.SendMessageAsync(null, false, builder.Build()).RunSync();
		}
	}
}
