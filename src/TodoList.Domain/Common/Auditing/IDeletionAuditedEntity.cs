using System;

namespace TodoList.Domain.Common.Auditing;

public interface IDeletionAuditedEntity
{
    Guid? DeleterId { get; set; }
    DateTime? DeletionTime { get; set; }
} 