using System.Collections.Generic;
using XsarilAI.Handlers;

namespace XsarilAI.Guilds {

	/// <summary>
	/// класс информации о сервере дискорда
	/// </summary>
	public class GuildSettings {

		/// <summary>
		/// Id сервера
		/// </summary>
		public ulong GuildId { get; set; }

		/// <summary>
		/// символ префикса для команд
		/// </summary>
		public char CommandPrefix { get; set; }

		/// <summary>
		/// Id канала приветствия
		/// </summary>
		public ulong WelcomeChannelId { get; set; }

		/// <summary>
		/// Id канала выдачи ролей
		/// </summary>
		public ulong RoleGainChannelId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ICollection<IEventHandler> Handlers { get; set; }

		public GuildSettings() {
			Handlers = new List<IEventHandler>();
		}

	}
}
