using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XLogger;

namespace XsarilAI.Handlers {
	internal class LoggerHandler : IHandler {
		public void SubscribeActions(DiscordSocketClient discordClient) {
			discordClient.Log += LogMessageAsync;
		}

		private Task LogMessageAsync(Discord.LogMessage arg) {
			Logger.Info(arg.ToString());
			return Task.CompletedTask;
		}
	}
}
