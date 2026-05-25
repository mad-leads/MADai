using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Domain.SystemEntities;
using MADai.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Settings;

public class SettingsService : ISettingsService
{
	private readonly IDbContextAccess _db;

	public SettingsService(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<IReadOnlyList<SystemSettingDto>> ListAsync(string? categoryPrefix, CancellationToken cancellationToken = default(CancellationToken))
	{
		IQueryable<SystemSetting> query = _db.SystemSettings.AsNoTracking().AsQueryable();
		if (!string.IsNullOrWhiteSpace(categoryPrefix))
		{
			string prefix = categoryPrefix.Trim();
			query = query.Where((SystemSetting s) => EF.Functions.Like(s.Key, $"{prefix}%") || s.Category == prefix);
		}
		return await (from s in query
			orderby s.Category, s.Key
			select new SystemSettingDto(s.Key, s.Category, s.Value, s.DataType, s.Description)).ToListAsync(cancellationToken);
	}

	public async Task<IReadOnlyDictionary<string, string?>> UpdateBatchAsync(IReadOnlyDictionary<string, string?> updates, CancellationToken cancellationToken = default(CancellationToken))
	{
		string[] keys = updates.Keys.ToArray();
		Dictionary<string, SystemSetting> existingByKey = (await _db.SystemSettings.Where((SystemSetting s) => keys.Contains(s.Key)).ToListAsync(cancellationToken)).ToDictionary((SystemSetting s) => s.Key, (SystemSetting s) => s);
		foreach (var (key, value) in updates)
		{
			if (existingByKey.TryGetValue(key, out var setting))
			{
				setting.Value = value;
				continue;
			}
			DbSet<SystemSetting> systemSettings = _db.SystemSettings;
			SystemSetting systemSetting = new SystemSetting();
			systemSetting.Key = key;
			systemSetting.Value = value;
			systemSetting.Category = (key.StartsWith("claude") ? "Claude" : "General");
			SystemSetting systemSetting2 = systemSetting;
			bool flag = ((value == "true" || value == "false") ? true : false);
			systemSetting2.DataType = (flag ? "bool" : "string");
			systemSettings.Add(systemSetting);
		}
		await _db.SaveChangesAsync(cancellationToken);
		List<string> allKeys = updates.Keys.ToList();
		return await _db.SystemSettings.Where((SystemSetting s) => allKeys.Contains(s.Key)).ToDictionaryAsync((SystemSetting s) => s.Key, (SystemSetting s) => s.Value, cancellationToken);
	}
}
