using XsarilAI.Events;

namespace XsarilAI.Handlers {
	public abstract class DerivedEventHandler<T> : AbstractEventHandler, IEventHandler<T> where T : IEvent {

		public override bool CanHandle(IEvent e) {
			return e is T;
		}

		public sealed override void Handle(IExecutionEnvironment environment, IEvent e) {
			Handle(environment, (T)e);
		}

		public abstract void Handle(IExecutionEnvironment environment, T e);
	}
}
