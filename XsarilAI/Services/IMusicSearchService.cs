using System.Collections.Generic;

namespace XsarilAI.Services {
	interface IMusicSearchService {

		IEnumerable<string> GetOrCreateMusicFiles(string searchStr);

	}
}
