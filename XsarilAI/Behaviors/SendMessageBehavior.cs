using Discord;
using Discord.WebSocket;

namespace XsarilAI.Behaviors {
	public class SendMessageBehavior : IBehavior {

		private readonly IMessageChannel Channel;

		private readonly string MessageText;

		private readonly string Attachment;

		public SendMessageBehavior(IMessageChannel channel, string messageText, string attachment = null) {
			this.Channel = channel;
			this.MessageText = messageText;
			this.Attachment = attachment;
		}

		public void Apply(DiscordSocketClient discordClient) {
			if (string.IsNullOrEmpty(Attachment)) {
				Channel.SendMessageAsync(MessageText);
			} else {
				Channel.SendFileAsync(Attachment, MessageText);
			}
		}
	}
}
