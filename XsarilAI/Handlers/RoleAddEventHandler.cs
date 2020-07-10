using System;
using XsarilAI.Events;

namespace XsarilAI.Handlers {
	public class RoleAddEventHandler<T> : DerivedEventHandler<T> where T : IEvent {
		
		public override void Handle(IExecutionEnvironment environment, T e) {
			
		}
	}
}
