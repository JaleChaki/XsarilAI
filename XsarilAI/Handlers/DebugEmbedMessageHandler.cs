using Discord;
using XsarilAI.Events;

namespace XsarilAI.Handlers {
	public class DebugEmbedMessageHandler : CommandMessageHandler {

		public DebugEmbedMessageHandler() : base("debug-embed") {

		}

		public override void Execute(IExecutionEnvironment environment, MessageReceivedEvent e, string commandArgs) {
			EmbedBuilder builder = new EmbedBuilder();
			builder.AddField("Field name", "__value__");
			builder.AddField("Field 2 name", "__val2");
			builder.AddField("inline field", "inval", true);
			builder.WithColor(Color.Green);

			e.Channel.SendMessageAsync(null, false, builder.Build());
		}
	}
}
