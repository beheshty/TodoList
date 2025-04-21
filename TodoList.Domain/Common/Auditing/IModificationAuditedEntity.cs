using System;

namespace TodoList.Domain.Common.Auditing;

public interface IModificationAuditedEntity
{
    DateTime? LastModificationTime { get; set; }
    Guid? LastModifierId { get; set; }
} 