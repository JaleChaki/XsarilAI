using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using XLogger;
using XsarilAI.Behaviors;
using XsarilAI.Handlers;

namespace XsarilAI {
	public class DiscordBot : IBehaviorContainer {

		private readonly ICollection<IBehavior> CachedBehaviors;

		public ICollection<IHandler> Handlers { get; private set; }

		private readonly DiscordSocketClient Client;

		private readonly string Token;

		public void Add(IBehavior behavior) {
			lock (CachedBehaviors) {
				CachedBehaviors.Add(behavior);
			}
		}

		public DiscordBot() {
			CachedBehaviors = new List<IBehavior>();
			Handlers = new List<IHandler>();
			Client = new DiscordSocketClient();
			Token = "NjkzMTI3NDIwMTE0NjMyNzU0.Xn-LMg.7aau-CiWgjjOYffi9kmuPbmfMKY";
		}

		public async Task Run() {
			await Client.LoginAsync(Discord.TokenType.Bot, Token);
			await Client.StartAsync();
			foreach (IHandler handler in Handlers) {
				handler.SubscribeActions(Client);
			}

			while (true) {
				RunOnce();
				Thread.Sleep(1500);
			}
			//await Task.Delay(-1);
		}

		public void RunOnce() {
			lock (CachedBehaviors) {
				foreach (IBehavior behavior in CachedBehaviors) {
					try {
						behavior.Apply(Client);
					}
					catch (Exception e) {
						Logger.Error(e);
					}
				}
				CachedBehaviors.Clear();
			}
		}

	}
}
