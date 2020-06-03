using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XLogger;
using XsarilAI.Behaviors;

namespace XsarilAI.Handlers {
	public class EmojiAddHandler : IHandler {

		private readonly IBehaviorContainer Cache;

		private readonly string ChannelName;

		private readonly string EmojiName;

		private readonly string RoleName;

		public EmojiAddHandler(IBehaviorContainer cache, string channelName, string emoji, string roleName) {
			this.Cache = cache;
			this.ChannelName = channelName;
			this.EmojiName = emoji;
			this.RoleName = roleName;
		}

		public void SubscribeActions(DiscordSocketClient discordClient) {
			discordClient.ReactionAdded += ReactionAdded;
		}

		private Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction) {
			if (ChannelName != channel.Name || reaction.Emote.Name != EmojiName) {
				Logger.Debug($"incompatible conditions {channel.Name} ; {reaction.Emote.Name}");
				return Task.CompletedTask;
			}
			Logger.Debug($"accept conditions");
			SocketGuild guild = (channel as SocketGuildChannel).Guild;
			Cache.Add(new RoleSetBehavior(guild.GetUser(reaction.UserId), guild.Roles.Where(role => role.Name == RoleName).FirstOrDefault()));
			return Task.CompletedTask;
		}
	}
}
