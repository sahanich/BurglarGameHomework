using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugins.CommonFunc.Events
{
    public class EventHandlingProcessor<TEventSignal> where TEventSignal: class
    {
        public readonly List<WeakReference<IEventHandler<TEventSignal>>> _handlers = new();
        
        public void RegisterHandler(IEventHandler<TEventSignal> handler)
        {
            if (_handlers.Any(handlerReference => 
                    handlerReference.TryGetTarget(out var target) && target == handler))
            {
                return;
            }
            RemoveNullHandlers();
            _handlers.Add(new WeakReference<IEventHandler<TEventSignal>>(handler));
        }
        
        public void UnregisterHandler(IEventHandler<TEventSignal> handler)
        {
            RemoveNullHandlers();
            _handlers.RemoveAll(handlerReference =>
                handlerReference.TryGetTarget(out var target) && target == handler);
        }
        
        public void FireEvent(TEventSignal eventValue)
        {
            RemoveNullHandlers();
            foreach (var handlerReference in _handlers)
            {
                if (handlerReference.TryGetTarget(out var handler))
                {
                    handler.HandleEvent(eventValue);
                }
            } 
        }

        private void RemoveNullHandlers()
        {
            _handlers.RemoveAll(handlerReference =>
                !handlerReference.TryGetTarget(out var target));            
        }
    }
}