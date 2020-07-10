using Discord;
using Discord.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using XLogger;
using XsarilAI.Configuration;

namespace XsarilAI.Services {
	public class DefaultMusicPlayer : IMusicPlayer {

		private readonly IDictionary<ulong, CancellationTokenSource> Tokens;

		private readonly IConfiguration Configuration;

		public DefaultMusicPlayer(IConfiguration musicPlayerConfiguration) {
			this.Configuration = musicPlayerConfiguration;
			this.Tokens = new Dictionary<ulong, CancellationTokenSource>();
		}

		private CancellationTokenSource GetOrCreateCancellationTokenSource(ulong guildId) {
			if (!Tokens.ContainsKey(guildId)) {
				CancellationTokenSource src = new CancellationTokenSource();
				Tokens.Add(guildId, src);
			}
			return Tokens[guildId];
		}

		public void Play(IAudioChannel channel, IMessageChannel outputChannel, ulong guildId, string filename) {

			CancellationTokenSource src = GetOrCreateCancellationTokenSource(guildId);
			TaskFactory factory = new TaskFactory(src.Token);
			factory.StartNew(() => PlayMusicAsync(channel, outputChannel, filename, src.Token));
			

		}

		public void Stop(IMessageChannel outputChannel, ulong guildId) {
			if (!Tokens.ContainsKey(guildId)) {
				throw new ArgumentException();
			}
			CancellationTokenSource source = GetOrCreateCancellationTokenSource(guildId);
			source.Cancel();
			Tokens.Remove(guildId);
			source.Dispose();
		}

		private Process CreateConverterProc(string filename) {
			return Process.Start(new ProcessStartInfo {
				FileName = Configuration["ffmpeg.path"],
				Arguments = string.Format(Configuration["ffmpeg.args"], filename),
				UseShellExecute = false,
				RedirectStandardOutput = true
			});
		}

		private async Task PlayMusicAsync(IAudioChannel channel, IMessageChannel outputChannel, string filename, CancellationToken cancellationToken) {
			IAudioClient audioClient = null;
			
			try {
				audioClient = await channel.ConnectAsync();
				using (var ffmpeg = CreateConverterProc(filename))
				using (var output = ffmpeg.StandardOutput.BaseStream)
				using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed)) {
					try {
						EmbedBuilder builder = new EmbedBuilder();
						builder.AddField("Now playing", Path.GetFileNameWithoutExtension(filename));
						builder.WithColor(Color.Green);
						await outputChannel.SendMessageAsync(null, false, builder.Build());
						await output.CopyToAsync(discord, cancellationToken);
					}
					catch (Exception e) {
						Logger.Error(e);
					}
					finally {
						await discord.FlushAsync();
						await channel.DisconnectAsync();
					}
				}
			}
			catch (Exception e) {
				Logger.Error(e);
			}
			finally {
				audioClient?.Dispose();
			}
		}
	}
}
