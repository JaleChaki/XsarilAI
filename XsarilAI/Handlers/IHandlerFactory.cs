using System.Collections.Generic;

namespace XsarilAI.Handlers {
	public interface IHandlerFactory {

		IEnumerable<IEventHandler> GetHandlerCollection(string collectionName);

	}
}
