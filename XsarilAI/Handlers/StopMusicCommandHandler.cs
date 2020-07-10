using XsarilAI.Events;
using XsarilAI.Services;

namespace XsarilAI.Handlers {
	public class StopMusicCommandHandler : CommandMessageHandler {

		public StopMusicCommandHandler() : base("stop") {

		}

		public override void Execute(IExecutionEnvironment environment, MessageReceivedEvent e, string commandArgs) {
			IMusicPlayer musicPlayer = environment.GetService<IMusicPlayer>();
			musicPlayer.Stop(e.Channel, e.GuildId);
		}

	}
}
