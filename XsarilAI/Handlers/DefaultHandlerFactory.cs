using System.Collections.Generic;

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
		}

	}
}
