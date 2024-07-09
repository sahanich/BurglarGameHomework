namespace Plugins.CommonFunc.Events
{
    public interface IEventHandler<in TEventValue>
    {
        public void HandleEvent(TEventValue value);
    }
}