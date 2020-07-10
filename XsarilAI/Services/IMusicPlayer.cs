using Discord;

namespace XsarilAI.Services {
	public interface IMusicPlayer {

		void Play(IAudioChannel channel, IMessageChannel outputChannel, ulong guildId, string musicSource);

		void Stop(IMessageChannel outputChannel, ulong guildId);

	}
}
