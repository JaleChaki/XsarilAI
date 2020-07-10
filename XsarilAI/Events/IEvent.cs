using Discord;

namespace XsarilAI.Events {
	public interface IEvent {

		IGuildUser Instigator { get; }

		ulong InstigatorId { get; }

		IGuild Guild { get; }

		ulong GuildId { get; }

	}
}
