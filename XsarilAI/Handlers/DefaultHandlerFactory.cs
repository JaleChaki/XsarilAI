using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace XsarilAI.Handlers {
	public class DefaultHandlerFactory : IHandlerFactory {

		public IEnumerable<IEventHandler> GetHandlerCollection(string collectionName) {
			if (collectionName == "music-service") {
				yield return new PlayMusicCommandHandler();
				yield return new PlayRandomMusicCommandHandler();
				yield return new StopMusicCommandHandler();
				yield break;
			}
			if (collectionName == "debug-utils") {
				yield return new DebugCommandHandler();
				yield break;
			}
			// @TODO make normal handler creation
			if (collectionName.StartsWith("rolegain")) {
				string filename = collectionName.Split('>')[1];
				var depends = JsonConvert.DeserializeObject<Dictionary<string, ulong>>(File.ReadAllText(filename));
				yield return new RoleGainEventHandler(depends);
				yield break;
			}
		}

	}
}
