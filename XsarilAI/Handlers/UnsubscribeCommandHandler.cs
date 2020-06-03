using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using XsarilAI.Behaviors;

namespace XsarilAI.Handlers {
	public class UnsubscribeCommandHandler : IHandler {

		private readonly IBehaviorContainer Cache;

		private readonly string CommandPrefix;

		private readonly string ReceiveChannelName;

		private readonly string TargetChannelName;

		public UnsubscribeCommandHandler(IBehaviorContainer cache, string commandPrefix, string receiveChannelName, string targetChannelName) {
			this.Cache = cache;
			this.CommandPrefix = commandPrefix;
			this.ReceiveChannelName = receiveChannelName;
			this.TargetChannelName = targetChannelName;
		}

		public void SubscribeActions(DiscordSocketClient discordClient) {
			discordClient.MessageReceived += MessageReceiveAsync;
		}

		private Task MessageReceiveAsync(SocketMessage message) {
			if (!message.Content.ToLower().StartsWith(CommandPrefix.ToLower()) || message.Channel.Name != ReceiveChannelName || !(message.Channel is SocketGuildChannel)) {
				return Task.CompletedTask;
			}
			SocketGuild guild = (message.Channel as SocketGuildChannel).Guild;
			Cache.Add(new RemovePermissionBehavior(guild.Channels.Where(channel => channel.Name == TargetChannelName).FirstOrDefault(), message.Author as SocketGuildUser));
			return Task.CompletedTask;
		}

	}
}
