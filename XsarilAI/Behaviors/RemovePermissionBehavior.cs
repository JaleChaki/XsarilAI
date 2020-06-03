using Discord;
using Discord.WebSocket;

namespace XsarilAI.Behaviors {
	public class RemovePermissionBehavior : IBehavior {


		private readonly IGuildChannel Channel;

		private readonly IGuildUser User;

		public RemovePermissionBehavior(IGuildChannel channel, IGuildUser user) {
			this.Channel = channel;
			this.User = user;
		}

		public void Apply(DiscordSocketClient discordClient) {
			Channel.RemovePermissionOverwriteAsync(User);
		}
	}
}
