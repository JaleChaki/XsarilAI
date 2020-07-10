using XsarilAI.Events;

namespace XsarilAI.Handlers {
	public abstract class AbstractEventHandler : IEventHandler {

		public abstract bool CanHandle(IEvent e);

		public abstract void Handle(IExecutionEnvironment environment, IEvent e);
	}
}
