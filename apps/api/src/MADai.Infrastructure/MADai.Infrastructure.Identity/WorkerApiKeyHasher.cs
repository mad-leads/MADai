using System;
using System.Security.Cryptography;
using System.Text;
using MADai.Application.Abstractions;

namespace MADai.Infrastructure.Identity;

public class WorkerApiKeyHasher : IWorkerApiKeyHasher
{
	private const string Prefix = "wk_";

	public string Generate()
	{
		byte[] bytes = RandomNumberGenerator.GetBytes(40);
		return "wk_" + Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_")
			.TrimEnd('=');
	}

	public string Hash(string apiKey)
	{
		using SHA256 sha = SHA256.Create();
		return Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(apiKey)));
	}

	public bool Verify(string apiKey, string hash)
	{
		string computed = Hash(apiKey);
		return CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(computed), Encoding.UTF8.GetBytes(hash));
	}
}
