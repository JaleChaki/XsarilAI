using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace XsarilAI.Behaviors {
	public class AddPermissionBehavior : IBehavior {

		private readonly IGuildChannel Channel;

		private readonly IGuildUser User;

		public AddPermissionBehavior(IGuildChannel channel, IGuildUser user) {
			this.Channel = channel;
			this.User = user;
		}

		public void Apply(DiscordSocketClient discordClient) {
			Channel.AddPermissionOverwriteAsync(User, new OverwritePermissions(readMessageHistory: PermValue.Allow, 
				speak: PermValue.Allow, 
				connect: PermValue.Allow, 
				useVoiceActivation: PermValue.Allow, 
				addReactions: PermValue.Allow, 
				sendMessages: PermValue.Allow, 
				embedLinks: PermValue.Allow, 
				attachFiles: PermValue.Allow));
		}



	}
}
