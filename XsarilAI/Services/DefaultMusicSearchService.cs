using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XLogger;
using XsarilAI.Configuration;

namespace XsarilAI.Services {
	public class DefaultMusicSearchService : IMusicSearchService {

		protected struct MusicFile {

			public string Filename;

			public int RelevantIndex;

			public MusicFile(string filename, int relevantIndex) {
				Filename = filename;
				RelevantIndex = relevantIndex;
			}

		}

		protected static string RemoveSpecificSymbols(string s) {
			if (s == "-" || s == ",") {
				return null;
			}
			StringBuilder result = new StringBuilder();
			for (int i = 0; i < s.Length; ++i) {
				if (s[i] == '(' || s[i] == ')' || s[i] == '[' || s[i] == ']') {
					continue;
				}
				result.Append(s[i]);
			}
			return result.ToString();
		}


		protected readonly IConfiguration Configuration;

		public DefaultMusicSearchService(IConfiguration configuration) {
			this.Configuration = configuration;
		}

		public virtual IEnumerable<string> GetOrCreateMusicFiles(string searchStr) {
			string[] searchTags = searchStr.Split(' ').Select(x => RemoveSpecificSymbols(x)).Where(x => x != null).ToArray();
			List<MusicFile> files = Directory.GetFiles(Configuration["music.dir"]).Select(x => CreateMusicFile(x, searchTags)).ToList();
			files.Sort((a, b) => a.RelevantIndex.CompareTo(b.RelevantIndex));
			int max = files.Last().RelevantIndex;
			if (max == 0) {
				return new string[0];
			}
			foreach (var f in files) {
				Logger.Debug(f.Filename + " " + f.RelevantIndex);
			}
			return files.Where(x => x.RelevantIndex == max).Select(x => x.Filename);
		}

		protected virtual MusicFile CreateMusicFile(string filename, IEnumerable<string> searchTags) {
			int relevantIndex = 0;
			string lowerFileName = filename.ToLower();
			foreach (string tag in searchTags) {
				if (lowerFileName.Contains(tag.ToLower())) {
					++relevantIndex;
				}
			}
			return new MusicFile(filename, relevantIndex);
		}
	}
}
