using Discord;
using System.Collections.Generic;
using XsarilAI.Events;

namespace XsarilAI.Handlers {
	public class RoleGainEventHandler : DerivedEventHandler<EmojiAddedEvent> {

		protected readonly IDictionary<string, ulong> EmojiDepends;

		public RoleGainEventHandler(IDictionary<string, ulong> emojiDepends) {
			this.EmojiDepends = emojiDepends;
		}

		public override void Handle(IExecutionEnvironment environment, EmojiAddedEvent e) {
			if (e.Emoji.Channel.Id != environment.GetGuildSettings(e.GuildId).RoleGainChannelId) {
				return;
			}
			if (!EmojiDepends.ContainsKey(e.Emoji.Emote.Name)) {
				return;
			}
			IRole role = e.Guild.GetRole(EmojiDepends[e.Emoji.Emote.Name]);
			if (role is null) {
				return;
			}
			e.Instigator.AddRoleAsync(role).RunSync();
		}
	}
}
