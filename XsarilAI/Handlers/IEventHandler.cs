using XsarilAI.Events;

namespace XsarilAI.Handlers {
	public interface IEventHandler {

		bool CanHandle(IEvent e);

		void Handle(IExecutionEnvironment environment, IEvent e);

	}

	public interface IEventHandler<T> : IEventHandler where T : IEvent {

		void Handle(IExecutionEnvironment environment, T e);

	}
}
