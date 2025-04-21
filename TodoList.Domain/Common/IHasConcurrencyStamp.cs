namespace TodoList.Domain.Common;

public interface IHasConcurrencyStamp
{
    public byte[] ConcurrencyStamp { get; set; }

}