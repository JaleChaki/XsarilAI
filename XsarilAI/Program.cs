using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord.Audio;
using Discord.WebSocket;
using XLogger;
using XLogger.Configuration;
using XLogger.Formatters;
using XsarilAI.Handlers;

namespace XsarilAI {
	class Program {

		private class LogMessageFormatter : ObjectFormatter<Discord.LogMessage> {

			public override string Format(LogMessage log) {
				return log.Object.ToString();
			}
		}

		private DiscordSocketClient client;

		static void Main(string[] args) {
			LoggerConfiguration.ConfigureLoggerConfiguration(builder => 
				builder.UseConsoleLogging()
					.UseLogLevel(LogLevel.Debug)
			);
			DiscordBot bot = new DiscordBot();
			bot.Handlers.Add(new LoggerHandler());
			bot.Handlers.Add(new MessageReceiveHandler(bot));
			bot.Handlers.Add(new EmojiAddHandler(bot, "welcome", "l_hand", "CS:GOсть"));
			bot.Handlers.Add(new SubscribeCommandHandler(bot, "!sub мемы", "voice-flood", "memaseks"));
			bot.Handlers.Add(new UnsubscribeCommandHandler(bot, "!unsub мемы", "voice-flood", "memaseks"));
			bot.Handlers.Add(new PlayMusicHandler(bot, "!play"));
			bot.Run().GetAwaiter().GetResult();
		}

		public async Task MainAsync() {
			client = new DiscordSocketClient();
			Console.WriteLine("AAAA");
			client.Log += Client_Log;
			await client.LoginAsync(Discord.TokenType.Bot,
				"NjkzMTI3NDIwMTE0NjMyNzU0.Xn-LMg.7aau-CiWgjjOYffi9kmuPbmfMKY");
			await client.StartAsync();
			// Block this task until the program is closed.
			await Task.Delay(-1);
		}

		public async Task RunAsync() {

			XLogger.Configuration.LoggerConfiguration.ConfigureLoggerConfiguration(builder => {
				builder
					.UseDefaultConfiguration()
					.UseLogLevel(LogLevel.Debug)
					.AddFormatter(new LogMessageFormatter());
				//.UseConsoleLogging();
			});
			client = new DiscordSocketClient();
			//Logger.Debug("start auth");
			Console.WriteLine("start auth");
			await client.LoginAsync(Discord.TokenType.Bot, "NjkzMTI3NDIwMTE0NjMyNzU0.Xn-LMg.7aau-CiWgjjOYffi9kmuPbmfMKY");
			client.Log += Client_Log;
			client.Ready += Client_Ready;
			client.MessageReceived += Client_MessageReceived;
			//RunAsync().GetAwaiter().GetResult();
			await client.StartAsync();
			Logger.Debug("completed");
			Thread.Sleep(3000);
			ulong g = 350208864525746186L;
			await HandleGuild(client.GetGuild(g));
			await Task.Delay(-1);
		}

		private async Task Client_MessageReceived(SocketMessage arg) {
			if (arg.Content != "music") {
				return;
			}
			if (arg.Channel is SocketGuildChannel guildChannel) {
				await HandleGuild(guildChannel.Guild);
			}
		}

		private async Task HandleGuild(SocketGuild guild) {
			var channel = guild.VoiceChannels.Last();
			Logger.Debug("attempt to join voice channel");
			IAudioClient audioClient = await channel.ConnectAsync();
			Logger.Debug("connection complete");
			using (var ffmpeg = Process.Start(new ProcessStartInfo {
				FileName = "ffmpeg/bin/ffmpeg.exe",
				Arguments = $"-hide_banner -loglevel panic -i \"mogo.mp4\" -ac 2 -f s16le -ar 48000 pipe:1",
				UseShellExecute = false,
				RedirectStandardOutput = true
			}))
			using (var output = ffmpeg.StandardOutput.BaseStream)
			using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed)) {
				try {
					Logger.Debug("copy output stream");
					await output.CopyToAsync(discord);
				}
				catch (Exception e) {
					Logger.Error(e);
				}
				finally {
					Logger.Debug("finished");
					await discord.FlushAsync();
				}
			}
		}

		private Task Client_Ready() {
			Logger.Info($"Bot online!");
			return Task.CompletedTask;
		}

		private Task Client_Log(Discord.LogMessage arg) {
			if (arg.Exception is null) {
				Logger.Info(arg);
			} else {
				Logger.Error(arg.Exception);
			}
			return Task.CompletedTask;
		}
	}
}
