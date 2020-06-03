using Discord.WebSocket;

namespace XsarilAI.Handlers {
	public interface IHandler {

		void SubscribeActions(DiscordSocketClient discordClient);

	}
}
