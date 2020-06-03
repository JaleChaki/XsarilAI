using Discord.WebSocket;

namespace XsarilAI.Behaviors {
	public interface IBehavior {

		void Apply(DiscordSocketClient discordClient);

	}
}
