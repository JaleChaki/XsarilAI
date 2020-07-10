using Discord;
using XsarilAI.Guilds;

namespace XsarilAI {
	public interface IExecutionEnvironment {

		void SendErrorMessage(string messageText, ulong guildId);

		GuildSettings GetGuildSettings(ulong guildId);

		void SaveGuildSettings(GuildSettings info);

		T GetService<T>();

	}
}
