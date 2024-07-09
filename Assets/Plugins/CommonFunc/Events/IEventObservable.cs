namespace Plugins.CommonFunc.Events
{
    public interface IEventObservable<out TEventSignal>
    {
        public void RegisterEventHandler(IEventHandler<TEventSignal> handler);
        public void UnregisterEventHandler(IEventHandler<TEventSignal> handler);        
    }
}