using Discord;
using Discord.WebSocket;

namespace XsarilAI.Behaviors {
	public class RoleSetBehavior : IBehavior {

		private readonly SocketGuildUser GuildUser;

		private readonly IRole Role;

		public RoleSetBehavior(SocketGuildUser guildUser, IRole role) {
			this.GuildUser = guildUser;
			this.Role = role;
		}

		public void Apply(DiscordSocketClient discordClient) {
			GuildUser.AddRoleAsync(Role);
		}
	}
}
