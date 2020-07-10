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
			musicPlayer.Play(e.Instigator.VoiceChannel, e.Channel, e.GuildId, musicSearchService.GetOrCreateMusicFiles(commandArgs).First());
		}
	}
}
