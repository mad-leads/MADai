using System;
using System.Collections.Generic;
using MADai.Application.Abstractions;
using MADai.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MADai.Infrastructure.Persistence;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MADaiDbContext>
{
	private class NoUserService : ICurrentUserService
	{
		public Guid? UserId => null;

		public string? Email => null;

		public Guid? CompanyId => null;

		public IReadOnlyList<string> Roles => Array.Empty<string>();

		public IReadOnlyList<string> Permissions => Array.Empty<string>();

		public bool IsAuthenticated => false;

		public bool IsInRole(string role)
		{
			return false;
		}

		public bool HasPermission(string permission)
		{
			return false;
		}
	}

	public MADaiDbContext CreateDbContext(string[] args)
	{
		return new MADaiDbContext(new DbContextOptionsBuilder<MADaiDbContext>().UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=MADai;Trusted_Connection=True;TrustServerCertificate=True;", delegate(SqlServerDbContextOptionsBuilder sql)
		{
			sql.MigrationsAssembly(typeof(MADaiDbContext).Assembly.GetName().Name);
		}).Options, new AuditingInterceptor(new NoUserService()));
	}
}
