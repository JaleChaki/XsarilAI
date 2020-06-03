using Discord;
using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using XLogger;

namespace XsarilAI.Behaviors {
	public class PlayMusicBehavior : IBehavior {

		private readonly IVoiceChannel VoiceChannel;

		private readonly IMessageChannel Source;

		private readonly string FileName;

		public PlayMusicBehavior(IVoiceChannel voiceChannel, IMessageChannel source, string fileName) {
			this.FileName = fileName;
			this.VoiceChannel = voiceChannel;
		}

		public void Apply(DiscordSocketClient discordClient) {
			Thread thread = new Thread(new ThreadStart(PlayMusicAsync));
			thread.Start();
		}

		private Process CreateFFMpegProc() {
			return Process.Start(new ProcessStartInfo {
				FileName = "ffmpeg/bin/ffmpeg.exe",
				Arguments = $"-hide_banner -loglevel panic -i \"{FileName}\" -ac 2 -f s16le -ar 48000 pipe:1",
				UseShellExecute = false,
				RedirectStandardOutput = true
			});
		}

		private async void PlayMusicAsync() {
			try {
				Logger.Debug("target file music is " + FileName);
				IAudioClient audioClient = await VoiceChannel.ConnectAsync();
				using (var ffmpeg = CreateFFMpegProc())
				using (var output = ffmpeg.StandardOutput.BaseStream)
				using (var discord = audioClient.CreatePCMStream(AudioApplication.Mixed)) {
					try {
						Logger.Debug("copy output stream");
						await Source.SendMessageAsync("Play: " + Path.GetFileNameWithoutExtension(FileName));
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
			catch (Exception e) {
				Logger.Error(e);
				await Source.SendMessageAsync("Упс, что-то пошло не так. Попробуйте повторить попытку.");
			}
		}

	}
}
