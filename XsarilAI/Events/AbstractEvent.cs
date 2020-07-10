using Discord;

namespace XsarilAI.Events {
	public abstract class AbstractEvent : IEvent {

		public IGuildUser Instigator { get; protected set; }

		public virtual ulong InstigatorId => Instigator.Id;

		public virtual IGuild Guild => Instigator.Guild;

		public virtual ulong GuildId => Instigator.GuildId;

		public AbstractEvent(IGuildUser guildUser) {
			this.Instigator = guildUser;
		}

	}
}
