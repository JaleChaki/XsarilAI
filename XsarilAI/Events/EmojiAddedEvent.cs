using Discord.WebSocket;

namespace XsarilAI.Events {
	public class EmojiAddedEvent : AbstractEvent {

		public readonly SocketReaction Emoji;

		public EmojiAddedEvent(SocketReaction emoji) : base((emoji.Channel as SocketGuildChannel).Guild.GetUser(emoji.User.Value.Id)) {
			this.Emoji = emoji;
		}

	}
}
