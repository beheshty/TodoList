using System;

namespace TodoList.Domain.Common.Auditing;

public interface ICreationAuditedEntity 
{
    DateTime CreationTime { get; set; }
    Guid? CreatorId { get; set; }
} 