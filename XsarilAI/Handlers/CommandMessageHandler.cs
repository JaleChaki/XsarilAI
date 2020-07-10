using XsarilAI.Events;
using XsarilAI.Guilds;

namespace XsarilAI.Handlers {
	public abstract class CommandMessageHandler : DerivedEventHandler<MessageReceivedEvent> {

		protected readonly string CommandName;

		public CommandMessageHandler(string commandName) {
			this.CommandName = commandName;
		}

		public sealed override void Handle(IExecutionEnvironment environment, MessageReceivedEvent e) {
			GuildSettings guildSettings = environment.GetGuildSettings(e.GuildId);
			string fullCommand = guildSettings.CommandPrefix + CommandName;
			string messageText = e.Text;
			if (messageText == fullCommand || messageText.StartsWith(fullCommand + " ")) {
				int cutLength = fullCommand.Length;
				if (messageText.Length > cutLength && messageText[cutLength + 1] == ' ') {
					++cutLength;
				}
				Execute(environment, e, messageText.Substring(cutLength));
			}
		}

		public abstract void Execute(IExecutionEnvironment environment, MessageReceivedEvent e, string commandArgs);
	}
}
