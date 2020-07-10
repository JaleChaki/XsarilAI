using Discord;
using Discord.WebSocket;

namespace XsarilAI.Events {

	public class MessageReceivedEvent : AbstractEvent {

		public readonly SocketMessage Message;

		public virtual string Text => Message.Content;

		public virtual IMessageChannel Channel => Message.Channel;

		public MessageReceivedEvent(SocketMessage message) : base(message.Author as IGuildUser) {
			this.Message = message;
		}

	}
}
