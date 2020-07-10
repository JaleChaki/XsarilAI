using Discord;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XsarilAI.Events;
using XsarilAI.Services;

namespace XsarilAI.Handlers {
	public class PlayMusicCommandHandler : CommandMessageHandler {

		public PlayMusicCommandHandler() : base("play") {

		}

		public override void Execute(IExecutionEnvironment environment, MessageReceivedEvent e, string commandArgs) {
			IMusicSearchService musicSearchService = environment.GetService<IMusicSearchService>();
			IMusicPlayer musicPlayer = environment.GetService<IMusicPlayer>();
			IEnumerable<string> files = musicSearchService.GetOrCreateMusicSources(commandArgs);
			if (files.Count() == 0) {
				e.Channel.SendMessageAsync("Не найдено ни одного результата по данному запросу").RunSync();
				return;
			}
			EmbedBuilder builder = null;
			if (files.Count() > 1) {
				builder = new EmbedBuilder();
				builder.WithTitle("Несколько результатов найдено");
				builder.WithDescription("Попробуйте указать более точное название");
				int counter = 1;
				int advancedResults = 0;
				foreach (var file in files) {
					if (counter < EmbedBuilder.MaxFieldCount - 1) {
						builder.AddField(counter.ToString() + ".", System.IO.Path.GetFileNameWithoutExtension(file));
					} else {
						++advancedResults;
					}
					++counter;
				}
				if (advancedResults > 0) {
					builder.AddField($"Ещё результаты ({advancedResults})", "...");
				}
				builder.WithColor(Color.LightGrey);
				e.Channel.SendMessageAsync(null, false, builder.Build()).RunSync();
				return;
			}
			if (e.Instigator.VoiceChannel is null) {
				e.Channel.SendMessageAsync("Вам требуется зайти в какой-либо голосовой канал").RunSync();
				return;
			}
			string source = musicSearchService.GetOrCreateMusicSources(commandArgs).First();
			musicPlayer.Play(e.Instigator.VoiceChannel, e.Channel, e.GuildId, source);
			builder = new EmbedBuilder();
			builder.AddField(Path.GetFileNameWithoutExtension(source), "Now playing");
			builder.WithColor(Color.Green);
			e.Channel.SendMessageAsync(null, false, builder.Build()).RunSync();
		}
	}
}
