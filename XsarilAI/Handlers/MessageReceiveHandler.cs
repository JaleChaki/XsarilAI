using Discord.WebSocket;
using System.Threading.Tasks;
using XsarilAI.Behaviors;

namespace XsarilAI.Handlers {
	public class MessageReceiveHandler : IHandler {

		private readonly IBehaviorContainer Cache;

		public MessageReceiveHandler(IBehaviorContainer cache) {
			this.Cache = cache;
		}

		public void SubscribeActions(DiscordSocketClient discordClient) {
			discordClient.MessageReceived += MessageReceivedHandler;
		}

		private Task MessageReceivedHandler(SocketMessage message) {
			if (message.Content != "!create welcome message") {
				return Task.CompletedTask;
			}
			Cache.Add(new SendMessageBehavior(message.Channel, "Добро пожаловать на Jostkiy Servak.\nЕсли вы сюда пришли играть в CS, то ткните по руке - эмодзи ниже.\nЕсли вы пришли за другим, то напишите одному из одменов, он Вас авторизует.\n", "jopa.png"));
			return Task.CompletedTask;
		}
	}
}
