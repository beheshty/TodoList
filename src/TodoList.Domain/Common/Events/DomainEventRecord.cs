namespace TodoList.Domain.Common.Events;

public class DomainEventRecord(
    object eventData,
    long eventOrder,
    EventPublishType dispatchType = EventPublishType.Local)
    : IDomainEventData
{
    public object EventData { get; } = eventData;
    public long EventOrder { get; } = eventOrder;
    public EventPublishType DispatchType { get; } = dispatchType;
}