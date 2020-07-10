using Discord;
using System.Collections.Generic;
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
			IEnumerable<string> files = musicSearchService.GetOrCreateMusicFiles(commandArgs);
			if (files.Count() == 0) {
				e.Channel.SendMessageAsync("Не найдено ни одного результата по данному запросу").RunSync();
				return;
			}
			if (files.Count() > 1) {
				EmbedBuilder builder = new EmbedBuilder();
				builder.WithTitle("Несколько результатов найдено");
				builder.WithDescription("Попробуйте указать более точное название");
				int counter = 1;
				foreach (var file in files) {
					builder.AddField(counter.ToString() + ".", System.IO.Path.GetFileNameWithoutExtension(file));
					++counter;
				}
				builder.WithColor(Color.LightGrey);
				e.Channel.SendMessageAsync(null, false, builder.Build()).RunSync();
				return;
			}
			if (e.Instigator.VoiceChannel is null) {
				e.Channel.SendMessageAsync("Вам требуется зайти в какой-либо голосовой канал").RunSync();
				return;
			}
			musicPlayer.Play(e.Instigator.VoiceChannel, e.Channel, e.GuildId, musicSearchService.GetOrCreateMusicFiles(commandArgs).First());
		}
	}
}
