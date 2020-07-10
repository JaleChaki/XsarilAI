using Discord;
using System;
using XsarilAI.Events;
using XsarilAI.Services;

namespace XsarilAI.Handlers {
	public class StopMusicCommandHandler : CommandMessageHandler {

		public StopMusicCommandHandler() : base("stop") {

		}

		public override void Execute(IExecutionEnvironment environment, MessageReceivedEvent e, string commandArgs) {
			IMusicPlayer musicPlayer = environment.GetService<IMusicPlayer>();
			//EmbedBuilder builder = new EmbedBuilder();
			try {
				musicPlayer.Stop(e.Channel, e.GuildId);
				/*builder.WithTitle("Stopped");
				builder.WithColor(Color.LighterGrey);*/
				//e.Channel.SendMessageAsync(null, false, builder.Build()).RunSync();
				e.Message.AddReactionAsync(new Emoji("\u23F9\uFE0F")).RunSync();
			}
			catch (ArgumentException) {
				/*builder.WithTitle("Но ведь я ничего и не играл");
				builder.WithColor(Color.Red);
				e.Channel.SendMessageAsync(null, false, builder.Build()).RunSync();*/
				e.Channel.SendMessageAsync("Но ведь я ничего и не играл").RunSync();
			}
		}

	}
}
