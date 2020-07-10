using Discord;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XLogger;
using XsarilAI.Configuration;
using XsarilAI.Events;
using XsarilAI.Guilds;
using XsarilAI.Handlers;

namespace XsarilAI {
	public class DiscordBot : IExecutionEnvironment {

		private readonly DiscordSocketClient Client;

#pragma warning disable IDE0044
		private object Sync = new object();
#pragma warning restore IDE0044

		private readonly IDictionary<ulong, GuildSettings> Guilds;

		private readonly ICollection<object> Services;

		private readonly ICollection<IEvent> CachedEvents;

		private readonly IConfiguration Configuration;

		public DiscordBot(IConfiguration configuration, IEnumerable<GuildSettings> guilds, IEnumerable<object> services) {
			this.Configuration = configuration;
			Client = new DiscordSocketClient();
			Guilds = guilds.ToDictionary(x => x.GuildId, y => y);
			Services = services.ToList();
			CachedEvents = new List<IEvent>();
		}

		public async Task Run() {
			await Client.LoginAsync(TokenType.Bot, Configuration["bot.token"]);
			await Client.StartAsync();
			Client.MessageReceived += OnMessageReceive;
			Client.ReactionAdded += OnReactionAdd;
			int loopTime = int.Parse(Configuration["bot.looptime"]);
			while (true) {
				HandleEvents();
				Thread.Sleep(loopTime);
			}
		}

		private void HandleEvents() {
			lock (Sync) {
				foreach (IEvent e in CachedEvents) {
					GuildSettings settings = GetGuildSettings(e.GuildId);
					if (settings is null) {
						continue;
					}
					foreach (IEventHandler handler in settings.Handlers) {
						if (handler.CanHandle(e)) {
							handler.Handle(this, e);
						}
					}
				}
				CachedEvents.Clear();
			}
		}

		private Task OnReactionAdd(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel messageChannel, SocketReaction reaction) {
			if (!(messageChannel is IGuildChannel)) {
				return Task.CompletedTask;
			}
			lock (Sync) {
				CachedEvents.Add(new EmojiAddedEvent(reaction));
			}
			return Task.CompletedTask;
		}

		private Task OnMessageReceive(SocketMessage message) {
			Logger.Info(message);
			if (!(message.Channel is IGuildChannel)) {
				return Task.CompletedTask;
			}
			lock (Sync) {
				CachedEvents.Add(new MessageReceivedEvent(message));
			}
			return Task.CompletedTask;
		}

		public void SendErrorMessage(string messageText, ulong guildId) {
			
		}

		public GuildSettings GetGuildSettings(ulong guildId) {
			return Guilds.ContainsKey(guildId) ? Guilds[guildId] : null;
		}

		public void SaveGuildSettings(GuildSettings info) {
			
		}

		public T GetService<T>() {
			foreach (object service in Services) {
				if (service is T castResult) {
					return castResult;
				}
			}
			return default;
		}
	}
}
