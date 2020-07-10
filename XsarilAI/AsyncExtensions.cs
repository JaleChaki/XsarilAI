using System.Threading.Tasks;

namespace XsarilAI {
	public static class AsyncExtensions {

		public static void RunSync(this Task task) {
			task.GetAwaiter().GetResult();
		}

		public static T RunSync<T>(this Task<T> task) {
			return task.GetAwaiter().GetResult();
		}

	}
}
