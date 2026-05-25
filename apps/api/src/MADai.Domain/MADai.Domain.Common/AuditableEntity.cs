using System;

namespace MADai.Domain.Common;

public abstract class AuditableEntity : Entity
{
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


	public DateTime? ModifiedDate { get; set; }

	public DateTime? DeletedDate { get; set; }

	public Guid? CreatedByUserId { get; set; }

	public Guid? ModifiedByUserId { get; set; }

	public Guid? DeletedByUserId { get; set; }

	public bool IsDeleted { get; set; }

	public byte[]? RowVersion { get; set; }
}
