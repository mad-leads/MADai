using System;

namespace MADai.Domain.Common;

public abstract class TenantEntity : AuditableEntity
{
	public Guid CompanyId { get; set; }
}
