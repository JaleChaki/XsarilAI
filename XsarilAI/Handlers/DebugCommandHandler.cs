using System.Text;
using XsarilAI.Events;

namespace XsarilAI.Handlers {
	public class DebugCommandHandler : CommandMessageHandler {

		public DebugCommandHandler() : base("debug") {

		}

		public override void Execute(IExecutionEnvironment environment, MessageReceivedEvent e, string commandArgs) {
			StringBuilder message = new StringBuilder("Упомянутые каналы\n");
			foreach (var channel in e.Message.MentionedChannels) {
				message
					.Append("Название: " + channel.Name + "\n")
					.Append("ID: " + channel.Id + "\n")
					.Append("Создан: " + channel.CreatedAt.ToString() + "\n");
			}
			e.Channel.SendMessageAsync(message.ToString()).GetAwaiter().GetResult();
			message = new StringBuilder("Упомянутые роли\n");
			foreach (var role in e.Message.MentionedRoles) {
				message
					.Append("Название: " + role.Name + "\n")
					.Append("ID: " + role.Id + "\n")
					.Append("Возможность пинговать: " + role.IsMentionable + "\n")
					.Append("Цвет: " + role.Color.RawValue + "\n");
			}
			e.Channel.SendMessageAsync(message.ToString()).GetAwaiter().GetResult();
			message = new StringBuilder("Упомянутые пользователи\n");
			foreach (var user in e.Message.MentionedUsers) {
				message
					.Append("Имя: " + user.Username + "\n")
					.Append("ID: " + user.Id + "\n")
					.Append("Статус: " + user.Status);
			}
			e.Channel.SendMessageAsync(message.ToString()).GetAwaiter().GetResult();

		}
	}
}
